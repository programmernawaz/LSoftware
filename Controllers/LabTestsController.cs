using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Entities;
using PathLabAPI.Dto;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabTestsController : ControllerBase
    {
        private readonly PathLabDbContext _context;
        public LabTestsController(PathLabDbContext context) => _context = context;

        // ---------- Request DTOs (local, simple) ----------
        // If you already have shared request DTOs, remove these and use the shared types.
        public class CreateParameterRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Unit { get; set; } = string.Empty;
            public string ReferenceRange { get; set; } = string.Empty;
        }

        public class CreateLabTestRequest
        {
            public string Name { get; set; } = string.Empty;
            public string? Category { get; set; }
            public decimal? Price { get; set; }
            public List<CreateParameterRequest> Parameters { get; set; } = new();
        }

        public class UpdateLabTestRequest
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Category { get; set; }
            public decimal? Price { get; set; }
            public List<CreateParameterRequest> Parameters { get; set; } = new();
        }

        // --------------------------
        // GET: api/LabTests
        // --------------------------
        [HttpGet]
        public async Task<IActionResult> GetTests()
        {
            var tests = await _context.LabTests
                .Include(t => t.Parameters)
                .ToListAsync();

            var dtoList = tests.Select(t => new LabTestDto
            {
                Id = t.Id,
                Name = t.Name,
                Category = t.Category,
                Parameters = t.Parameters.Select(p => new ParameterDto
                {
                    Id = p.Id,
                    Name = p.ParameterName,
                    Unit = p.Unit,
                    ReferenceRange = p.ReferenceRange
                }).ToList()
            }).ToList();

            return Ok(dtoList);
        }

        // --------------------------
        // GET: api/LabTests/{id}
        // --------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTest(int id)
        {
            var test = await _context.LabTests
                .Include(t => t.Parameters)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null) return NotFound();

            var dto = new LabTestDto
            {
                Id = test.Id,
                Name = test.Name,
                Category = test.Category,
                Parameters = test.Parameters.Select(p => new ParameterDto
                {
                    Id = p.Id,
                    Name = p.ParameterName,
                    Unit = p.Unit,
                    ReferenceRange = p.ReferenceRange
                }).ToList()
            };

            return Ok(dto);
        }

        // --------------------------
        // POST: api/LabTests
        // --------------------------
        [HttpPost]
        public async Task<IActionResult> CreateTest([FromBody] CreateLabTestRequest request)
        {
            if (request == null) return BadRequest("Request body null.");
            if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest("Name is required.");
            if (request.Parameters == null || request.Parameters.Count == 0) return BadRequest("At least one parameter is required.");

            var entity = new LabTest
            {
                Name = request.Name,
                Category = request.Category ?? string.Empty,
                Price = request.Price ?? 0M,
                Parameters = request.Parameters.Select(p => new TestParameter
                {
                    ParameterName = p.Name,
                    Unit = p.Unit,
                    ReferenceRange = p.ReferenceRange
                }).ToList()
            };

            try
            {
                _context.LabTests.Add(entity);
                await _context.SaveChangesAsync();

                var createdDto = new LabTestDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Category = entity.Category,
                    Parameters = entity.Parameters.Select(p => new ParameterDto
                    {
                        Id = p.Id,
                        Name = p.ParameterName,
                        Unit = p.Unit,
                        ReferenceRange = p.ReferenceRange
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetTest), new { id = entity.Id }, createdDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Save failed", detail = ex.Message });
            }
        }

        // --------------------------
        // PUT: api/LabTests/{id}
        // --------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTest(int id, [FromBody] UpdateLabTestRequest request)
        {
            if (request == null) return BadRequest("Request body null.");
            if (id != request.Id) return BadRequest("Id mismatch.");
            if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest("Name is required.");

            var entity = await _context.LabTests
                .Include(t => t.Parameters)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null) return NotFound();

            // update parent
            entity.Name = request.Name;
            entity.Category = request.Category ?? entity.Category;
            entity.Price = request.Price ?? entity.Price;

            // remove existing parameters (simple and reliable)
            if (entity.Parameters != null && entity.Parameters.Count > 0)
            {
                // remove via Set<TestParameter>() so DbSet<TestParameter> is not required in DbContext
                _context.Set<TestParameter>().RemoveRange(entity.Parameters);
            }

            // add new parameters
            entity.Parameters = request.Parameters?.Select(p => new TestParameter
            {
                ParameterName = p.Name,
                Unit = p.Unit,
                ReferenceRange = p.ReferenceRange
            }).ToList() ?? new List<TestParameter>();

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.LabTests.AnyAsync(e => e.Id == id)) return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Update failed", detail = ex.Message });
            }
        }

        // --------------------------
        // DELETE: api/LabTests/{id}
        // --------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var entity = await _context.LabTests
                .Include(t => t.Parameters)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null) return NotFound();

            try
            {
                if (entity.Parameters != null && entity.Parameters.Count > 0)
                {
                    _context.Set<TestParameter>().RemoveRange(entity.Parameters);
                }

                _context.LabTests.Remove(entity);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Delete failed", detail = ex.Message });
            }
        }
    }
}

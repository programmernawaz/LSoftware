using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;
using PathLabAPI.Entities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestResultsController : Controller
    {
        private readonly PathLabDbContext _context;
        public TestResultsController(PathLabDbContext context) => _context = context;

        // GET: api/testresults
        [HttpGet]
        public async Task<IActionResult> GetResults()
        {
            var results = await _context.TestResults
                .Include(r => r.TestOrderItem)
                .Include(r => r.TestParameter)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = results.Select(r => new TestResultDto
            {
                Id = r.Id,
                TestOrderItemId = r.TestOrderItemId,
                TestParameterId = r.TestParameterId,
                Value = r.Value,
                Flag = r.Flag,
                TestOrderItem = r.TestOrderItem == null ? null : new TestOrderItemDto
                {
                    Id = r.TestOrderItem.Id,
                    LabTestId = r.TestOrderItem.LabTestId,
                    // keep LabTest null here to avoid heavy payload; include if you want
                },
                TestParameter = r.TestParameter == null ? null : new ParameterDto
                {
                    Id = r.TestParameter.Id,
                    Name = r.TestParameter.ParameterName,
                    Unit = r.TestParameter.Unit,
                    ReferenceRange = r.TestParameter.ReferenceRange
                }
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/testresults/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetResult([FromRoute] int id)
        {
            var r = await _context.TestResults
                .Include(x => x.TestOrderItem)
                .Include(x => x.TestParameter)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (r == null) return NotFound();

            var dto = new TestResultDto
            {
                Id = r.Id,
                TestOrderItemId = r.TestOrderItemId,
                TestParameterId = r.TestParameterId,
                Value = r.Value,
                Flag = r.Flag,
                TestOrderItem = r.TestOrderItem == null ? null : new TestOrderItemDto
                {
                    Id = r.TestOrderItem.Id,
                    LabTestId = r.TestOrderItem.LabTestId
                },
                TestParameter = r.TestParameter == null ? null : new ParameterDto
                {
                    Id = r.TestParameter.Id,
                    Name = r.TestParameter.ParameterName,
                    Unit = r.TestParameter.Unit,
                    ReferenceRange = r.TestParameter.ReferenceRange
                }
            };

            return Ok(dto);
        }

        // POST: api/testresults
        [HttpPost]
        public async Task<IActionResult> CreateResult([FromBody] TestResult body)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.TestResults.Add(body);
            await _context.SaveChangesAsync();

            // Return created resource (mapped)
            var created = await _context.TestResults
                .Include(x => x.TestOrderItem)
                .Include(x => x.TestParameter)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == body.Id);

            var dto = new TestResultDto
            {
                Id = created!.Id,
                TestOrderItemId = created.TestOrderItemId,
                TestParameterId = created.TestParameterId,
                Value = created.Value,
                Flag = created.Flag,
                TestOrderItem = created.TestOrderItem == null ? null : new TestOrderItemDto
                {
                    Id = created.TestOrderItem.Id,
                    LabTestId = created.TestOrderItem.LabTestId
                },
                TestParameter = created.TestParameter == null ? null : new ParameterDto
                {
                    Id = created.TestParameter.Id,
                    Name = created.TestParameter.ParameterName,
                    Unit = created.TestParameter.Unit,
                    ReferenceRange = created.TestParameter.ReferenceRange
                }
            };

            return CreatedAtAction(nameof(GetResult), new { id = dto.Id }, dto);
        }

        // PUT: api/testresults/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateResult([FromRoute] int id, [FromBody] TestResult body)
        {
            var entity = await _context.TestResults.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            // Update allowed fields
            entity.TestOrderItemId = body.TestOrderItemId;
            entity.TestParameterId = body.TestParameterId;
            entity.Value = body.Value;
            entity.Flag = body.Flag;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/testresults/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteResult([FromRoute] int id)
        {
            var entity = await _context.TestResults.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _context.TestResults.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    // --- small response DTO for TestResult ---
    public class TestResultDto
    {
        public int Id { get; set; }
        public int TestOrderItemId { get; set; }
        public int TestParameterId { get; set; }
        public string? Value { get; set; }
        public string? Flag { get; set; }

        public TestOrderItemDto? TestOrderItem { get; set; }
        public ParameterDto? TestParameter { get; set; }
        public string? Notes { get; internal set; }
    }
}

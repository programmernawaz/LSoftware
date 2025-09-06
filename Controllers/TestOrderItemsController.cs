using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;
using PathLabAPI.Entities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestOrderItemsController : Controller
    {
        private readonly PathLabDbContext _context;
        public TestOrderItemsController(PathLabDbContext context) => _context = context;

        // GET: api/testorderitems
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _context.TestOrderItems
                .Include(i => i.LabTest).ThenInclude(t => t.Parameters)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = items.Select(i => new TestOrderItemDto
            {
                Id = i.Id,
                LabTestId = i.LabTestId,
                LabTest = i.LabTest == null ? null : new LabTestDto
                {
                    Id = i.LabTest.Id,
                    Name = i.LabTest.Name,
                    Parameters = i.LabTest.Parameters.Select(p => new ParameterDto
                    {
                        Id = p.Id,
                        Name = p.ParameterName,
                        Unit = p.Unit,
                        ReferenceRange = p.ReferenceRange
                    }).ToList()
                }
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/testorderitems/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetItem([FromRoute] int id)
        {
            var item = await _context.TestOrderItems
                .Include(i => i.LabTest).ThenInclude(t => t.Parameters)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null) return NotFound();

            var dto = new TestOrderItemDto
            {
                Id = item.Id,
                LabTestId = item.LabTestId,
                LabTest = item.LabTest == null ? null : new LabTestDto
                {
                    Id = item.LabTest.Id,
                    Name = item.LabTest.Name,
                    Parameters = item.LabTest.Parameters.Select(p => new ParameterDto
                    {
                        Id = p.Id,
                        Name = p.ParameterName,
                        Unit = p.Unit,
                        ReferenceRange = p.ReferenceRange
                    }).ToList()
                }
            };

            return Ok(dto);
        }

        // POST: api/testorderitems
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] TestOrderItem body)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.TestOrderItems.Add(body);
            await _context.SaveChangesAsync();

            // Return the created entity (you can change to Created DTO if you prefer)
            return CreatedAtAction(nameof(GetItem), new { id = body.Id }, body);
        }

        // PUT: api/testorderitems/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] TestOrderItem body)
        {
            var entity = await _context.TestOrderItems.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            // update allowed fields
            entity.TestOrderId = body.TestOrderId;
            entity.LabTestId = body.LabTestId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/testorderitems/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id)
        {
            var entity = await _context.TestOrderItems.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _context.TestOrderItems.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

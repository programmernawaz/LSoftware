using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;
using PathLabAPI.Entities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestParametersController : Controller
    {
        private readonly PathLabDbContext _context;
        public TestParametersController(PathLabDbContext context) => _context = context;

        // GET: api/testparameters
        [HttpGet]
        public async Task<IActionResult> GetParameters()
        {
            var parameters = await _context.TestParameters
                .Include(p => p.LabTest)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = parameters.Select(p => new ParameterDto
            {
                Id = p.Id,
                Name = p.ParameterName,
                Unit = p.Unit,
                ReferenceRange = p.ReferenceRange
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/testparameters/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetParameter([FromRoute] int id)
        {
            var p = await _context.TestParameters
                .Include(x => x.LabTest)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return NotFound();

            var dto = new ParameterDto
            {
                Id = p.Id,
                Name = p.ParameterName,
                Unit = p.Unit,
                ReferenceRange = p.ReferenceRange
            };

            return Ok(dto);
        }

        // POST: api/testparameters
        [HttpPost]
        public async Task<IActionResult> CreateParameter([FromBody] TestParameter body)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.TestParameters.Add(body);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParameter), new { id = body.Id }, new ParameterDto
            {
                Id = body.Id,
                Name = body.ParameterName,
                Unit = body.Unit,
                ReferenceRange = body.ReferenceRange
            });
        }

        // PUT: api/testparameters/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateParameter([FromRoute] int id, [FromBody] TestParameter body)
        {
            var entity = await _context.TestParameters.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.LabTestId = body.LabTestId;
            entity.ParameterName = body.ParameterName;
            entity.Unit = body.Unit;
            entity.ReferenceRange = body.ReferenceRange;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/testparameters/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteParameter([FromRoute] int id)
        {
            var entity = await _context.TestParameters.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _context.TestParameters.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

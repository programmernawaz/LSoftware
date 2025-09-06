using Microsoft.AspNetCore.Mvc;
using PathLabAPI.Data;
using PathLabAPI.Entities;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Dto;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabTestsController : Controller
    {
        private readonly PathLabDbContext _context;
        public LabTestsController(PathLabDbContext context) => _context = context;

        // GET: api/LabTests
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


        // GET: api/LabTests/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTest([FromRoute] int id)
        {
            var test = await _context.LabTests
                .Include(t => t.Parameters)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null) return NotFound();

            var dto = new LabTestDto
            {
                Id = test.Id,
                Name = test.Name,
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


    }
}

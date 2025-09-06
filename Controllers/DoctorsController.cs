using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;
using PathLabAPI.Entities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : Controller
    {
        private readonly PathLabDbContext _context;
        public DoctorsController(PathLabDbContext context) => _context = context;

        // GET: api/doctors
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Doctors
                .AsNoTracking()
                .ToListAsync();

            var dtoList = doctors.Select(d => new DoctorDto
            {
                Id = d.Id,
                Name = d.Name,
                Specialization = d.Specialization
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/doctors/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDoctor([FromRoute] int id)
        {
            var d = await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (d == null) return NotFound();

            var dto = new DoctorDto
            {
                Id = d.Id,
                Name = d.Name,
                Specialization = d.Specialization
            };

            return Ok(dto);
        }

        // POST: api/doctors
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorDto body)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var entity = new Doctor
            {
                Name = body.Name,
                Specialization = body.Specialization
            };

            _context.Doctors.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = entity.Id }, new DoctorDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Specialization = entity.Specialization
            });
        }

        // PUT: api/doctors/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDoctor([FromRoute] int id, [FromBody] DoctorDto body)
        {
            var entity = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.Name = body.Name;
            entity.Specialization = body.Specialization;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/doctors/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int id)
        {
            var entity = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _context.Doctors.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

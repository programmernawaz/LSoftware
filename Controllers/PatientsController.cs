using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;
using PathLabAPI.Entities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : Controller
    {
        private readonly PathLabDbContext _context;
        public PatientsController(PathLabDbContext context) => _context = context;

        // GET: api/patients
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _context.Patients
                .AsNoTracking()
                .ToListAsync();

            var dtoList = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                PatientCode = p.PatientCode,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                Phone = p.Phone
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/patients/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPatient([FromRoute] int id)
        {
            var p = await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return NotFound();

            var dto = new PatientDto
            {
                Id = p.Id,
                PatientCode = p.PatientCode,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                Phone = p.Phone
            };

            return Ok(dto);
        }

        // POST: api/patients
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] PatientDto body)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var entity = new Patient
            {
                PatientCode = body.PatientCode,
                Name = body.Name,
                Age = body.Age,
                Gender = body.Gender,
                Phone = body.Phone
            };

            _context.Patients.Add(entity);
            await _context.SaveChangesAsync();

            // Return 201 with location header to GET /api/patients/{id}
            return CreatedAtAction(nameof(GetPatient), new { id = entity.Id }, new PatientDto
            {
                Id = entity.Id,
                PatientCode = entity.PatientCode,
                Name = entity.Name,
                Age = entity.Age,
                Gender = entity.Gender,
                Phone = entity.Phone
            });
        }

        // PUT: api/patients/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePatient([FromRoute] int id, [FromBody] PatientDto body)
        {
            var entity = await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.PatientCode = body.PatientCode;
            entity.Name = body.Name;
            entity.Age = body.Age;
            entity.Gender = body.Gender;
            entity.Phone = body.Phone;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/patients/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePatient([FromRoute] int id)
        {
            var entity = await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _context.Patients.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

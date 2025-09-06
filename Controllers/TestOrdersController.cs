using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;
using PathLabAPI.Entities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestOrdersController : Controller
    {
        private readonly PathLabDbContext _context;
        public TestOrdersController(PathLabDbContext context) => _context = context;

        // GET: api/testorders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.TestOrders
                .Include(o => o.Patient)
                .Include(o => o.Doctor)
                .Include(o => o.Items).ThenInclude(i => i.LabTest).ThenInclude(t => t.Parameters)
                .Include(o => o.Invoice)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = orders.Select(o => new TestOrderDto
            {
                Id = o.Id,
                PatientId = o.PatientId,
                DoctorId = o.DoctorId,
                OrderDate = o.OrderDate,
                Patient = o.Patient == null ? null : new PatientDto
                {
                    Id = o.Patient.Id,
                    PatientCode = o.Patient.PatientCode,
                    Name = o.Patient.Name,
                    Age = o.Patient.Age,
                    Gender = o.Patient.Gender,
                    Phone = o.Patient.Phone
                },
                Doctor = o.Doctor == null ? null : new DoctorDto
                {
                    Id = o.Doctor.Id,
                    Name = o.Doctor.Name,
                    Specialization = o.Doctor.Specialization
                },
                Items = o.Items.Select(i => new TestOrderItemDto
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
                }).ToList(),
                Invoice = o.Invoice == null ? null : new InvoiceDto
                {
                    Id = o.Invoice.Id,
                    TestOrderId = o.Invoice.TestOrderId,
                    InvoiceDate = o.Invoice.InvoiceDate,
                    TotalAmount = o.Invoice.TotalAmount,
                    PaidAmount = o.Invoice.PaidAmount
                }
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/testorders/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            var o = await _context.TestOrders
                .Include(x => x.Patient)
                .Include(x => x.Doctor)
                .Include(x => x.Items).ThenInclude(i => i.LabTest).ThenInclude(t => t.Parameters)
                .Include(x => x.Invoice)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (o == null) return NotFound();

            var dto = new TestOrderDto
            {
                Id = o.Id,
                PatientId = o.PatientId,
                DoctorId = o.DoctorId,
                OrderDate = o.OrderDate,
                Patient = o.Patient == null ? null : new PatientDto
                {
                    Id = o.Patient.Id,
                    PatientCode = o.Patient.PatientCode,
                    Name = o.Patient.Name,
                    Age = o.Patient.Age,
                    Gender = o.Patient.Gender,
                    Phone = o.Patient.Phone
                },
                Doctor = o.Doctor == null ? null : new DoctorDto
                {
                    Id = o.Doctor.Id,
                    Name = o.Doctor.Name,
                    Specialization = o.Doctor.Specialization
                },
                Items = o.Items.Select(i => new TestOrderItemDto
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
                }).ToList(),
                Invoice = o.Invoice == null ? null : new InvoiceDto
                {
                    Id = o.Invoice.Id,
                    TestOrderId = o.Invoice.TestOrderId,
                    InvoiceDate = o.Invoice.InvoiceDate,
                    TotalAmount = o.Invoice.TotalAmount,
                    PaidAmount = o.Invoice.PaidAmount
                }
            };

            return Ok(dto);
        }

        // POST: api/testorders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] TestOrder body)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.TestOrders.Add(body);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = body.Id }, body);
        }

        // PUT: api/testorders/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] TestOrder body)
        {
            var entity = await _context.TestOrders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.PatientId = body.PatientId;
            entity.DoctorId = body.DoctorId;
            entity.OrderDate = body.OrderDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/testorders/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var entity = await _context.TestOrders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _context.TestOrders.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

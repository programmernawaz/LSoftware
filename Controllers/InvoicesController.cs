using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;
using PathLabAPI.Entities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : Controller
    {
        private readonly PathLabDbContext _context;
        public InvoicesController(PathLabDbContext context) => _context = context;

        // GET: api/invoices
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _context.Invoices
                .Include(i => i.TestOrder)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                TestOrderId = i.TestOrderId,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount,
                PaidAmount = i.PaidAmount
                // Balance is computed in DTO (TotalAmount - PaidAmount)
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/invoices/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInvoice([FromRoute] int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.TestOrder)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null) return NotFound();

            var dto = new InvoiceDto
            {
                Id = invoice.Id,
                TestOrderId = invoice.TestOrderId,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                PaidAmount = invoice.PaidAmount
            };

            return Ok(dto);
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice body)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            _context.Invoices.Add(body);
            await _context.SaveChangesAsync();

            var dto = new InvoiceDto
            {
                Id = body.Id,
                TestOrderId = body.TestOrderId,
                InvoiceDate = body.InvoiceDate,
                TotalAmount = body.TotalAmount,
                PaidAmount = body.PaidAmount
            };

            return CreatedAtAction(nameof(GetInvoice), new { id = body.Id }, dto);
        }

        // PUT: api/invoices/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateInvoice([FromRoute] int id, [FromBody] Invoice body)
        {
            var entity = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.TestOrderId = body.TestOrderId;
            entity.InvoiceDate = body.InvoiceDate;
            entity.TotalAmount = body.TotalAmount;
            entity.PaidAmount = body.PaidAmount;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/invoices/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInvoice([FromRoute] int id)
        {
            var entity = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _context.Invoices.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

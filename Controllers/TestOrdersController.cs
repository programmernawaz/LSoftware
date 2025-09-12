using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;
using PathLabAPI.Entities;

namespace PathLabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestOrdersController : ControllerBase
    {
        private readonly PathLabDbContext _context;
        private readonly ILogger<TestOrdersController> _logger;

        public TestOrdersController(PathLabDbContext context, ILogger<TestOrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/testorders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.TestOrders
                .Include(o => o.Patient)
                .Include(o => o.Doctor)
                .Include(o => o.Items).ThenInclude(i => i.LabTest).ThenInclude(t => t.Parameters)
                .Include(o => o.Items).ThenInclude(i => i.Results)
                .Include(o => o.Invoice)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = orders.Select(MapToDto).ToList();
            return Ok(dtoList);
        }

        // GET: api/testorders/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            var o = await _context.TestOrders
                .Include(x => x.Patient)
                .Include(x => x.Doctor)
                .Include(x => x.Items).ThenInclude(i => i.LabTest).ThenInclude(t => t.Parameters)
                .Include(x => x.Items).ThenInclude(i => i.Results)
                .Include(x => x.Invoice)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (o == null) return NotFound();

            var dto = MapToDto(o);
            return Ok(dto);
        }

        // Lightweight request DTOs for POST
        public class CreateTestOrderRequest
        {
            public PatientDto Patient { get; set; } = new PatientDto();
            public int? DoctorId { get; set; }
            public DateTime? OrderDate { get; set; }
            public List<CreateTestOrderItemRequest> Items { get; set; } = new();
        }

        public class CreateTestOrderItemRequest
        {
            public int LabTestId { get; set; }
            public List<CreateTestResultRequest> Results { get; set; } = new();
        }

        public class CreateTestResultRequest
        {
            public int TestParameterId { get; set; }
            public string Value { get; set; } = string.Empty;
            public string? Notes { get; set; }
        }

        // POST: api/testorders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateTestOrderRequest req)
        {
            if (req == null) return BadRequest("Request body required.");
            if (req.Items == null || !req.Items.Any()) return BadRequest("At least one test item is required.");
            if (req.Patient == null || string.IsNullOrWhiteSpace(req.Patient.Name))
                return BadRequest("Patient name is required.");

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Creating order for patient {Patient}", req.Patient.Name);

                // 1) create or find patient
                Patient patientEntity;
                if (req.Patient.Id > 0)
                {
                    patientEntity = await _context.Patients.FindAsync(req.Patient.Id)
                        ?? throw new ApplicationException($"Patient with Id {req.Patient.Id} not found.");
                }
                else
                {
                    patientEntity = new Patient
                    {
                        PatientCode = string.IsNullOrWhiteSpace(req.Patient.PatientCode) ? GeneratePatientCode() : req.Patient.PatientCode,
                        Name = req.Patient.Name.Trim(),
                        Age = req.Patient.Age,
                        Gender = req.Patient.Gender ?? string.Empty,
                        Phone = req.Patient.Phone ?? string.Empty
                    };
                    _context.Patients.Add(patientEntity);
                    await _context.SaveChangesAsync();
                }

                // 2) validate doctor if provided
                int? doctorId = null;
                if (req.DoctorId.HasValue && req.DoctorId.Value > 0)
                {
                    var docExists = await _context.Doctors.AnyAsync(d => d.Id == req.DoctorId.Value);
                    if (!docExists)
                    {
                        await tx.RollbackAsync();
                        return BadRequest($"Doctor with id {req.DoctorId.Value} not found.");
                    }
                    doctorId = req.DoctorId.Value;
                }

                // 3) create order
                var order = new TestOrder
                {
                    PatientId = patientEntity.Id,
                    DoctorId = doctorId,
                    OrderDate = req.OrderDate ?? DateTime.UtcNow
                };
                _context.TestOrders.Add(order);
                await _context.SaveChangesAsync(); // set order.Id

                // 4) prefetch param ids for validation
                var requestedTestIds = req.Items.Select(i => i.LabTestId).Distinct().ToList();
                var paramLookup = await _context.TestParameters
                    .Where(p => requestedTestIds.Contains(p.LabTestId))
                    .GroupBy(p => p.LabTestId)
                    .ToDictionaryAsync(g => g.Key, g => g.Select(p => p.Id).ToHashSet());

                // 5) create items
                var items = new List<TestOrderItem>();
                foreach (var itemReq in req.Items)
                {
                    var testExists = await _context.LabTests.AnyAsync(lt => lt.Id == itemReq.LabTestId);
                    if (!testExists)
                    {
                        await tx.RollbackAsync();
                        return BadRequest($"LabTest with id {itemReq.LabTestId} not found.");
                    }

                    items.Add(new TestOrderItem
                    {
                        TestOrderId = order.Id,
                        LabTestId = itemReq.LabTestId
                    });
                }
                _context.TestOrderItems.AddRange(items);
                await _context.SaveChangesAsync(); // now items have ids

                // 6) validate and add any initial results supplied
                var resultsToAdd = new List<TestResult>();
                for (int idx = 0; idx < req.Items.Count; idx++)
                {
                    var itemReq = req.Items[idx];
                    var savedItem = items[idx];

                    if (itemReq.Results == null || !itemReq.Results.Any()) continue;

                    if (!paramLookup.TryGetValue(itemReq.LabTestId, out var validParamIds))
                    {
                        validParamIds = new HashSet<int>();
                    }

                    foreach (var r in itemReq.Results)
                    {
                        if (!validParamIds.Contains(r.TestParameterId))
                        {
                            await tx.RollbackAsync();
                            return BadRequest($"Parameter id {r.TestParameterId} does not belong to LabTest {itemReq.LabTestId}.");
                        }

                        resultsToAdd.Add(new TestResult
                        {
                            TestOrderItemId = savedItem.Id,
                            TestParameterId = r.TestParameterId,
                            Value = r.Value ?? string.Empty,
                            Notes = r.Notes
                        });
                    }
                }

                if (resultsToAdd.Any())
                {
                    _context.TestResults.AddRange(resultsToAdd);
                    await _context.SaveChangesAsync();
                }

                await tx.CommitAsync();
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, new { orderId = order.Id });
            }
            catch (ApplicationException aex)
            {
                try { await tx.RollbackAsync(); } catch { }
                _logger.LogWarning(aex, "Business error creating order");
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                try { await tx.RollbackAsync(); } catch { }
                _logger.LogError(ex, "Error creating test order");
#if DEBUG
                return StatusCode(500, new { message = ex.Message, exception = ex.ToString(), inner = ex.InnerException?.ToString() });
#else
                return StatusCode(500, "An error occurred while creating the test order.");
#endif
            }
        }

        // POST: api/testorders/{orderId}/items/{itemId}/results
        public class AddResultsRequest
        {
            public int TestParameterId { get; set; }
            public string? Value { get; set; }
            public string? Notes { get; set; }
        }

        [HttpPost("{orderId:int}/items/{itemId:int}/results")]
        public async Task<IActionResult> AddResults([FromRoute] int orderId, [FromRoute] int itemId, [FromBody] List<AddResultsRequest> req)
        {
            if (req == null || !req.Any()) return BadRequest("No results provided.");

            var item = await _context.TestOrderItems
                .Include(i => i.LabTest).ThenInclude(lt => lt.Parameters)
                .Include(i => i.Results)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.TestOrderId == orderId);

            if (item == null) return NotFound("Order item not found.");

            var validParamIds = item.LabTest?.Parameters.Select(p => p.Id).ToHashSet() ?? new HashSet<int>();
            foreach (var r in req)
            {
                if (!validParamIds.Contains(r.TestParameterId))
                    return BadRequest($"Parameter id {r.TestParameterId} does not belong to LabTest {item.LabTestId}.");
            }

            var entities = req.Select(r => new TestResult
            {
                TestOrderItemId = itemId,
                TestParameterId = r.TestParameterId,
                Value = r.Value ?? string.Empty,
                Notes = r.Notes
            }).ToList();

            _context.TestResults.AddRange(entities);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // PUT: api/testorders/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] TestOrder update)
        {
            var entity = await _context.TestOrders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.PatientId = update.PatientId;
            entity.DoctorId = update.DoctorId;
            entity.OrderDate = update.OrderDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/testorders/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var entity = await _context.TestOrders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _context.TestOrders.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // helper mapper (uses DTOs from PathLabAPI.Dto explicitly to avoid collisions)
        private TestOrderDto MapToDto(TestOrder o)
        {
            return new TestOrderDto
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
                        Category = i.LabTest.Category,
                        Parameters = i.LabTest.Parameters.Select(p => new ParameterDto
                        {
                            Id = p.Id,
                            Name = p.ParameterName,
                            Unit = p.Unit,
                            ReferenceRange = p.ReferenceRange
                        }).ToList()
                    },
                    Results = i.Results?.Select(r => new PathLabAPI.Dto.TestResultDto
                    {
                        Id = r.Id,
                        TestOrderItemId = r.TestOrderItemId,
                        TestParameterId = r.TestParameterId,
                        Value = r.Value,
                        Notes = r.Notes,
                        Flag = r.Flag
                    }).ToList() ?? new List<PathLabAPI.Dto.TestResultDto>()
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
        }

        private string GeneratePatientCode() => "P-" + DateTime.UtcNow.ToString("yyMMddHHmmss");
    }
}

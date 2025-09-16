using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathLabAPI.Data;
using PathLabAPI.Dto;

namespace PathLabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly PathLabDbContext _context;
        public ReportController(PathLabDbContext context)
        {
            _context = context;
        }

        [HttpGet("TestReports")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetTestReports()
        {
            var data = await (from to in _context.TestOrders
                              join d in _context.Doctors on to.DoctorId equals d.Id
                              join p in _context.Patients on to.PatientId equals p.Id
                              join toi in _context.TestOrderItems on to.Id equals toi.TestOrderId
                              join l in _context.LabTests on toi.LabTestId equals l.Id
                              select new ReportDto
                              {
                                  PatientId = p.Id,
                                  PatientName = p.Name,
                                  DoctorName = d.Name,
                                  TestName = l.Name
                              }).ToListAsync();

            return Ok(data);
        }
    }

}

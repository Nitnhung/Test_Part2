using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhanCongViec.Abstract;
using PhanCongViec.Data;
using PhanCongViec.DTOs;
using PhanCongViec.Models;

namespace PhanCongViec.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class WorkboardsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWorkboardService _iservice;
        public WorkboardsController(AppDbContext context, IWorkboardService iservice)
        {
            _context = context;
            _iservice = iservice;
        }



        [HttpGet("getdb")]
        public async Task<ActionResult<IEnumerable<Developer>>> GetDeveloper()
        {
            var developer = await _context.Developers
                .AsNoTracking()
                .ToListAsync();

            var respone = ResponeFactory.Success
                (HttpContext.TraceIdentifier, developer);

            return Ok(respone);
        }



        [HttpGet("HealthCheck")]
        public async Task<ActionResult> HeathCheck()
        {
            bool check = await _context.Database.CanConnectAsync();
            if (check)
            {
                return Ok(ResponeFactory.Success(HttpContext.TraceIdentifier, new { Data = "true" }));
            }
            return BadRequest(ResponeFactory.Fail(HttpContext.TraceIdentifier, 503, "can not conect"));
        }



        [HttpPut("SoftDeleted/{id}")]
        public async Task<ActionResult> SoftDeletedAsync(long id)
        {
            var checkId = await _iservice.SoftDeletedAsync(id);
            if (checkId)
            {
                return Ok(ResponeFactory.Success(HttpContext.TraceIdentifier, 204, "success"));
            }
            return BadRequest(ResponeFactory.Fail(HttpContext.TraceIdentifier, 404, "not found"));
            
        }



        [HttpPost("work-items/{id}/notes")]
        public async Task<ActionResult> PostWorkItem([FromQuery] long id, [FromBody] string note)
        {
            var newNote = _context.Add(note);
            var geId = await _iservice.PostWorkItem(id, note);
            if (geId is null)
            {
                return BadRequest(ResponeFactory.Fail(HttpContext.TraceIdentifier, 404, "kiem tra lai id hoac note "));
            }
            return Ok(ResponeFactory.Success(HttpContext.TraceIdentifier, new { Data = geId }));
        }






        [HttpGet("work-items/{id}")]
        public async Task<ActionResult> GetDetailWorkItemByIdAsync(long id)
        {
            var getId = await _iservice.GetDetailWorkItemByIdAsync(id);
            if (getId is null)
            {
                return BadRequest(ResponeFactory.Fail(HttpContext.TraceIdentifier, 404, $"not find this id{id}"));
            }
            return Ok(ResponeFactory.Success(HttpContext.TraceIdentifier, new { Data = getId }));
        }




        [HttpGet("work-items/{id}/history")]
        public async Task<ActionResult> GetHistoryDtoAsync(long id)
        {
            var getid = await _iservice.GetHistoryDtoAsync(id);
            if (getid is null)
            {
                return BadRequest(ResponeFactory.Fail(HttpContext.TraceIdentifier, 404, "not found"));
            }
            return Ok(ResponeFactory.Success(HttpContext.TraceIdentifier, new { Data = getid }));
        }




        [HttpGet("work-item/{fromDate} and {toDate}")]
        public async Task<ActionResult> GetHistoryByDateFormTo([FromQuery] DateTime? fromDate, DateTime? toDate )
        {
            var fDate = await _iservice.GetHistoryByDateFormTo(fromDate, toDate);
            if (fDate is null)
            {
                return BadRequest(ResponeFactory.Fail(HttpContext.TraceIdentifier,404, "kiem tra lai du lieu truyen vao "));
            }
            return Ok(ResponeFactory.Success(HttpContext.TraceIdentifier, new { Data = fDate}));

        }

    }
}

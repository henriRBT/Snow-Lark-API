using Microsoft.AspNetCore.Mvc;
using Swow_Lark_API.Services;

namespace Swow_Lark_API.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : Controller
    {
        private readonly Reportjob _job;

        public ReportController(Reportjob job)
        {
            _job = job;
        }

        [HttpGet("run")]
        public async Task<IActionResult> RunNow()
        {
            await _job.GenerateReport();
            return Ok("✅ Job dijalankan manual.");
        }
    }
}

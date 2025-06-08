using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Mvc;
using Swow_Lark_API.Services;

namespace Swow_Lark_API.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : Controller
    {
        private readonly ServiceNow snow;
        private readonly Reportjob job;

        public ReportController(ServiceNow snow, Reportjob job)
        {
            this.snow = snow;
            this.job = job;
        }

        [HttpGet("run")]
        public async Task<IActionResult> RunNow()
        {
            var data = await snow.GetSnow();
            await job.GenerateReport();

            foreach (var item in data)
            {
                Console.WriteLine($"Nomor     : {item.Nomor}");
                Console.WriteLine($"Deskripsi : {item.Deskripsi}");
                Console.WriteLine($"CreateOn  : {item.CreateOn}");
                Console.WriteLine($"State     : {item.State}");
                Console.WriteLine(new string('-', 40));
            }

            return Ok("✅ Job dijalankan manual.");
        
        }
    }
}

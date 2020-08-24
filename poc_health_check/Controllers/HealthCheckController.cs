using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace poc_health_check.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("live")]
        public async Task<ActionResult> GetLiveAsync() 
            => await CallHealthCheck("live");

        [HttpGet("ready")]
        public async Task<ActionResult> GetReadyAsync() 
            => await CallHealthCheck("ready");

        [HttpGet("ui-data")]
        public async Task<ActionResult> GetUIDataAsync() 
            => await CallHealthCheck("ui");


        private async Task<ActionResult> CallHealthCheck(string param)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = $"https://localhost:44316/api/health/{param}";

                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                return Ok(await response.Content.ReadAsStringAsync());
            }
        }
    }
}

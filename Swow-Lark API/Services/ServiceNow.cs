using Newtonsoft.Json.Linq;
using Swow_Lark_API.Models;
using System.Net;
using System.Text;

namespace Swow_Lark_API.Services
{
        public class ServiceNow
        {
            private readonly HttpClient _httpClient;
        
            public ServiceNow(IConfiguration config)
            {
                var website = config["ServiceNow:Website"];
                var username = config["ServiceNow:Username"];
                var password = config["ServiceNow:Password"];
                var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri($"https://{website}/portalpelindo?id=landing")
                };
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            }

            public async Task<List<SnowModel>> GetSnow()
            {
                var query = "assignment_group=ac4cd4814762c290415f7569116d43d2^active=true^stateIN2^sys_created_onONToday";
                var response = await _httpClient.GetAsync($"/api/now/table/incident?sysparm_query={query}&sysparm_limit=100");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse( content );

                return json["result"]!.Select(i => new SnowModel
                {
                    Nomor = i["number"]?.ToString(),
                    Deskripsi = i["short_description"]?.ToString(),
                    CreateOn = i["sys_created_on"]?.ToString(),
                    State = i["state"]?.ToString()
                }).ToList();
            }

        }
}

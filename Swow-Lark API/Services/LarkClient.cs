using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Swow_Lark_API.Models;
using System.Net.Http.Headers;

namespace Swow_Lark_API.Services
{
    public class LarkClient
    {
        private readonly IConfiguration _config;

        public LarkClient(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GetToken()
        {
            using var client = new HttpClient();
            var lark = new
            {
                app_id = _config["Lark:AppId"],
                app_secret = _config["Lark:AppSecret"]
            };

            var res = await client.PostAsJsonAsync("https://open.larksuite.com/open-apis/auth/v3/tenant_access_token/internal", lark);
            var json = await res.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(json)!;
            Console.WriteLine("🔑 Token Lark: " + result.tenant_access_token);
            return result.tenant_access_token;
        }

        public async Task<string> CreateSpreadsheet(string token, string title)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new { title };
            var res = await client.PostAsJsonAsync("https://open.larksuite.com/open-apis/sheets/v3/spreadsheets", payload);
            var json = await res.Content.ReadAsStringAsync();

            //pengecekan Json
            Console.WriteLine("Response from Lark CreateSpreadsheet:");
            Console.WriteLine(json);

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception($"Error from Lark API: {res.StatusCode} - {json}");
            }


            dynamic result = JsonConvert.DeserializeObject(json)!;
            return result.data.spreadsheet.spreadsheet_token;
        }



        public async Task WriteData(string token, string sheetToken, List<SnowModel> data)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var values = new List<List<string>> {
            new() { "Nomor", "Deskripsi", "Create On", "State" }
        };

            values.AddRange(data.Select(i => new List<string> {
            i.Nomor, i.Deskripsi, i.CreateOn, i.State }));

            var payload = new
            {
                valueRange = new
                {
                    range = "Sheet1!A1",
                    values = values
                }
            };

            var res = await client.PutAsJsonAsync(
                $"https://open.larksuite.com/open-apis/sheets/v2/spreadsheets/{sheetToken}/values", payload);

            res.EnsureSuccessStatusCode();
        }
    }
}

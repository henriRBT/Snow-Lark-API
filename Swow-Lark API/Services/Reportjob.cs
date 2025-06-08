namespace Swow_Lark_API.Services
{
    public class Reportjob
    {
        private readonly ServiceNow snow;
        private readonly LarkClient lark;
        public Reportjob(ServiceNow snow, LarkClient lark)
        {
            this.snow = snow;
            this.lark = lark;
        }

        public async Task GenerateReport()
        {
            Console.WriteLine($"snow is null? {snow == null}");
            Console.WriteLine($"lark is null? {lark == null}");

            var data = await snow.GetSnow();
            var token = await lark.GetToken();
            var sheet = await lark.CreateSpreadsheet(token, $"Incident {DateTime.Today:yyyy-MM-dd}");
            await lark.WriteData(token, sheet, data);

            foreach (var item in data)
            {
                Console.WriteLine($"Nomor     : {item.Nomor}");
                Console.WriteLine($"Deskripsi : {item.Deskripsi}");
                Console.WriteLine($"CreateOn  : {item.CreateOn}");
                Console.WriteLine($"State     : {item.State}");
                Console.WriteLine(new string('-', 40));
            }
        }
    }

}


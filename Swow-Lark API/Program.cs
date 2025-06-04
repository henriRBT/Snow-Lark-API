using Hangfire;
using Hangfire.MemoryStorage;
using Swow_Lark_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHangfire(c => c.UseMemoryStorage());
builder.Services.AddHangfireServer();


builder.Services.AddScoped<ServiceNow>();
builder.Services.AddScoped<LarkClient>();
builder.Services.AddScoped<Reportjob>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

RecurringJob.AddOrUpdate<Reportjob>(
    "report",
    job => job.GenerateReport(),
    Cron.Daily(14)
);

app.Run();
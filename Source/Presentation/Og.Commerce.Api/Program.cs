using Og.Commerce.Application;
using Og.Commerce.Application.Localization;
using Og.Commerce.Core;
using Og.Commerce.Core.Infrastructure;
using Og.Commerce.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddData(builder.Configuration);
builder.Services.AddScoped<LanguageService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();

//create engine and configure service provider
var engine = EngineContext.Create();

engine.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(
    options => options.AllowAnyOrigin().AllowAnyMethod()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ConfigureRequestPipeline();

app.Run();

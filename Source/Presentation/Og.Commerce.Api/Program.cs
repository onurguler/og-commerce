using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Og.Commerce.Application;
using Og.Commerce.Application.Localization;
using Og.Commerce.Core;
using Og.Commerce.Core.Infrastructure;
using Og.Commerce.Data;
using Og.Commerce.Core.AspNetCore.Middlware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddData(builder.Configuration);
builder.Services.AddScoped<LanguageAdminAppService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();
builder.Services.AddSingleton<IActionResultExecutor<ObjectResult>, ResponseEnvelopeResultExecutor>();

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

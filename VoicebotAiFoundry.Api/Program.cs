using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using VoicebotAiFoundry.Domain.Entities;
// using VoicebotAiFoundry.Application.UseCases;
using VoicebotAiFoundry.Application.Interfaces;
using VoicebotAiFoundry.Infrastructure.Services;
using VoicebotAiFoundry.Shared.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Cargar configuración desde appsettings.json
builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection("AzureOpenAI"));

// Registrar servicios generales
builder.Services.AddHttpClient<AzureOpenAIService>();

builder.Services.AddScoped<ISemanticKernelService, SemanticKernelService>();
builder.Services.AddScoped<IVoicebotService, VoicebotService>();
builder.Services.AddScoped<ITuyaHelpService, TuyaHelpService>();
builder.Services.AddScoped<ITuyaDemandasService, TuyaDemandasService>();

// Configurar dependencias
builder.Services.AddHttpClient();
builder.Services.AddScoped<IAzureOpenAIService, AzureOpenAIService>();

// Agregar soporte para Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/voicebot/messages", async ([FromBody] ChatRequest request, ISemanticKernelService service) =>
{
    var response = await service.ExecuteAgentAsync(request);
    return Results.Ok(new { response });
}).WithName("SendMessage_Voicebot");

app.MapPost("/tuyahelp", async ([FromBody] ChatRequest request, ITuyaHelpService service) =>
{
    var response = await service.ProcessHelpRequest(request);
    return Results.Ok(new { response });
}).WithName("SendMessage_TuyaHelp");

app.MapPost("/demandas", async ([FromBody] ChatRequest request, ITuyaDemandasService service) =>
{
    var response = await service.ProcessDemandRequest(request);
    return Results.Ok(new { response });
}).WithName("SendMessage_Demandas");


await app.RunAsync();

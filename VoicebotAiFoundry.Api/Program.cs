using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using VoicebotAiFoundry.Domain.Entities;
using VoicebotAiFoundry.Application.UseCases;
using VoicebotAiFoundry.Infrastructure.Services;
using VoicebotAiFoundry.Shared.Configuration;
using VoicebotAiFoundry.Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Cargar configuración desde appsettings.json
builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection("AzureOpenAI"));

// 🔹 Registrar el servicio de OpenAI
builder.Services.AddHttpClient<AzureOpenAIService>();

// Configurar dependencias
builder.Services.AddHttpClient();
builder.Services.AddScoped<IMessageService, MessageService>();
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

// Endpoint para consumir Azure OpenAI
app.MapPost("/messages", async ([FromBody] MessageRequest request, IMessageService messageService) =>
{
    var response = await messageService.GenerateResponseAsync(new Message(request.Message));
    return Results.Ok(new { response });
})
.WithName("SendMessage");


await app.RunAsync();

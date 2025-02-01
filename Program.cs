using Microsoft.AspNetCore.Mvc;
using VoicebotAiFoundry.Models;
using VoicebotAiFoundry.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar configuración desde appsettings.json
builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection("AzureOpenAI"));

// Registrar el servicio de OpenAI
builder.Services.AddSingleton<AzureOpenAIService>();

// Agregar soporte para OpenAPI
builder.Services.AddOpenApi();

// 3. Agregar servicios para documentación Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Configurar el middleware para Swagger (solo en desarrollo, pero puedes ajustarlo según necesites)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Definir el endpoint /messages para consumir Azure OpenAI
app.MapPost("/messages", async ([FromBody] MessageRequest request, AzureOpenAIService aiService) =>
{
    var response = await aiService.GenerateResponseAsync(request.Message);
    return Results.Ok(new { response });
})
.WithName("SendMessage");

await app.RunAsync();



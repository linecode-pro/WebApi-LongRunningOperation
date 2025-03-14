using MyWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



// Добавить сервис длительных операций
builder.Services.AddSingleton<ILongRunningOperationService, LongRunningOperationService>();

// Добавить использование конфигурации окружения
builder.Configuration.AddEnvironmentVariables();

// Добавить HealthCheck
builder.Services.AddHealthChecks();



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// HealthCheck Middleware
app.MapHealthChecks("/api/health");

app.Run();

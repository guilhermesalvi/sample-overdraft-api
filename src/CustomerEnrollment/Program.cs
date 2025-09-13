using CustomerEnrollment.Extensions;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddObservability();
builder.Services.AddHeaderPropagation(options => options.Headers.Add("X-Correlation-ID"));
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersioning();
builder.Services.AddOpenApi();
builder.AddData();

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateOnBuild = true;
    options.ValidateScopes = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsProduction())
    app.UseHttpsRedirection();

app.UseJsonDeserializationProblemDetails();
app.MapHealthEndpoints();
app.UseBaggageEnrichment();
app.UseHeaderPropagation();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.Run();

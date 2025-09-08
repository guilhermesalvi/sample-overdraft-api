using ClientEnrollment.Extensions;
using ServiceDefaults;

var builder = WebApplication.CreateSlimBuilder(args);

// Add services to the container.
builder.AddObservability();
builder.Services.AddHeaderPropagation(options => options.Headers.Add("X-Correlation-ID"));
builder.Services.AddProblemDetails();
builder.AddEndpoints();
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

app.MapHealthEndpoints();
app.UseBaggageEnrichment();
app.UseHeaderPropagation();
app.MapEndpoints();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.Run();

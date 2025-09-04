using ClientEnrollment.BankAccount;
using ClientEnrollment.Extensions;
using ClientEnrollment.OverdraftContract;
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
builder.AddBankAccount();
builder.AddOverdraftContract();

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

app.UseObservability();
app.UseHeaderPropagation();
app.MapEndpoints();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.Run();

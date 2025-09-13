using CustomerEnrollment.Data.Context;
using DataMigration;
using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);
builder.AddObservability();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<CustomerEnrollmentDbContext>("customer-enrollment-db");

var host = builder.Build();
host.Run();

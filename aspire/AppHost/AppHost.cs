var builder = DistributedApplication.CreateBuilder(args);

var sql = builder
    .AddSqlServer("overdraft-sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("overdraft-sql");

var initScriptPath = Path.Combine(AppContext.BaseDirectory, "init.sql");

var clientEnrollmentDb = sql
    .AddDatabase(
        name: "client-enrollment-db",
        databaseName: "ClientEnrollment")
    .WithCreationScript(File.ReadAllText(initScriptPath));

_ = builder
    .AddProject<Projects.ClientEnrollment>("client-enrollment-api")
    .WithHttpHealthCheck("/health/ready")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithReference(clientEnrollmentDb)
    .WaitFor(clientEnrollmentDb);

builder.Build().Run();

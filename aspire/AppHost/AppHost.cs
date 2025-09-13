var builder = DistributedApplication.CreateBuilder(args);

var sql = builder
    .AddSqlServer("overdraft-sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("overdraft-sql");

var customerEnrollmentDb = sql.AddDatabase(
    name: "customer-enrollment-db",
    databaseName: "CustomerEnrollment");

var migrations = builder.AddProject<Projects.DataMigration>("migrations")
    .WithReference(customerEnrollmentDb)
    .WaitFor(customerEnrollmentDb);

_ = builder
    .AddProject<Projects.CustomerEnrollment>("customer-enrollment-api")
    .WithHttpHealthCheck("/health/ready")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithReference(customerEnrollmentDb)
    .WaitFor(customerEnrollmentDb)
    .WaitForCompletion(migrations);

builder.Build().Run();

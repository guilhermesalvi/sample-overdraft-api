using System.Text.Json.Serialization;
using CustomerEnrollment.Features.BankAccounts.Domain;
using CustomerEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;
using CustomerEnrollment.Features.BankAccounts.Repositories;
using FluentValidation;

namespace CustomerEnrollment.Features.BankAccounts;

public static class BankAccountExtensions
{
    public static void AddBankAccounts(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Add(BankAccountsJsonSerializerContext.Default));

        builder.Services.AddScoped<BankAccountRepository>();
        builder.Services.AddScoped<IValidator<CreateBankAccountRequest>, CreateBankAccountRequestValidator>();
    }
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true
)]
[JsonSerializable(typeof(BankAccount))]
[JsonSerializable(typeof(CustomerType))]
[JsonSerializable(typeof(CreateBankAccountRequest))]
internal partial class BankAccountsJsonSerializerContext : JsonSerializerContext;

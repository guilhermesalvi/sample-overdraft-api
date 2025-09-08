using System.Text.Json.Serialization;
using ClientEnrollment.Features.BankAccounts.Domain;
using ClientEnrollment.Features.BankAccounts.Endpoints.CreateBankAccount;
using ClientEnrollment.Features.BankAccounts.Repositories;
using FluentValidation;

namespace ClientEnrollment.Features.BankAccounts;

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
[JsonSerializable(typeof(ClientType))]
[JsonSerializable(typeof(CreateBankAccountRequest))]
internal partial class BankAccountsJsonSerializerContext : JsonSerializerContext;

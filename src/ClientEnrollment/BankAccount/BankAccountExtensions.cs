using System.Text.Json.Serialization;
using ClientEnrollment.BankAccount.Endpoints.CreateBankAccount;
using FluentValidation;

namespace ClientEnrollment.BankAccount;

public static class BankAccountExtensions
{
    public static void AddBankAccount(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Add(BankAccountJsonSerializerContext.Default));

        builder.Services.AddScoped<BankAccountRepository>();

        builder.Services.AddScoped<IValidator<CreateBankAccountRequest>, CreateBankAccountRequestValidator>();
    }
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true
)]
[JsonSerializable(typeof(Account))]
[JsonSerializable(typeof(CustomerType))]
[JsonSerializable(typeof(CreateBankAccountRequest))]
internal partial class BankAccountJsonSerializerContext : JsonSerializerContext;

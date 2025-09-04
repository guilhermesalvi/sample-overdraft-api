using System.Text.Json.Serialization;

namespace ClientEnrollment.BankAccount;

public static class BankAccountExtensions
{
    public static void AddBankAccount(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Add(BankAccountJsonSerializerContext.Default));
    }
}

[JsonSerializable(typeof(Account))]
[JsonSerializable(typeof(CustomerType))]
internal partial class BankAccountJsonSerializerContext : JsonSerializerContext;

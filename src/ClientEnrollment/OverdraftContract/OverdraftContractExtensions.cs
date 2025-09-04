using System.Text.Json.Serialization;

namespace ClientEnrollment.OverdraftContract;

public static class OverdraftContractExtensions
{
    public static void AddOverdraftContract(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Add(OverdraftContractJsonSerializerContext.Default));
    }
}

[JsonSerializable(typeof(Contract))]
internal partial class OverdraftContractJsonSerializerContext : JsonSerializerContext;

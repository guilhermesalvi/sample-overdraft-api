using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Settings;

public record DataSettings
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Data connection string is required")]
    public string ConnectionString { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Cache schema name is required")]
    public string CacheSchemaName { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Cache table name is required")]
    public string CacheTableName { get; init; } = string.Empty;
}

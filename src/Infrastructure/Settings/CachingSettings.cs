using System.ComponentModel.DataAnnotations;

namespace Overdraft.Infrastructure.Settings;

public class CachingSettings
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Caching connection string is required")]
    public string ConnectionString { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Caching schema name is required")]
    public string SchemaName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Caching table name is required")]
    public string TableName { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;

namespace Overdraft.Infrastructure.Data.Settings;

public record DataSettings
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "ConnectionString is required")]
    public string ConnectionString { get; init; } = string.Empty;
}

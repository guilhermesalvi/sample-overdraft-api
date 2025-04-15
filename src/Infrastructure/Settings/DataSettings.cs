using System.ComponentModel.DataAnnotations;

namespace Overdraft.Infrastructure.Settings;

public record DataSettings
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Data connection string is required")]
    public string ConnectionString { get; set; } = string.Empty;
}

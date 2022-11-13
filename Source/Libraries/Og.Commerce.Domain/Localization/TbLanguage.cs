using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Og.Commerce.Domain.Localization;

[Index(nameof(UniqueSeoCode), IsUnique = true)]
public class TbLanguage
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(100)]
    public string NativeName { get; set; } = default!;

    [MaxLength(5)]
    public string CultureName { get; set; } = default!;

    [MaxLength(5)]
    public string UniqueSeoCode { get; set; } = default!;

    [MaxLength(15)]
    public string? FlagImageFileName { get; set; }

    public bool Rtl { get; set; }

    public bool Published { get; set; }

    public int DisplayOrder { get; set; }
}

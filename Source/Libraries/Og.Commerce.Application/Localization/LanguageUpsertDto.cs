using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Og.Commerce.Application.Localization;

public class LanguageUpsertDto
{
    [Key]
    [DisplayName("Kayıt No")]
    public Guid Id { get; set; }

    [MaxLength(100)]
    [DisplayName("İsim")]
    public string Name { get; set; } = default!;

    [MaxLength(5)]
    [DisplayName("Kültür")]
    public string CultureName { get; set; } = default!;

    [MaxLength(5)]
    [DisplayName("Benzersiz SEO Kodu")]
    public string Slug { get; set; } = default!;

    [MaxLength(15)]
    [DisplayName("Bayrak Dosya İsmi")]
    public string? FlagImageFileName { get; set; }

    [DisplayName("Sağdan Sola")]
    public bool Rtl { get; set; }

    [DisplayName("Yayında")]
    public bool Published { get; set; }

    [DisplayName("Görüntü Sırası")]
    public int DisplayOrder { get; set; }
}

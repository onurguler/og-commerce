namespace Og.Commerce.Application.Localization;

public class LanguageCultureDto
{
    public string Name { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string NativeName { get; set; } = null!;
    public string ThreeLetterISOLanguageName { get; set; } = null!;
    public string TwoLetterISOLanguageName { get; set; } = null!;
    public string IetfLanguageTag { get; set; } = null!; 
}

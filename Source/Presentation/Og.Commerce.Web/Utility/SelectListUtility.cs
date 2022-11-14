using Microsoft.AspNetCore.Mvc.Rendering;
using Og.Commerce.Application.Localization;

namespace Og.Commerce.Web.Utility;

public class SelectListUtility
{
    private readonly LanguageService _languageService;

    public SelectListUtility(LanguageService languageService)
    {
        _languageService = languageService;
    }

    public List<SelectListItem> GetCultures() 
        => _languageService.GetCultures()
        .Select(s => new SelectListItem($"{s.EnglishName}. {s.IetfLanguageTag}", s.IetfLanguageTag))
        .ToList();
}

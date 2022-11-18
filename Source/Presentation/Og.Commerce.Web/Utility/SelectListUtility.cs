using Microsoft.AspNetCore.Mvc.Rendering;
using Og.Commerce.Application.Localization;

namespace Og.Commerce.Web.Utility;

public class SelectListUtility
{
    private readonly LanguageAdminAppService _languageService;

    public SelectListUtility(LanguageAdminAppService languageService)
    {
        _languageService = languageService;
    }

    public List<SelectListItem> GetCultures() 
        => _languageService.GetCultures()
        .Select(s => new SelectListItem($"{s.EnglishName}. {s.IetfLanguageTag}", s.IetfLanguageTag))
        .ToList();
}

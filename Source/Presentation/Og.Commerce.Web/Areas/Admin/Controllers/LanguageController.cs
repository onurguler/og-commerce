using Microsoft.AspNetCore.Mvc;

namespace Og.Commerce.Web.Areas.Admin.Controllers;

public class LanguageController : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;
using Og.Commerce.Application.Localization;

namespace Og.Commerce.Api.Controllers.Localization
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly LanguageService _languageService;

        public LanguagesController(LanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet("cultures")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public ActionResult<List<CultureDto>> GetCultures() => _languageService.GetCultures();
    }

    //[Route("api/[controller]")]
    //[ApiController]
    //public class LanguagesController : ControllerBase
    //{
    //    public LanguagesController()
    //    {
    //    }

    //    // GET: api/Languages
    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<TbLanguage>>> GetTbLanguages()
    //    {
    //        return null;
    //    }

    //    // GET: api/Languages/5
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<TbLanguage>> GetTbLanguage(Guid id)
    //    {
    //        return null;
    //    }

    //    // PUT: api/Languages/5
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> PutTbLanguage(Guid id, TbLanguage tbLanguage)
    //    {
    //        return null;
    //    }

    //    // POST: api/Languages
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPost]
    //    public async Task<ActionResult<TbLanguage>> PostTbLanguage(TbLanguage tbLanguage)
    //    {
    //        return null;
    //    }

    //    // DELETE: api/Languages/5
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteTbLanguage(Guid id)
    //    {
    //        return null;
    //    }
    //}
}

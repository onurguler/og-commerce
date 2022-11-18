using Microsoft.AspNetCore.Mvc;
using Og.Commerce.Application.Localization;
using Og.Commerce.Core.AspNetCore;
using Og.Commerce.Core.Domain;
using Og.Commerce.Core.Result;
using Og.Commerce.Domain.Localization;

namespace Og.Commerce.Api.Controllers.Localization
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly LanguageAdminAppService _languageService;

        public LanguagesController(LanguageAdminAppService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<Language>>> GetById([FromRoute] Guid id)
        {
            var result = await _languageService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(Result.Success(result, "Kayıt olusturuldu"));
        }

        [HttpGet]
        public async Task<ApiResponse<IPagedList<Language>>> GetList([FromQuery] int page = 1, [FromQuery] int limit = 10)
            => Ok(await _languageService.GetPagedListAsync(page, limit));

        [HttpPost]
        public async Task<ActionResult<Language>> Create([FromBody] LanguageUpsertDto input)
        {
            var result = await _languageService.UpsertAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<ActionResult<Language>> Update([FromBody] LanguageUpsertDto input)
        {
            var result = await _languageService.UpsertAsync(input);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Language>> Delete([FromRoute] Guid id)
        {
            await _languageService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("cultures")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public ActionResult<List<LanguageCultureDto>> GetCultures() => _languageService.GetCultures();
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

using Og.Commerce.Domain.Localization;
using Og.Commerce.Core.Application;
using Og.Commerce.Data.DbContexts;
using Og.Commerce.Core.Data.Repositories;
using Og.Commerce.Core.Domain;

namespace Og.Commerce.Application.Localization;

public class LanguageService : ApplicationService<ApplicationDbContext>
{
    private readonly IRepository<TbLanguage> _languageRepository;

    public LanguageService()
    {
        _languageRepository = UnitOfWork.GetRepository<TbLanguage>();
    }

    public async Task<TbLanguage?> GetByIdAsync(Guid id) => await _languageRepository.GetAsync(id);

    public async Task<IPagedList<TbLanguage>> GetPagedListAsync(int page = 1, int limit = 10, bool publishedOnly = true)
        => await _languageRepository.GetListPagedAsync(query =>
            {
                if (publishedOnly)
                    query = query.Where(w => w.Published);

                query = query.OrderBy(o => o.DisplayOrder).AsQueryable();
                return Task.FromResult(query);
            }, page - 1, limit);

    public async Task<TbLanguage> InsertAsync(TbLanguage input) => await _languageRepository.InsertAsync(input, true);

    public async Task<TbLanguage> UpdateAsync(TbLanguage input) => await _languageRepository.UpdateAsync(input, true);

    public async Task DeleteAsync(TbLanguage input) => await _languageRepository.DeleteAsync(input, true);

    public async Task<bool> DeleteAsync(Guid id) => await _languageRepository.DeleteByIdAsync(id);

    #region [ Utilities ]

    public List<CultureDto> GetCultures()
            => System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures)
                        .OrderBy(x => x.EnglishName)
                        .Select(x => new CultureDto
                        {
                            DisplayName = x.DisplayName,
                            TwoLetterISOLanguageName = x.TwoLetterISOLanguageName,
                            Name = x.Name,
                            EnglishName = x.EnglishName,
                            IetfLanguageTag = x.IetfLanguageTag,
                            NativeName = x.NativeName,
                            ThreeLetterISOLanguageName = x.ThreeLetterISOLanguageName
                        }).ToList();

    #endregion
}

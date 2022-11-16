using Og.Commerce.Domain.Localization;
using Og.Commerce.Core.Application;
using Og.Commerce.Data.DbContexts;
using Og.Commerce.Core.Data.Repositories;
using Og.Commerce.Core.Domain;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

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

    public async Task<TbLanguage> UpsertAsync(LanguageUpsertDto input)
    {
        bool newRecord = false;
        var language = await _languageRepository.GetAsync(input.Id);
        if (language is null)
        {
            newRecord = true;
            language = new TbLanguage();
        }

        CheckCultureIsValid(input.CultureName, true);
        await CheckUniqueSeoCodeAvailableAsync(input.UniqueSeoCode, input.Id, throwException: true);

        language.CultureName = input.CultureName;
        language.DisplayOrder = input.DisplayOrder;
        language.FlagImageFileName = input.FlagImageFileName;
        language.Name = input.Name;
        language.Published = input.Published;
        language.Rtl = input.Rtl;
        language.UniqueSeoCode = input.UniqueSeoCode;

        _ = newRecord 
            ? await _languageRepository.InsertAsync(language, true) 
            : await _languageRepository.UpdateAsync(language, true);

        return language;
    }

    public async Task<bool> DeleteAsync(Guid id) => await _languageRepository.DeleteByIdAsync(id);

    #region [ Utilities ]

    public List<LanguageCultureDto> GetCultures()
            => System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures)
                        .OrderBy(x => x.EnglishName)
                        .Select(x => new LanguageCultureDto
                        {
                            DisplayName = x.DisplayName,
                            TwoLetterISOLanguageName = x.TwoLetterISOLanguageName,
                            Name = x.Name,
                            EnglishName = x.EnglishName,
                            IetfLanguageTag = x.IetfLanguageTag,
                            NativeName = x.NativeName,
                            ThreeLetterISOLanguageName = x.ThreeLetterISOLanguageName
                        }).ToList();

    public bool CheckCultureIsValid(string cultureName, bool throwException = false)
    {
        try
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            if (culture == null)
                throw new Exception($"The culture '{cultureName}' is not valid.");
            return true;
        }
        catch (Exception)
        {
            if (throwException)
                throw new Exception($"The culture '{cultureName}' is not valid.");
            return false;
        }
    }

    public async Task<bool> CheckUniqueSeoCodeAvailableAsync(string uniqueSeoCode, Guid? languageIdToIgnore = null, bool throwException = false)
    {
        var query = _languageRepository.Table.Where(w => w.UniqueSeoCode == uniqueSeoCode);
        if (languageIdToIgnore.HasValue)
            query = query.Where(w => w.Id != languageIdToIgnore.GetValueOrDefault());
        bool exists = await query.AnyAsync();

        if (exists && throwException)
            throw new Exception($"A language exists with this unique seo code '{uniqueSeoCode}'");

        return !exists;
    }

    #endregion
}

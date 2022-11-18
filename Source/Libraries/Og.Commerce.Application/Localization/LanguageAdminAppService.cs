using Og.Commerce.Core.Application;
using Og.Commerce.Data.DbContexts;
using Og.Commerce.Core.Data.Repositories;
using Og.Commerce.Core.Domain;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Og.Commerce.Domain.Localization;

namespace Og.Commerce.Application.Localization;

public class LanguageAdminAppService : ApplicationService<ApplicationDbContext>
{
    private readonly IRepository<Language> _languageRepository;

    public LanguageAdminAppService()
    {
        _languageRepository = UnitOfWork.GetRepository<Language>();
    }

    public async Task<LanguageDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var language = await _languageRepository.GetAsync(id, cancellationToken);
        if (language is null)
            throw new Exception($"Language with id {id} not found.");
        return ObjectMapper.Map<LanguageDto>(language);
    }

    public async Task<IPagedList<Language>> GetPagedListAsync(int page = 1, int limit = 10, bool publishedOnly = true, CancellationToken cancellationToken = default)
        => await _languageRepository.GetListPagedAsync(query =>
            {
                if (publishedOnly)
                    query = query.Where(w => w.Published);

                query = query.OrderBy(o => o.DisplayOrder).AsQueryable();
                return Task.FromResult(query);
            }, page - 1, limit, cancellationToken: cancellationToken);

    public async Task<LanguageDto> UpsertAsync(LanguageUpsertDto input)
    {
        bool newRecord = false;
        var language = await _languageRepository.GetAsync(input.Id);
        if (language is null)
        {
            newRecord = true;
            language = new Language();
        }

        CheckCultureIsValid(input.CultureName, true);
        await CheckSlugAvailableAsync(input.Slug, input.Id, throwException: true);

        ObjectMapper.Map(input, language);

        _ = newRecord
            ? await _languageRepository.AddAsync(language)
            : _languageRepository.Update(language);

        await UnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<LanguageDto>(language);
    }

    public async Task DeleteAsync(Guid id)
    {
        var language = await _languageRepository.GetAsync(id);

        if (language is null)
            throw new Exception($"Language with id {id} not found.");

        _languageRepository.Remove(language);

        await UnitOfWork.SaveChangesAsync();
    }

    #region [ Utilities ]

    public List<LanguageCultureDto> GetCultures()
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
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
            })
            .ToList();
        return cultures;
    }

    public bool CheckCultureIsValid(string cultureName, bool throwException = false)
    {
        var culture = CultureInfo.GetCultureInfo(cultureName);
        if (culture is null && throwException)
            throw new Exception($"The culture '{cultureName}' is not valid.");
        return culture != null;
    }

    public async Task<bool> CheckSlugAvailableAsync(string uniqueSeoCode, Guid? languageIdToIgnore = null, bool throwException = false)
    {
        var query = _languageRepository.Table.Where(w => w.Slug == uniqueSeoCode);
        if (languageIdToIgnore.HasValue)
            query = query.Where(w => w.Id != languageIdToIgnore.GetValueOrDefault());
        bool exists = await query.AnyAsync();

        if (exists && throwException)
            throw new Exception($"A language exists with this unique seo code '{uniqueSeoCode}'");

        return !exists;
    }

    #endregion
}

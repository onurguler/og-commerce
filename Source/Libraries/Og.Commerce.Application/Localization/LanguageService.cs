using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Og.Commerce.Domain.Localization;
using Og.Commerce.Infrastructure.Persistence.DataContexts;

namespace Og.Commerce.Application.Localization;

public class LanguageService
{
    private readonly ApplicationDbContext _context;

    public LanguageService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TbLanguage?> GetByIdAsync(Guid id)
    {
        var entity = await _context.TbLanguages.FindAsync(id);
        return entity;
    }

    public async Task<List<TbLanguage>> GetPagedListAsync(int page = 1, int limit = 10, bool publishedOnly = true)
    {
        var query = _context.TbLanguages.AsNoTracking();
        if (publishedOnly)
            query = query.Where(w => w.Published);

        var list = await query.OrderBy(o => o.DisplayOrder).Skip((page - 1) * limit).Take(limit).ToListAsync();

        return list;
    }

    public async Task<TbLanguage> InsertAsync(TbLanguage input)
    {
        var createdEntity = await _context.TbLanguages.AddAsync(input);
        await _context.SaveChangesAsync();
        return createdEntity.Entity;
    }

    public async Task<TbLanguage> UpdateAsync(TbLanguage input)
    {
        _context.TbLanguages.Update(input);
        await _context.SaveChangesAsync();
        return input;
    }

    public async Task<TbLanguage> DeleteAsync(TbLanguage input)
    {
        _context.TbLanguages.Remove(input);
        await _context.SaveChangesAsync();
        return input;
    }

    public async Task<TbLanguage> DeleteAsync(Guid id)
    {
        var entity = await _context.TbLanguages.FindAsync(id);
        if (entity is null)
            throw new Exception($"Language with id '{id}' not found.");
        _context.TbLanguages.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

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

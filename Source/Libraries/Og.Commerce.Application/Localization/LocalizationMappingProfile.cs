using AutoMapper;
using Og.Commerce.Domain.Localization;

namespace Og.Commerce.Application.Localization;

internal class LocalizationMappingProfile : Profile
{
    public LocalizationMappingProfile()
    {
        CreateMap<TbLanguage, LanguageDto>().ReverseMap();
        CreateMap<TbLanguage, LanguageUpsertDto>().ReverseMap();
    }
}

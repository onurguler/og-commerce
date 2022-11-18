using AutoMapper;
using Og.Commerce.Domain.Localization;

namespace Og.Commerce.Application.Localization;

internal class LocalizationMappingProfile : Profile
{
    public LocalizationMappingProfile()
    {
        CreateMap<Language, LanguageDto>().ReverseMap();
        CreateMap<Language, LanguageUpsertDto>().ReverseMap();
    }
}

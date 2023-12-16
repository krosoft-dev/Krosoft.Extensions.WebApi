using AutoMapper;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Samples.Library.Mappings;

public class LogicielProfile : Profile
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="LogicielProfile" />.
    /// </summary>
    public LogicielProfile()
    {
        CreateMap<Logiciel, LogicielDto>()
            .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nom, o => o.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Categorie, o => o.MapFrom(src => src.Categorie))
            .ForMember(dest => dest.StatutCode, o => o.MapFrom(src => src.StatutCode))
            .ForMember(dest => dest.DateCreation, o => o.MapFrom(src => src.DateCreation))
            .ForAllOtherMembers(m => m.Ignore());

        CreateMap<Logiciel, LogicielCsvDto>()
            .ForMember(dest => dest.Nom, o => o.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Categorie, o => o.MapFrom(src => src.Categorie))
            .ForAllOtherMembers(m => m.Ignore());
    }
}
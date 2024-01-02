using AutoMapper;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Samples.Library.Mappings;

public class PaysProfile : Profile
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="PaysProfile" />.
    /// </summary>
    public PaysProfile()
    {
        CreateMap<Pays, PaysDto>()
            .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, o => o.MapFrom(src => src.Code))
            .ForAllOtherMembers(m => m.Ignore());
    }
}
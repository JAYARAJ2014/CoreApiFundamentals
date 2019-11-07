using AutoMapper;
using CoreCodeCamp.DTOs;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {

        public CampProfile()
        {
            this.CreateMap<Talk, TalkDto>();
            this.CreateMap<Speaker, SpeakerDto>();

            this.CreateMap<Camp, CampDto>()
            .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName))
            .ForMember(c => c.Address1, o => o.MapFrom(m => m.Location.Address1))
            .ForMember(c => c.Address2, o => o.MapFrom(m => m.Location.Address2))
            .ForMember(c => c.Address3, o => o.MapFrom(m => m.Location.Address3))
            .ForMember(c => c.CityTown, o => o.MapFrom(m => m.Location.CityTown))
            .ForMember(c => c.StateProvince, o => o.MapFrom(m => m.Location.StateProvince))
            .ForMember(c => c.PostalCode, o => o.MapFrom(m => m.Location.PostalCode))
            .ForMember(c => c.Country, o => o.MapFrom(m => m.Location.Country))
                        ;

            this.CreateMap<CampDto, Camp>();
            
            //  .ForMember(c => c.Location.VenueName, o => o.MapFrom(m => m.Venue))
            // .ForMember(c => c.Location.Address1, o => o.MapFrom(m => m.Address1))
            // .ForMember(c => c.Location.Address2, o => o.MapFrom(m => m.Address2))
            // .ForMember(c => c.Location.Address3, o => o.MapFrom(m => m.Address3))
            // .ForMember(c => c.Location.CityTown, o => o.MapFrom(m => m.CityTown))
            // .ForMember(c => c.Location.StateProvince, o => o.MapFrom(m => m.StateProvince))
            // .ForMember(c => c.Location.PostalCode, o => o.MapFrom(m => m.PostalCode))
            // .ForMember(c => c.Location.Country, o => o.MapFrom(m => m.Country))
            // ;

        }
    }

}
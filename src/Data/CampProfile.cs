using AutoMapper;
using CoreCodeCamp.DTOs;

namespace CoreCodeCamp.Data
{
    public class CampProfile: Profile
    {

        public CampProfile()
        {
            this.CreateMap<Camp,CampDto>();
        }
    }

}
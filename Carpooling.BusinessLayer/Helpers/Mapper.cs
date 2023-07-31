using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.Requests;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Carpooling.BusinessLayer.Helpers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserRequest, User>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();
            CreateMap<Feedback, FeedbackDTO>().ReverseMap();

            CreateMap<Address, AddressDTO>()
                .ForMember(a => a.Country, opt => opt.MapFrom(c => c.Country.Name))
                .ReverseMap();
            
            CreateMap<Car, CarDTO>().ReverseMap();

            CreateMap<Country, CountryDTO>()
                .ForMember(c=>c.Country, c=>c.MapFrom(opt=>opt.Name))
                .ReverseMap();
        }
    }
}

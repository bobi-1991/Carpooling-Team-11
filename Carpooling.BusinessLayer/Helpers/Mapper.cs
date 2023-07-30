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
            CreateMap<TravelRequest, Travel>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();
            CreateMap<Feedback, FeedbackResponse>().ReverseMap();
            CreateMap<Feedback, FeedbackRequest>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap()
                .ForMember(a => a.Country.Name, opt => opt.MapFrom(c => c.Country));
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Car, CarDTO>().ReverseMap();
        }
    }
}

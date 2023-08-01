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
                .ForPath(a => a.Country, opt => opt.MapFrom(c => c.Country.Name));

            CreateMap<AddressDTO, Address>()
                .ForPath(a => a.Country.Name, opt => opt.MapFrom(c => c.Country));

                     
            
            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<Travel, TravelResponse>()
                .ForMember(dest => dest.StartLocationName, opt => opt.MapFrom(src => src.StartLocation.City))
                .ForMember(dest => dest.DestinationName, opt => opt.MapFrom(src => src.EndLocation.City))
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.Car.AvailableSeats))
                //.ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.IsCompleted.HasValue ? src.IsCompleted.Value : false))
                .ForMember(dest => dest.CarRegistration, opt => opt.MapFrom(src => src.Car != null ? src.Car.Registration : string.Empty))
                .ReverseMap();

            CreateMap<Country, CountryDTO>()
                .ForMember(c=>c.Country, c=>c.MapFrom(opt=>opt.Name))
                .ReverseMap();

        }
    }
}

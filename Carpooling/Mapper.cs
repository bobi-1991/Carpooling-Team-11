using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.Requests;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.Models;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using System.Collections.Generic;
using System.Configuration;

namespace Carpooling
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
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
                .ForMember(dest=> dest.DistanceBetweenDestinations, opt=>opt.MapFrom(src=>src.TravelDistance))
                //.ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.IsCompleted.HasValue ? src.IsCompleted.Value : false))
                .ForMember(dest => dest.CarRegistration, opt => opt.MapFrom(src => src.Car != null ? src.Car.Registration : string.Empty))
                .ReverseMap();

            CreateMap<Country, CountryDTO>()
                .ForMember(c => c.Country, c => c.MapFrom(opt => opt.Name))
                .ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<UserResponse, UserViewModel>().ReverseMap();
            CreateMap<Travel, TravelViewModel>()
                .ForPath(c => c.CarRegistration, opt => opt.MapFrom(src => src.Car.Registration))
                .ForPath(c => c.CityEndDest, opt => opt.MapFrom(src => src.EndLocation.City))
                .ForPath(c => c.CityStartDest, opt => opt.MapFrom(src => src.StartLocation.City))
                .ForPath(c => c.Country, opt => opt.MapFrom(src => src.StartLocation.Country.Name))
                .ForPath(c => c.Country, opt => opt.MapFrom(src => src.EndLocation.Country.Name))
                .ReverseMap();
            CreateMap<Car, CarViewModel>().ReverseMap();
            CreateMap<TripRequest,TripRequestViewModel>().ReverseMap();
        }

    }
}

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

            //CreateMap<Travel, TravelUpdateDto>()
            //   .ForMember(dest => dest.StartLocationId, opt => opt.MapFrom(src => src.StartLocation))
            //   .ForMember(dest => dest.DestionationId, opt => opt.MapFrom(src => src.EndLocation))
            //   .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
            //   .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
            //   .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
            //   .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.Car))
            //   .ReverseMap();

            CreateMap<Travel, TravelUpdateDto>()
           .ForMember(dest => dest.StartLocationId, opt => opt.MapFrom(src => src.StartLocation.Id))
           .ForMember(dest => dest.DestionationId, opt => opt.MapFrom(src => src.EndLocation.Id))
           .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
           .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
           .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
           .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.Car.Id))
           .ReverseMap();

          //  CreateMap<TravelUpdateDto, Travel>()
          //.ForPath(dest => dest.StartLocation.Id, opt => opt.MapFrom(src => src.StartLocationId))
          //.ForPath(dest => dest.EndLocation.Id, opt => opt.MapFrom(src => src.DestionationId))
          //.ForPath(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
          //.ForPath(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
          //.ForPath(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
          //.ForPath(dest => dest.Car.Id, opt => opt.MapFrom(src => src.CarId));
        }
    }
}

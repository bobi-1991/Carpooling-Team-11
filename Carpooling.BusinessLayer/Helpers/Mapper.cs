using AutoMapper;
using CarPooling.Data.Models;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;

namespace Carpooling.BusinessLayer.Helpers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserRequest, User>().ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();

            CreateMap<TripRequestRequest, TripRequest>().ReverseMap();
            CreateMap<TripRequestResponse, TripRequest>().ReverseMap();

            CreateMap<TravelRequest, Travel>().ReverseMap();
            CreateMap<TravelResponse, Travel>().ReverseMap();

            CreateMap<FeedbackRequest, Feedback>().ReverseMap();
            CreateMap<FeedbackResponse, Feedback>().ReverseMap();
        }
    }
}

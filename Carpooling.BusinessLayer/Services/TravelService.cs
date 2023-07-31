using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class TravelService : ITravelService
    {
        private readonly ITravelRepository travelRepository;
        private readonly IMapper mapper;
        private readonly IAddressRepository addressRepository;
        private readonly ICarRepository carRepository;
        private readonly ITravelValidator travelValidator;

        public TravelService(ITravelRepository travelRepository, IMapper mapper, IAddressRepository addressRepository, ICarRepository carRepository, ITravelValidator travelValidator)
        {
            this.travelRepository = travelRepository;
            this.mapper = mapper;
            this.addressRepository = addressRepository;
            this.carRepository = carRepository;
            this.travelValidator = travelValidator;
        }

        public async Task<IEnumerable<TravelResponse>> GetAllAsync()
        {
            var travels = await this.travelRepository.GetAllAsync();

            return travels.Select(x => mapper.Map<TravelResponse>(x));
        }
        public async Task<TravelResponse> GetByIdAsync(int travelId)
        {
            return  this.mapper.Map<TravelResponse>(await this.travelRepository.GetByIdAsync(travelId));
        }

        public async Task<TravelResponse> CreateTravelAsync(User loggedUser, TravelRequest travelRequest)
        {
            await this.travelValidator.ValidateIsLoggedUserAreDriver(loggedUser,travelRequest.DriverId);

            var startLocation = await this.addressRepository.GetByIdAsync(travelRequest.StartLocationId);
            var destination = await this.addressRepository.GetByIdAsync(travelRequest.DestionationId);
            var car = await this.carRepository.GetByIdAsync(travelRequest.CarId);

            var travel = new Travel
            {
                DriverId = travelRequest.DriverId,
                DepartureTime = travelRequest.DepartureTime,
                ArrivalTime = travelRequest.ArrivalTime,
                StartLocation = startLocation,
                IsCompleted = false,
                EndLocation = destination,
                Car = car
            };

            if (!await this.travelValidator.ValidateIsNewTravelPossible(travel.DriverId, travel.DepartureTime, travel.ArrivalTime))
            {
                throw new UnauthorizedOperationException("This trip cannot be made because the driver has another trip at the time.");
            }


            var createdTravel = await this.travelRepository.CreateTravelAsync(travel);

            return new TravelResponse
            {
                StartLocationName = createdTravel.StartLocation.Details,
                DestinationName = createdTravel.EndLocation.Details,
                DepartureTime = createdTravel.DepartureTime,
                ArrivalTime = createdTravel.ArrivalTime,
                AvaibleSeats = createdTravel.AvailableSlots,
                IsComplete = false,
                CarRegistration = createdTravel.Car.Registration
            };


        //return this.mapper.Map<TravelResponse>(await this.travelRepository.CreateTravelAsync(this.mapper.Map<Travel>(travelRequest)));
    }

        public async Task<TravelResponse> DeleteAsync(int travelId)
        {
            throw new NotImplementedException();
        }


        public async Task<TravelResponse> UpdateAsync(int travelId, Travel travel)
        {
            throw new NotImplementedException();
        }
        public async Task<TravelResponse> AddUserToTravelAsync(string driveId, int travelId, string passengerId)
        {
            throw new NotImplementedException();
        }
    }
}

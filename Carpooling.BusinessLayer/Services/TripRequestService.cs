using AutoMapper;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class TripRequestService : ITripRequestService
    {
        private readonly ITravelRepository travelRepository;
        // private readonly IMapper mapper;
        //  private readonly IAddressRepository addressRepository;
        //  private readonly ICarRepository carRepository;
        //  private readonly ITravelValidator travelValidator;
        private readonly IUserValidation userValidation;
        private readonly ITripRequestRepository tripRequestRepository;
        private readonly IUserRepository userRepository;
        private readonly ITripRequestValidator tripRequestValidator;

        public TripRequestService(ITripRequestRepository tripRequestRepository, IUserValidation userValidation, ITravelRepository travelRepository, IUserRepository userRepository, ITripRequestValidator tripRequestValidator)
        {
            this.tripRequestRepository = tripRequestRepository;
            this.userValidation = userValidation;
            this.travelRepository = travelRepository;
            this.userRepository = userRepository;
            this.tripRequestValidator = tripRequestValidator;
        }

        public Task<IEnumerable<TripRequestResponse>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TripRequestResponse>> GetAllDriverRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TripRequestResponse>> GetAllPassengerRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TripRequestResponse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }



        public async Task<TripRequestResponse> CreateAsync(User loggedUser, TripRequestRequest tripRequest)
        {
            var passenger = await this.userRepository.GetByIdAsync(tripRequest.PassengerId);

            var travel = await this.travelRepository.GetByIdAsync(tripRequest.TravelId);
            var driverId = travel.DriverId;

            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, passenger.Id);

            if (await this.tripRequestValidator.ValidateIfPassengerAlreadyCreateTripRequest(tripRequest))
            {
                throw new DublicateEntityException("You already has request for this travel.");
            }

            var trip = await this.tripRequestRepository.CreateAsync(driverId, passenger.Id, travel.Id);

            return new TripRequestResponse
            {
                PassengerUsername = passenger.UserName,
                StartLocationDetails = travel.StartLocation.Details,
                EndLocationDetails = travel.EndLocation.Details,
                DepartureTime = (DateTime)travel.DepartureTime,
                Status = trip.Status.ToString()
            };
        }


        public Task<TripRequestResponse> UpdateTripRequestAsync(User loggedUser, int tripRequestId, bool answer)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteAsync(User loggedUser, int tripRequestId)
        {
            throw new NotImplementedException();
        }
    }
}

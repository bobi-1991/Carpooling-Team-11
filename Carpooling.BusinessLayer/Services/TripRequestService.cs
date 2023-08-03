using AutoMapper;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;

namespace Carpooling.BusinessLayer.Services
{
    public class TripRequestService : ITripRequestService
    {
        private readonly ITravelRepository travelRepository;
         private readonly IMapper mapper;
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

        public async Task<IEnumerable<TripRequestResponse>> GetAllAsync()
        {
            var tripRequests = await this.tripRequestRepository.GetAllAsync();

            return tripRequests.Select(x => new TripRequestResponse(
                x.Passenger.UserName,
                x.Driver.UserName,
                x.Travel.StartLocation.Details,
                x.Travel.EndLocation.Details,(DateTime)
                x.Travel.DepartureTime,
                x.Status.ToString()));
        }

        public Task<IEnumerable<TripRequestResponse>> GetAllDriverRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TripRequestResponse>> GetAllPassengerRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TripRequestResponse> GetByIdAsync(int id)
        {
            var tripRequest = await this.tripRequestRepository.GetByIdAsync(id);

            return new TripRequestResponse(
                tripRequest.Passenger.UserName,
                tripRequest.Driver.UserName,
                tripRequest.Travel.StartLocation.Details,
                tripRequest.Travel.EndLocation.Details,
                (DateTime)tripRequest.Travel.DepartureTime,
                tripRequest.Status.ToString());
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

            return new TripRequestResponse(
                passenger.UserName,
                trip.Driver.UserName,
                travel.StartLocation.Details,
                travel.EndLocation.Details,
                (DateTime)travel.DepartureTime,
                trip.Status.ToString());
        }

        public async Task<string> DeleteAsync(User loggedUser, int tripRequestId)
        {
            var tripRequest = await this.tripRequestRepository.GetByIdAsync(tripRequestId);
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, tripRequest.PassengerId);

            return await this.tripRequestRepository.DeleteAsync(tripRequestId);   
        }

        public async Task<string> EditRequestAsync(User loggedUser,int tripId,  string answer)
        {
            var tripRequestToUpdate = await this.tripRequestRepository.GetByIdAsync(tripId);
            var driverId = tripRequestToUpdate.Travel.DriverId;
            var travel = await this.travelRepository.GetByIdAsync(tripRequestToUpdate.TravelId);

            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, driverId);

            var currentAnswer = await this.tripRequestValidator.ValidateStatusOfTripRequest(tripRequestToUpdate,answer);

            if (currentAnswer.Equals("approve") && travel.AvailableSeats > 0)
            {
              await this.travelRepository.AddUserToTravelAsync(travel.Id, tripRequestToUpdate.PassengerId);
            }
            else if (currentAnswer.Equals("approve") && travel.AvailableSeats == 0)
            {
                return "I'm sorry, but there are no seats available for this trip";
            }
            else if (currentAnswer.Equals("decline"))
            {
                await this.travelRepository.RemoveUserToTravelAsync(travel.Id, tripRequestToUpdate.PassengerId);
            }

            return await this.tripRequestRepository.EditRequestAsync(tripRequestToUpdate, currentAnswer);

        }
        public async Task<IEnumerable<TripRequestResponse>> SeeAllHisDriverRequestsAsync(User loggedUser, string driverId)
        {
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, driverId);


            var tripRequests = await this.tripRequestRepository.SeeAllHisDriverRequestsAsync(driverId);

            return tripRequests.Select(x => new TripRequestResponse(
               x.Passenger.UserName,
               x.Driver.UserName,
               x.Travel.StartLocation.Details,
               x.Travel.EndLocation.Details, (DateTime)
               x.Travel.DepartureTime,
               x.Status.ToString()));
        }

        public async Task<IEnumerable<TripRequestResponse>> SeeAllHisPassengerRequestsAsync(User loggedUser, string passengerId)
        {
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, passengerId);


            var tripRequests = await this.tripRequestRepository.SeeAllHisPassengerRequestsAsync(passengerId);

            return tripRequests.Select(x => new TripRequestResponse(
               x.Passenger.UserName,
               x.Driver.UserName,
               x.Travel.StartLocation.Details,
               x.Travel.EndLocation.Details, (DateTime)
               x.Travel.DepartureTime,
               x.Status.ToString()));
        }
    }
}

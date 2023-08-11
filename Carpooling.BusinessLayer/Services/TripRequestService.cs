using AutoMapper;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Enums;
using CarPooling.Data.Models.ViewModels;
using CarPooling.Data.Repositories.Contracts;
using System.Diagnostics;

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
                x.Travel.EndLocation.Details, (DateTime)
                x.Travel.DepartureTime,
                x.Status.ToString()));
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

        public async Task<TripRequest> CreateTripRequestForMVCAsync(User loggedUser, TripRequest request)
        {
            var travel = await this.travelRepository.GetByIdAsync(request.TravelId);
            var driverId = travel.DriverId;
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, loggedUser.Id);

            var trips = await this.tripRequestRepository.GetAllAsync();

            if (trips.Any(x => x.PassengerId.Equals(loggedUser.Id) && x.TravelId.Equals(request.TravelId)))
            {
                throw new DublicateEntityException("You already has request for this travel.");
            }
            if (driverId.Equals(loggedUser.Id))
            {
                throw new DriverPassengerMachingException("You cannot apply for your own travel!");
            }
            var trip = await this.tripRequestRepository.CreateAsync(driverId, loggedUser.Id, travel.Id);
            return trip;
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
            var travelId = tripRequest.TravelId;
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, tripRequest.PassengerId);

            if (tripRequest.Status.ToString().ToLower() == "approved")
            {
                await this.travelRepository.RemoveUserToTravelAsync(travelId, tripRequest.PassengerId);
            }

            return await this.tripRequestRepository.DeleteAsync(tripRequestId);
        }

        public async Task<string> EditRequestAsync(User loggedUser, int tripId, string answer)
        {
            var tripRequestToUpdate = await this.tripRequestRepository.GetByIdAsync(tripId);
            var driverId = tripRequestToUpdate.Travel.DriverId;
            var travel = await this.travelRepository.GetByIdAsync(tripRequestToUpdate.TravelId);

            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, driverId);

            var currentAnswer = await this.tripRequestValidator.ValidateStatusOfTripRequest(tripRequestToUpdate, answer);

            if (currentAnswer.Equals("approve") && travel.AvailableSeats > 0)
            {
                await this.travelRepository.AddUserToTravelAsync(travel.Id, tripRequestToUpdate.PassengerId);
            }
            else if (currentAnswer.Equals("approve") && travel.AvailableSeats == 0)
            {
                return "There are no seats available for this trip.";
            }
            else if (currentAnswer.Equals("decline") && tripRequestToUpdate.Status.ToString().ToLower() == "approved")
            {
                await this.travelRepository.RemoveUserToTravelAsync(travel.Id, tripRequestToUpdate.PassengerId);
            }


            return await this.tripRequestRepository.EditRequestAsync(tripRequestToUpdate, currentAnswer);

        }

        public async Task<string> EditRequestMVCAsync(User loggedUser, int tripId, string answer)
        {
            var tripRequestToUpdate = await this.tripRequestRepository.GetByIdAsync(tripId);
            var driverId = tripRequestToUpdate.Travel.DriverId;
            var travel = await this.travelRepository.GetByIdAsync(tripRequestToUpdate.TravelId);

            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, driverId);

            var currentAnswer = await this.tripRequestValidator.ValidateStatusOfTripRequest(tripRequestToUpdate, answer);

            if (currentAnswer.Equals("approve") && travel.AvailableSeats > 0)
            {
                await this.travelRepository.AddUserToTravelAsync(travel.Id, tripRequestToUpdate.PassengerId);
            }
            else if (currentAnswer.Equals("approve") && travel.AvailableSeats == 0)
            {
                throw new UnauthorizedAccessException("I'm sorry, but there are no seats available for this trip");
            }
            else if (currentAnswer.Equals("decline") && tripRequestToUpdate.Status.ToString().ToLower() == "approved")
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

        public async Task<IEnumerable<PassengersListViewModel>> SeeAllHisDriverPassengersMVCAsync(User loggedUser, string driverId)
        {
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, driverId);

            var tripRequests = await this.tripRequestRepository.SeeAllHisDriverRequestsMVCAsync(driverId);

            var passengers = tripRequests
                  .Select(tripRequest => tripRequest.Passenger)
                  .Where(passenger => passenger != null && !passenger.IsDeleted)
                  .ToList();

            var passengerViewModel = new List<PassengersListViewModel>();

            foreach (var trip in tripRequests.Where(x=> x.Status == TripRequestEnum.Approved))
            {
                passengerViewModel.Add(new PassengersListViewModel
                {
                    DepartureTime = (DateTime)trip.Travel.DepartureTime,
                    DepartureCity = trip.Travel.StartLocation.City,
                    DepartureAddress = trip.Travel.StartLocation.Details,
                    ArrivalCity = trip.Travel.EndLocation.City,
                    ArrivalAddress = trip.Travel.EndLocation.Details,
                    Username = trip.Passenger.UserName,
                    AverageRating = trip.Passenger.AverageRating
                });             
            }

            return passengerViewModel;
        }

        public async Task<IEnumerable<TripRequestViewResponseModel>> SeeAllHisDriverRequestsMVCAsync(User loggedUser, string driverId)
        {
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, driverId);


            var tripRequests = await this.tripRequestRepository.SeeAllHisDriverRequestsMVCAsync(driverId);

            return tripRequests.Select(x => new TripRequestViewResponseModel(
               x.Id,
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
        public async Task<IEnumerable<TripRequestViewResponseModel>> SeeAllHisPassengerRequestsMVCAsync(User loggedUser, string passengerId)
        {
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, passengerId);


            var tripRequests = await this.tripRequestRepository.SeeAllHisPassengerRequestsMVCAsync(passengerId);

            return tripRequests.Select(x => new TripRequestViewResponseModel(
               x.Id,
               x.Passenger.UserName,
               x.Driver.UserName,
               x.Travel.StartLocation.Details,
               x.Travel.EndLocation.Details, (DateTime)
               x.Travel.DepartureTime,
               x.Status.ToString()));
        }


    }
}

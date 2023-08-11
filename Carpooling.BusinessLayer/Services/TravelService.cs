using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;

namespace Carpooling.BusinessLayer.Services
{
    public class TravelService : ITravelService
    {
        private readonly ITravelRepository travelRepository;
        private readonly IMapper mapper;
        private readonly IAddressRepository addressRepository;
        private readonly ICarRepository carRepository;
        private readonly ITravelValidator travelValidator;
        private readonly IUserValidation userValidation;
        private readonly IMapService mapService;

        public TravelService(ITravelRepository travelRepository, IMapper mapper, IAddressRepository addressRepository, ICarRepository carRepository, ITravelValidator travelValidator, IUserValidation userValidation, IMapService mapService)
        {
            this.travelRepository = travelRepository;
            this.mapper = mapper;
            this.addressRepository = addressRepository;
            this.carRepository = carRepository;
            this.travelValidator = travelValidator;
            this.userValidation = userValidation;
            this.mapService = mapService;
        }

        public async Task<IEnumerable<TravelResponse>> GetAllAsync()
        {
            var travels = await this.travelRepository.GetAllAsync();

            return travels.Select(x => mapper.Map<TravelResponse>(x));
        }

        public async Task<IEnumerable<Travel>> GetAllTravelAsync()
        {
            return await this.travelRepository.GetAllAsync();
        }
        public async Task<TravelResponse> GetByIdAsync(int travelId)
        {
            return this.mapper.Map<TravelResponse>(await this.travelRepository.GetByIdAsync(travelId));
        }
        public async Task<Travel> CreateTravelForMVCAsync(User loggedUser, Travel travel)
        {
            //mapService.GetDirection();
            //Add Duration/Trip to travel ? or smth else

            await this.travelValidator.ValidateIsLoggedUserAreDriver(loggedUser);
            if (loggedUser.IsBlocked)
            {
                throw new UnauthorizedOperationException($"You can't create travel because you're banned.");
            }
            if (!loggedUser.Cars.Any(x => x.Registration == travel.Car.Registration))
            {
                throw new EntityUnauthorizatedException("The driver cannot operate a car that is not owned by them.");
            }
            if (!await this.travelValidator.ValidateIsNewTravelPossible(travel.DriverId, (DateTime)travel.DepartureTime, (DateTime)travel.ArrivalTime))
            {
                throw new ArgumentException("This trip cannot be made because the driver has another trip at the time.");
            }
            Car car = loggedUser.Cars.Where(x => x.Registration == travel.Car.Registration)
                .FirstOrDefault();
            travel.Car = car;
            travel.Driver = loggedUser;
            travel.IsCompleted = false;
            travel.DriverId = loggedUser.Id;
            return await travelRepository.CreateTravelAsync(travel);
        }
        public async Task<TravelResponse> CreateTravelAsync(User loggedUser, TravelRequest travelRequest)
        {
            await this.travelValidator.ValidateIsLoggedUserAreDriver(loggedUser);
            //  var driverCar = loggedUser.Cars.FirstOrDefault();

            var car = await this.carRepository.GetByIdAsync(travelRequest.CarId);

            if (loggedUser.IsBlocked)
            {
                throw new UnauthorizedOperationException($"You can't create travel because you're banned.");
            }

            if (!loggedUser.Cars.Any(x => x.Id == travelRequest.CarId))
            {
                throw new EntityUnauthorizatedException("The driver cannot operate a car that is not owned by them.");
            }

            var startLocation = await this.addressRepository.GetByIdAsync(travelRequest.StartLocationId);
            var destination = await this.addressRepository.GetByIdAsync(travelRequest.DestionationId);

            var travel = new Travel
            {
                DriverId = travelRequest.DriverId,
                DepartureTime = travelRequest.DepartureTime,
                ArrivalTime = travelRequest.ArrivalTime,
                StartLocation = startLocation,
                AvailableSeats = travelRequest.AvailableSeats,
                IsCompleted = false,
                EndLocation = destination,
                Car = car
            };

            if (!await this.travelValidator.ValidateIsNewTravelPossible(travel.DriverId, (DateTime)travel.DepartureTime, (DateTime)travel.ArrivalTime))
            {
                throw new ArgumentException("This trip cannot be made because the driver has another trip at the time.");
            }


            var createdTravel = await this.travelRepository.CreateTravelAsync(travel);

            return new TravelResponse
            {
                StartLocationName = createdTravel.StartLocation.Details,
                DestinationName = createdTravel.EndLocation.Details,
                DepartureTime = (DateTime)createdTravel.DepartureTime,
                ArrivalTime = (DateTime)createdTravel.ArrivalTime,
                AvailableSeats = (int)createdTravel.AvailableSeats,
                IsCompleted = false,
                CarRegistration = createdTravel.Car.Registration
            };
        }

        public async Task<string> DeleteAsync(User loggedUser, int travelId)
        {
            var travel = await this.travelRepository.GetByIdAsync(travelId);
            if (travel.IsDeleted)
            {
                return "This travel is already deleted.";
            }
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, travel.DriverId);

            return await this.travelRepository.DeleteAsync(travelId);
        }
        public async Task<string> DeleteMVCAsync(User loggedUser, int travelId)
        {
            var travel = await this.travelRepository.GetByIdAsync(travelId);
            if (travel.IsDeleted)
            {
                throw new UnauthorizedAccessException("This travel is already deleted.");
            }
            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, travel.DriverId);

            return await this.travelRepository.DeleteAsync(travelId);
        }
        public async Task<string> SetTravelToIsCompleteAsync(User loggedUser, int id)
        {
            var travel = await this.travelRepository.GetByIdAsync(id);

            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, travel.DriverId);

            if (travel.IsCompleted == true)
            {
                return "This travel is already completed.";
            }

            return await this.travelRepository.SetTravelToIsCompleteAsync(travel);

        }
        public async Task<string> SetTravelToIsCompleteMVCAsync(User loggedUser, int id)
        {
            var travel = await this.travelRepository.GetByIdAsync(id);

            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, travel.DriverId);

            if (travel.IsCompleted == true)
            {
                throw new UnauthorizedAccessException("This travel is already completed.");
            }

            return await this.travelRepository.SetTravelToIsCompleteAsync(travel);

        }

        public async Task<TravelResponse> UpdateAsync(User loggedUser, int travelId, TravelUpdateDto travelDataForUpdate)
        {
            var travelToUpdate = await this.travelRepository.GetByIdAsync(travelId);

            if (loggedUser.IsBlocked)
            {
                throw new UnauthorizedOperationException($"You can't update the travel because you're banned.");
            }

            await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, travelToUpdate.DriverId);

            if (!await this.travelValidator.CheckIsUpdateDataAreValid(travelToUpdate, travelDataForUpdate))
            {
                throw new UnauthorizedOperationException("Please put correct input data for update.");
            }

            var startLocation = await this.addressRepository.GetByIdAsync(travelDataForUpdate.StartLocationId);
            var endLocation = await this.addressRepository.GetByIdAsync(travelDataForUpdate.DestionationId);
            var car = await this.carRepository.GetByIdAsync(travelDataForUpdate.CarId);

            var travelForUpdate = new Travel
            {
                DriverId = travelToUpdate.DriverId,
                StartLocation = startLocation,
                EndLocation = endLocation,
                DepartureTime = travelDataForUpdate.DepartureTime,
                ArrivalTime = travelDataForUpdate.ArrivalTime,
                AvailableSeats = travelDataForUpdate.AvailableSeats,
                Car = car
            };

            var updatedTravel = await this.travelRepository.UpdateAsync(travelId, travelForUpdate);

            var updatedTravelResponse = new TravelResponse
            {
                StartLocationName = updatedTravel.StartLocation.City,
                DestinationName = updatedTravel.EndLocation.City,
                DepartureTime = (DateTime)updatedTravel.DepartureTime,
                ArrivalTime = (DateTime)updatedTravel.ArrivalTime,
                IsCompleted = (bool)updatedTravel.IsCompleted,
                AvailableSeats = (int)updatedTravel.AvailableSeats,
                CarRegistration = updatedTravel.Car.Registration
            };


            return updatedTravelResponse;
        }

        public async Task<IEnumerable<Travel>> FilterTravelsAndSortForMVCAsync(string sortBy)
        {
            var travels = await travelRepository.FilterTravelsAndSortAsync(sortBy); // can sort by create, startlocation, endlocation, departuretime, arrivaltime
            return travels;
        }
        public async Task<IEnumerable<TravelResponse>> FilterTravelsAndSortAsync(string sortBy)
        {
            var travels = await travelRepository.FilterTravelsAndSortAsync(sortBy);

            var travelResponses = travels.Select(x => new TravelResponse
            {
                StartLocationName = x.StartLocation.Details,
                DestinationName = x.EndLocation.Details,
                DepartureTime = (DateTime)x.DepartureTime,
                ArrivalTime = (DateTime)x.ArrivalTime,
                AvailableSeats = (int)x.AvailableSeats,
                IsCompleted = (bool)x.IsCompleted,
                CarRegistration = x.Car.Registration
            });
            return travelResponses;
        }


    }
}

using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public City Create(City city, User user)
        {
            if (user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("Only non-blocked users can create city location!");
            }
            city.User = user;
            return _cityRepository.Create(city);
        }

        public City Delete(int id, User user)
        {
            //ToDo when we have user roles.
            City cityToDelete = GetById(id);
            if(!cityToDelete.User.UserName.Equals(user.UserName) && user.IsBlocked==true && user.IsAdmin==false)
            {
                throw new UnauthorizedOperationException("Only owner or admin can remove cities!");
            }
            _cityRepository.Delete(id);
            return cityToDelete;
        }

        public List<City> GetAll()
        {
            return _cityRepository.GetAll();
        }

        public City GetById(int id)
        {
            return _cityRepository.GetById(id);
        }

        public City GetByName(string name)
        {
            return _cityRepository.GetByName(name);
        }

        public City Update(int id, User user, City city)
        {
            //ToDo when we have user roles.
            City cityToUpdate = GetById(id);
            if (!city.User.UserName.Equals(user.UserName) && user.IsBlocked == true && user.IsAdmin == false)
            {
                throw new UnauthorizedOperationException("Only owner or admin can update cities!");
            }
            cityToUpdate = _cityRepository.Update(id, city);
            return cityToUpdate;
        }
    }
}

using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;

namespace Carpooling.BusinessLayer.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressService)
        {
            _addressRepository = addressService;
        }
        public Address Create(Address address, User user)
        {
            //ToDo when we have user roles.
            if(user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("Only non-banned users can create addresses!");
            }
            throw new NotImplementedException();
        }

        public Address Delete(int id, User user)
        {
            //ToDo when we have user roles.
            throw new NotImplementedException();
            
        }

        public List<Address> FilterByAddressAndSort(string addressName, string sortBy)
        {
            return _addressRepository.FilterByAddressAndSort(addressName, sortBy);
        }

        public List<Address> GetAll()
        {
            return _addressRepository.GetAll();
        }

        public Address GetById(int id)
        {
            return _addressRepository.GetById(id);
        }

        public Address GetByName (string name)
        {
            return _addressRepository.GetByName(name);
        }

        public Address Update(int id, User user, Address address)
        {
            //ToDo when we have user roles.
            throw new NotImplementedException();
        }
    }
}

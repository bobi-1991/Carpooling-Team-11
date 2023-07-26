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
            address.User = user;
            return _addressRepository.Create(address);
        }

        public Address Delete(int id, User user)
        {
            //ToDo when we have user roles.
            Address addressToDelete = _addressRepository.GetById(id);
            if(!addressToDelete.User.UserName.Equals(user.UserName) && user.IsBlocked==true && user.IsAdmin==false)
            {
                throw new UnauthorizedOperationException("Only owner of the address or admin can delete!");
            }
            _addressRepository.Delete(id);
            return addressToDelete;
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
            Address addressToUpdate = GetById(id);
            if (!address.User.UserName.Equals(user.UserName) && user.IsBlocked == true && user.IsAdmin == false)
            {
                throw new UnauthorizedOperationException("Only owner or admin can update addresses!");
            }
            addressToUpdate = _addressRepository.Update(id, address);
            return addressToUpdate;
        }
    }
}

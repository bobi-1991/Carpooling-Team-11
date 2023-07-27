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
            if(user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("Only non-banned users can create addresses!");
            }
            
            return _addressRepository.Create(address);
        }

        public Address Delete(int id, User user)
        {
            Address addressToDelete = GetById(id); 
            if(user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("You do not have permission to delete this address!");
            }
            addressToDelete = _addressRepository.Delete(id);
            return addressToDelete;
        }

        public List<Address> GetAll()
        {
            return _addressRepository.GetAll();
        }

        public Address GetById(int id)
        {
            return _addressRepository.GetById(id);
        }


        public Address Update(int id, User user, Address address)
        {
            Address addressToUpdate = GetById(id);
            if(user.IsBlocked == true) 
            {
                throw new UnauthorizedOperationException("You do not have permission to update the address!");
            }

            addressToUpdate = _addressRepository.Update(id, address);

            return addressToUpdate;
        }
    }
}

using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Address> CreateAsync(Address address, User user)
        {
            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("Only non-banned users can create addresses!");
            }

            return await _addressRepository.CreateAsync(address);
        }

        public async Task<Address> DeleteAsync(int id, User user)
        {
            Address addressToDelete = await GetByIdAsync(id);

            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("You do not have permission to delete this address!");
            }

            addressToDelete = await _addressRepository.DeleteAsync(id);
            return addressToDelete;
        }

        public async Task<List<Address>> GetAllAsync()
        {
            return await _addressRepository.GetAllAsync();
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            
            return await _addressRepository.GetByIdAsync(id);
        }

        public async Task<Address> UpdateAsync(int id, User user, Address address)
        {
            Address addressToUpdate = await GetByIdAsync(id);

            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("You do not have permission to update the address!");
            }

            addressToUpdate = await _addressRepository.UpdateAsync(id, address);
            return addressToUpdate;
        }
    }
}
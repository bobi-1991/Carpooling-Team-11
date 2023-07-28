using CarPooling.Data.Models;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface IAddressService
    {
        Task<List<Address>> GetAllAsync();
        Task<Address> GetByIdAsync(int id);
        Task<Address> CreateAsync(Address address, User user);
        Task<Address> UpdateAsync(int id, User user, Address address);
        Task<Address> DeleteAsync(int id, User user);
    }
}

using CarPooling.Data.Models;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAllAsync();
        Task<Address> GetByIdAsync(int id);
        Task<Address> CreateAsync(Address address);
        Task<Address> UpdateAsync(int id, Address address);
        Task<Address> DeleteAsync(int id);
    }  
}

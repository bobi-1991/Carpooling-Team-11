using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarPooling.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CarPoolingDbContext _context;
        public AddressRepository(CarPoolingDbContext context)
        {
            _context = context;
        }
        public Address Create(Address address)
        {
            _context.Addresses.Add(address);
            address.CreatedOn = DateTime.Now;
            _context.SaveChanges();
            return address;
        }

        public Address Delete(int id)
        {
            Address addressToRemove=GetById(id);

            addressToRemove.IsDeleted=true;
            addressToRemove.DeletedOn = DateTime.Now;

            _context.SaveChanges();
            return addressToRemove;
        }


        public List<Address> GetAll()
        {
            return _context.Addresses
                .Include(a => a.Details)
                .Include(a => a.City)
                .Include(a => a.Country)
                .Include(a=>a.CreatedOn)
                .Include(a=>a.UpdatedOn)
                .Include(a=>a.DeletedOn)
                .ToList();
        }

        public Address GetById(int id)
        {
            Address address = _context.Addresses.Where(a=>a.Id == id)
                .Include(a=>a.Details)
                .Include(a=>a.City)
                .Include(a=>a.Country)
                .Include(a => a.CreatedOn)
                .Include(a => a.UpdatedOn)
                .Include(a => a.DeletedOn)
                .FirstOrDefault();

            return address ?? throw new EntityNotFoundException($"Could not find an address with id: {id}!");
        }

        public Address Update(int id, Address address)
        {
            Address addressToUpdate = GetById(id);

            addressToUpdate.City = address.City;
            addressToUpdate.Country = address.Country;
            addressToUpdate.Details = address.Details;
            addressToUpdate.UpdatedOn = DateTime.Now;

            _context.Update(addressToUpdate);
            _context.SaveChanges();

            return addressToUpdate;
        }
    }
}

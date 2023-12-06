using Pokemon_Wep_Api.Data;
using Pokemon_Wep_Api.interfaces;
using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.Repository
{
    public class CountryRpository:ICountryRpository
    {
        private readonly ApplicationDbContext _context;

        public CountryRpository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CountryExists(int CountryId)
        {
            return _context.Countries.Any(c => c.Id == CountryId);
        }

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int CountryId)
        {
            return _context.Countries.Where(w => w.Id == CountryId).FirstOrDefault();
        }

        public Country GetCountryByOwner(int OwnerId)
        {
            return _context.Owners.Where(o => o.Id == OwnerId).Select(s => s.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int CountryId)
        {
            return _context.Owners.Where(c => c.Country.Id == CountryId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}

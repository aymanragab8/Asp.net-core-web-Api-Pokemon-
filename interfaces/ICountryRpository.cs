using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.interfaces
{
    public interface ICountryRpository
    {
        ICollection<Country>GetCountries();
        Country GetCountry(int CountryId);
        bool CountryExists(int CountryId);
        Country GetCountryByOwner(int OwnerId);
        ICollection<Owner>GetOwnersFromACountry(int CountryId);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
       
    }
}

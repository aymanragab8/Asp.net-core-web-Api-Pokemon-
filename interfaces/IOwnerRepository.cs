using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int OwnerId);
        bool OwnerExists(int OwnerId);
        ICollection<Pokemon>GetPokemonByOwner(int OwnerId);
        ICollection<Owner> GetOwnerOfAPoemon(int PokeId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}

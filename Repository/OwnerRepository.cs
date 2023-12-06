using Pokemon_Wep_Api.Data;
using Pokemon_Wep_Api.interfaces;
using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplicationDbContext _context;

        public OwnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int OwnerId)
        {
            return _context.Owners.Where(w => w.Id == OwnerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPoemon(int PokeId)
        {
            return _context.PokemonOwners.Where(o => o.Pokemon.Id == PokeId).Select(s => s.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int OwnerId)
        {
            return _context.PokemonOwners.Where(o =>o.Owner.Id==OwnerId).Select(s => s.Pokemon).ToList();
        }

        public bool OwnerExists(int OwnerId)
        {
            return _context.Owners.Any(o => o.Id == OwnerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}

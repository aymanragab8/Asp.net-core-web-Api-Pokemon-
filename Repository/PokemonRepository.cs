using Pokemon_Wep_Api.Data;
using Pokemon_Wep_Api.Dto;
using Pokemon_Wep_Api.interfaces;
using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.Repository
{
    public class PokemonRepository:IPokemonRepository
    {
        private readonly ApplicationDbContext _context;

        public PokemonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int OwnerId, int CategoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Owners.Where(o =>o.Id==OwnerId).FirstOrDefault();
            var category = _context.Categories.Where(c => c.Id == CategoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _context.Add(pokemonCategory);

            _context.Add(pokemon);

            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            var pokemon = _context.Pokemons.Where(p => p.Id == id).FirstOrDefault();
            return pokemon;
        }

        public Pokemon GetPokemon(string name)
        {
            var pokemon = _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
            return pokemon;
        }

        public double GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);
            if (review.Count()<= 0)
                return 0;
            return ((double)review.Sum(s => s.Rating)/review.Count());
 
        }

        public ICollection<Pokemon> GetPokemons()
        {
            var pks = _context.Pokemons.ToList();
            return pks;
        }

        public Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate)
        {
            return GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemons.Any(p => p.Id == pokeId);
        }

        public bool Save()
        {
            var saved =_context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdatePokemon(int OwnerId, int CategoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}

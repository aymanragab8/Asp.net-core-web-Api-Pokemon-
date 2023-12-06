using Pokemon_Wep_Api.Dto;
using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon>GetPokemons();
        Pokemon GetPokemon(int id );
        Pokemon GetPokemon(string name);
        Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate);
        double GetPokemonRating(int pokeId);
        bool PokemonExists (int pokeId);
        bool CreatePokemon(int OwnerId , int CategoryId , Pokemon pokemon);
        bool UpdatePokemon(int OwnerId, int CategoryId, Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();


    }
}

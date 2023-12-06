using AutoMapper;
using Pokemon_Wep_Api.Dto;
using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.Helper
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CountryDto, Country>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<Country, CountryDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewerDto, Reviewer>();


        }
    }
}

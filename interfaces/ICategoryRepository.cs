using Pokemon_Wep_Api.Models;
using System.Collections.ObjectModel;

namespace Pokemon_Wep_Api.interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category>GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
        bool CategoriesExists(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}

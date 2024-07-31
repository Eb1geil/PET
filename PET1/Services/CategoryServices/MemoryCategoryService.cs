using PET1.Domain.Models;
using PET1.Domain.Entities;

namespace PET1.Services.CategoryServices
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<Dictionary<string, List<Category>>>>
        GetCategoryListAsync()
        {
            var categoriesByGroup = new Dictionary<string, List<Category>>
{
    { "Game Genres", new List<Category>
        {
            new Category { Id = 1, Name = "Action", NormalizedName = "action" },
            new Category { Id = 2, Name = "Strategy", NormalizedName = "strat" },
            // другие категории жанров игр
        }
    },
    { "Plot Types", new List<Category>
        {
            new Category { Id = 4, Name = "Adventury", NormalizedName = "adv" },
            new Category { Id = 5, Name = "Cartoon", NormalizedName = "cart" },
            new Category { Id = 6, Name = "Drama", NormalizedName = "dram" },
            // другие категории типов сюжета
        }
    }
};
            var result = new ResponseData<Dictionary<string, List<Category>>>(true, categoriesByGroup);
            result.Data = categoriesByGroup;
            return Task.FromResult(result);
        }
    }
}

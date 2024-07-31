using Microsoft.AspNetCore.Mvc;
using PET1.Domain.Entities;
using PET1.Domain.Models;
using PET1.Services.CategoryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace PET1.Services.ProductServices
{
    public class MemoryProductService : IProductService
    {
        private IConfiguration _config;
        private ListModel<Game> _Games;
        private Dictionary<string, List<Category>> _categoriesByGroup;

        public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _categoriesByGroup = categoryService.GetCategoryListAsync()
                                                .Result
                                                .Data
                                                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
            SetupData();
        }

        public Task<ResponseData<Game>> CreateProductAsync(Game product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Game>>> GetProductListAsync(string? CategoryNormalizedName, int pageNo = 1)
        {
            List<Game> responseGame = null;

            int itemsPerPage = 3; // Set a default value
            var itemsPerPageConfig = _config.GetSection("ItemsPerPage").Value;
            if (int.TryParse(itemsPerPageConfig, out int parsedItemsPerPage))
            {
                itemsPerPage = parsedItemsPerPage;
            }

            if (CategoryNormalizedName != null)
            {
                responseGame = _Games.Items
                    .Where(c => c.Category != null && c.Category.NormalizedName.Equals(CategoryNormalizedName))
                    .ToList();
            }

            else
            {
                responseGame = _Games.Items.ToList();
            }

            ResponseData<ListModel<Game>> result = new ResponseData<ListModel<Game>>(true, new ListModel<Game>()
            {
                Items = responseGame.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                CurrentPage = pageNo,
                TotalPages = Convert.ToInt32(Math.Ceiling((double)responseGame.Count / itemsPerPage))
            });

            return Task.FromResult(result);
        }

        // Fetch games based on the category
        //private List<Game>? FetchGamesByCategory(string? categoryNormalizedName)
        //{
        //    if (string.IsNullOrEmpty(categoryNormalizedName))
        //    {
        //        return _Games;
        //    }

        //    return _Games.Where(g => g.Category != null && g.Category.NormalizedName.Equals(categoryNormalizedName, StringComparison.OrdinalIgnoreCase)).ToList();
        //}

        public Task UpdateProductAsync(int id, Game product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize lists
        /// </summary>
        private void SetupData()
        {
            List<Game> list = new List<Game>()
            {
                new Game {Id = 1, Name="Elden Ring", Description="GOTY 2022, FromSoftware's game, project of Hidetaka Miyadzaki", Price =20, ImgPath="Images/Elden_Ring.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals("action"))},
                new Game {Id = 2, Name="Game 2", Description="Description 2", Price = 330, ImgPath="Images/Game_2.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Game {Id = 3, Name="Game 3", Description="Description 3", Price = 330, ImgPath="Images/Game_3.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Game {Id = 4, Name="Game 2", Description="Description 2", Price = 330, ImgPath="Images/Game_2.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Game {Id = 5, Name="Game 3", Description="Description 3", Price = 330, ImgPath="Images/Game_3.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Game {Id = 6, Name="Game 2", Description="Description 2", Price = 330, ImgPath="Images/Game_2.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Game {Id = 7, Name="Game 3", Description="Description 3", Price = 330, ImgPath="Images/Game_3.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Game {Id = 8, Name="Game 2", Description="Description 2", Price = 330, ImgPath="Images/Game_2.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Game {Id = 9, Name="Game 3", Description="Description 3", Price = 330, ImgPath="Images/Game_3.jpg", Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))}
            };
            _Games = new ListModel<Game>() { Items = list };
        }
    }
}

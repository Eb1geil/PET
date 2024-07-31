using Microsoft.AspNetCore.Mvc;
using PET1.Domain.Entities;
using PET1.Domain.Models;
using PET1.Services.CategoryServices;

namespace PET1.Services.MovieServices
{
    public class MemoryMovieService : IMovieService
    {
        private IConfiguration _config;
        private ListModel<Movies> _Movies;
        private Dictionary<string, List<Category>> _categoriesByGroup;

        public MemoryMovieService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _categoriesByGroup = categoryService.GetCategoryListAsync()
                                                .Result
                                                .Data
                                                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
            SetupData();
        }


        public Task<ResponseData<Movies>> CreateProductAsync(Movies product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }



        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Movies product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }


        public Task<ResponseData<Movies>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Movies>>> GetProductListAsync(string? CategoryNormalizedName, int pageNo = 1)
        {
            List<Movies> responseMovies = null;

            int itemsPerPage = 3; // Set a default value
            var itemsPerPageConfig = _config.GetSection("ItemsPerPage").Value;
            if (int.TryParse(itemsPerPageConfig, out int parsedItemsPerPage))
            {
                itemsPerPage = parsedItemsPerPage;
            }

            if (CategoryNormalizedName != null)
            {
                responseMovies = _Movies.Items
                    .Where(c => c.Category != null && c.Category.NormalizedName.Equals(CategoryNormalizedName))
                    .ToList();
            }

            else
            {
                responseMovies = _Movies.Items.ToList();
            }

            ResponseData<ListModel<Movies>> result = new ResponseData<ListModel<Movies>>(true, new ListModel<Movies>()
            {
                Items = responseMovies.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                CurrentPage = pageNo,
                TotalPages = Convert.ToInt32(Math.Ceiling((double)responseMovies.Count / itemsPerPage))
            });

            return Task.FromResult(result);
        }

        // Fetch Moviess based on the category
        //private List<Movies>? FetchMoviessByCategory(string? categoryNormalizedName)
        //{
        //    if (string.IsNullOrEmpty(categoryNormalizedName))
        //    {
        //        return _Movies;
        //    }

        //    return _Movies.Where(g => g.Category != null && g.Category.NormalizedName.Equals(categoryNormalizedName, StringComparison.OrdinalIgnoreCase)).ToList();
        //}


        /// <summary>
        /// Initialize lists
        /// </summary>
        private void SetupData()
        {
            List<Movies> list = new List<Movies>()
            {
                new Movies {
                    Id = 1,
                    Name="Oppenheimer",
                    Description="The story of theoretical physicist Julius Robert Oppenheimer.",
                    Price =20,
                    ImgPath="Images/Oppen.jpg",
                    Category=_categoriesByGroup["Plot Types"].Find(c => c.NormalizedName.Equals("dram"))},

                new Movies {
                    Id = 2,
                    Name="Shôgun",
                    Description="In 1600, the fearless English sailor and Protestant John Blackthorne, who is a pilot on a Dutch merchant ship," +
                    " is looking for a way to Japan to try to oust the Portuguese Catholics from there. ",
                    Price = 10,
                    ImgPath="Images/Shog.jpg",
                    Category=_categoriesByGroup["Plot Types"].Find(c => c.NormalizedName.Equals("adv"))},

                new Movies {
                    Id = 3,
                    Name="Guardians of the Galaxy Vol. 3",
                    Description="Peter Quill cannot come to terms with the loss of Gamora. " +
                    "And at this difficult moment for himself, he will once again assemble his team to protect the universe. " +
                    "After all, if he fails, it will mean only one thing - the Mission is a failure!" +
                    " And this is the end of the entire Guardians of the Galaxy. ",
                    Price = 20,
                    ImgPath="Images/Guard.jpg",
                    Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals("fanc"))},
                new Movies {
                    Id = 4,
                    Name="Movies 2",
                    Description="Description 2",
                    Price = 330,
                    ImgPath="Images/Movies_2.jpg",
                    Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Movies {Id = 5,
                    Name="Movies 3",
                    Description="Description 3",
                    Price = 330,
                    ImgPath="Images/Movies_3.jpg",
                    Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Movies {
                    Id = 6,
                    Name="Movies 2",
                    Description="Description 2",
                    Price = 330,
                    ImgPath="Images/Movies_2.jpg",
                    Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Movies {
                    Id = 7,
                    Name="Movies 3",
                    Description="Description 3",
                    Price = 330,
                    ImgPath="Images/Movies_3.jpg",
                    Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Movies {
                    Id = 8,
                    Name="Movies 2",
                    Description="Description 2",
                    Price = 330,
                    ImgPath="Images/Movies_2.jpg",
                    Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))},
                new Movies {
                    Id = 9,
                    Name="Movies 3",
                    Description="Description 3",
                    Price = 330,
                    ImgPath="Images/Movies_3.jpg",
                    Category=_categoriesByGroup["Game Genres"].Find(c => c.NormalizedName.Equals(""))}
            };
            _Movies = new ListModel<Movies>() { Items = list };
        }
    }
}

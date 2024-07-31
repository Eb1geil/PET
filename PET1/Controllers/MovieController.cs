using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PET1.Domain.Entities;
using PET1.Domain.Models;
using PET1.Services.CategoryServices;
using PET1.Services.ProductServices;
using PET1.Services.MovieServices;

namespace PET1.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;

        public MovieController(IMovieService movieService, ICategoryService categoryService)
        {
            _movieService = movieService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, string? group, int pageNo = 1)
        {
            ResponseData<ListModel<Movies>> productResponse = await _movieService.GetProductListAsync(category, pageNo);
            var categoryList = await _categoryService.GetCategoryListAsync();
            ViewData["categoryGroups"] = categoryList.Data.Keys;
            ViewData["currentGroup"] = group;
            if (!productResponse.Success || !categoryList.Success)
            {
                return NotFound(productResponse.Message ?? categoryList.Message);
            }

            string groupKey = group ?? "";
            if (categoryList.Data.ContainsKey(groupKey))
            {
                ViewData["typeList"] = categoryList.Data[groupKey];
            }
            else
            {
                // Обработка случая, когда группа не найдена
                ViewData["typeList"] = new List<Category>();
            }
            ViewData["currentType"] = category;

            return View(productResponse.Data);
        }

    }
}

using Microsoft.EntityFrameworkCore;
using PET1.Domain.Entities;
using PET1.Domain.Models;
using PET1.Services.MovieServices;
using PET1.Services.ProductServices;

namespace PET1.API.Services.MovieService
{
    public class MovieService : IMovieService
    {
        private readonly int _maxPageSize = 20;
        private readonly IConfiguration _config;
        private readonly ListModel<Movies> _Movies;
        private readonly Dictionary<string, List<Category>> _categoriesByGroup;
        public Task<ResponseData<Movies>> CreateProductAsync(Movies product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Movies>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<ListModel<Movies>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _Movies.Items.AsQueryable();
            var dataList = new ListModel<Movies>();
            query = query
            .Where(d => categoryNormalizedName == null
            || d.Category.NormalizedName.Equals(categoryNormalizedName));
            // количество элементов в списке
            var count = await query.CountAsync();
            if (count == 0)
            {
                return new ResponseData<ListModel<Movies>>
                {
                    Data = dataList
                };
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return new ResponseData<ListModel<Movies>>
                {
                    Data = null,
                    Success = false,
                    Message = "No such page"
                };
            dataList.Items = await query
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            var response = new ResponseData<ListModel<Movies>>
            {
                Data = dataList
            };
            return response;
        }

        public Task UpdateProductAsync(int id, Movies product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}

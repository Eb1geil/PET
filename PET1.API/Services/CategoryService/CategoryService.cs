using Microsoft.EntityFrameworkCore;
using PET1.API.Data;
using PET1.Domain.Entities;
using PET1.Domain.Models;

namespace PET1.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {

        private readonly AppDbContext _db;
        public CategoryService(AppDbContext db) => (_db) = (db);

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                var categories = await _db.Categories.ToListAsync();

                return new ResponseData<List<Category>>(true, categories);
            }
            catch (Exception ex)
            {
                return new ResponseData<List<Category>>(false, ex.Message);
            }
        }
    }
}

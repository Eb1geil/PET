using PET1.Domain.Models;
using PET1.Domain.Entities;
namespace PET1.Services.CategoryServices
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<Dictionary<string, List<Category>>>> GetCategoryListAsync();
    }
}

using System.Text;
using System.Text.Json;
using PET1.Domain.Entities;
using PET1.Domain.Models;
using PET1.Services.CategoryServices;

namespace PET1.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger _logger;

        public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _serializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Genres/");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");

                    return new ResponseData<List<Category>>(false, ex.Message);
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");

            return new ResponseData<List<Category>>(false, $"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        Task<ResponseData<Dictionary<string, List<Category>>>> ICategoryService.GetCategoryListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
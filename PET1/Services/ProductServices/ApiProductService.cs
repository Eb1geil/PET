using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using PET1.Domain.Entities;
using PET1.Domain.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PET1.Services.ProductServices
{
    public class ApiProductService: IProductService
    {
        private readonly HttpContext _httpContext;
        HttpClient _httpClient;
        int _pageSize;
        private readonly IConfiguration _configuration;
        JsonSerializerOptions _serializerOptions;
        ILogger _logger;
        private readonly string _itemsPerPage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _itemsPerPage = configuration.GetSection("ItemsPerPage").Value;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContext = _httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext));

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        public async Task<ResponseData<Game>> CreateProductAsync(Game Game, IFormFile? formFile)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "games");

            var response = await _httpClient.PostAsJsonAsync(uri, Game, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var ResponseData = await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);

                    if (formFile != null)
                    {
                        await SaveImageAsync(ResponseData.Data.Id, formFile);
                    }

                    return ResponseData;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");

                    return new ResponseData<Game>(false, ex.Message);
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");

            return new ResponseData<Game>(false, $"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task DeleteProductAsync(int id)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}games/{id}");

            await _httpClient.DeleteAsync(uri);
        }

        public async Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Games/{id}");

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Game>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");

                    return new ResponseData<Game>(false, ex.Message);
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode.ToString()}");

            return new ResponseData<Game>(false, $"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task<ResponseData<ListModel<Game>>> GetProductListAsync(string? GameTypeNormaizeName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}games/");

            if (GameTypeNormaizeName != null)
            {
                urlString.Append($"{GameTypeNormaizeName}/");
            }

            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            };

           // if (!_itemsPerPage.Equals("3"))
           // {
             //   urlString.Append(QueryString.Create("pageSize", _itemsPerPage));
            //}

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Game>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");

                    return new ResponseData<ListModel<Game>>(false, ex.Message);
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");

            return new ResponseData<ListModel<Game>>(false, $"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task UpdateProductAsync(int id, Game Game, IFormFile? formFile)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            if (formFile != null)
            {
                await SaveImageAsync(Game.Id, formFile);
            }

            var uri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}games/{id}");

            await _httpClient.PutAsJsonAsync(uri, Game, _serializerOptions);
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}games/{id}")
            };

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());

            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;

            await _httpClient.SendAsync(request);
        }
    }
}
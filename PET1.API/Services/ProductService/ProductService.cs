using Microsoft.EntityFrameworkCore;
using PET1.API.Data;
using PET1.Domain.Entities;
using PET1.Domain.Models;
using PET1.Services.ProductServices;
using System.Drawing.Printing;

namespace PET1.API.Services.ProductService
{

    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _maxPageSize = 20;

        public ProductService(AppDbContext db, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) => (_db, _env, _httpContextAccessor) = (db, env, httpContextAccessor);

        public async Task<ResponseData<Game>> CreateProductAsync(Game game, IFormFile? formFile)
        {
            try
            {
                if (game != null && game.Category != null)
                {
                    game.Category = await _db.Categories.FirstOrDefaultAsync(t => t.Id.Equals(game.Category.Id));
                }

                await _db.Games.AddAsync(game);
                await _db.SaveChangesAsync();

                return new ResponseData<Game>(true, game);
            }
            catch (Exception ex)
            {
                return new ResponseData<Game>(false, ex.Message);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var removeItem = await _db.Games.FirstOrDefaultAsync(item => item.Id.Equals(id));

            if (removeItem != null)
            {
                _db.Games.Remove(removeItem);

                await _db.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<Game>> GetProductByIdAsync(int id)
        {
            var query = _db.Games.AsQueryable();

            if (true)
            {
                query.Include(v => v.Category);
            }

            try
            {
                var searchVehicle = await query
                    .AsNoTracking()

                    .FirstOrDefaultAsync(item => item.Id.Equals(id));

                if (searchVehicle != null)
                {
                    return new ResponseData<Game>(true, searchVehicle);
                }

                return new ResponseData<Game>(false, "Not Found");
            }
            catch (Exception ex)
            {
                return new ResponseData<Game>(false, ex.Message);
            }
        }

        public async Task<ResponseData<ListModel<Game>>> GetProductListAsync(string? vehicleTypeNormaizeName, int pageNo = 1, int pageSize = 3)
        {
            try
            {
                if (pageSize > _maxPageSize)
                    pageSize = _maxPageSize;

                var dataList = new ListModel<Game>();

                var query = _db.Games.AsQueryable()
                                        .AsNoTracking()
                                        .Include(v => v.Category)
                                        .Where(d => vehicleTypeNormaizeName == null || d.Category.NormalizedName.Equals(vehicleTypeNormaizeName));

                var count = await query.CountAsync();
                if (count == 0)
                {
                    return new ResponseData<ListModel<Game>>(true, dataList);
                }

                int totalPages = (int)Math.Ceiling(count / (double)pageSize);
                if (pageNo > totalPages)
                {
                    return new ResponseData<ListModel<Game>>(false, "No such page");
                }

                dataList.Items = await query
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                dataList.CurrentPage = pageNo;
                dataList.TotalPages = totalPages;

                return new ResponseData<ListModel<Game>>(true, dataList);
            }
            catch (Exception ex)
            {
                return new ResponseData<ListModel<Game>>(false, ex.Message);
            }
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var game = await _db.Games.FindAsync(id);
            if (game == null)
            {
                return new ResponseData<string>(false, "No item found");
            }

            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;

            var imageFolder = Path.Combine(_env.WebRootPath, "images");

            if (formFile != null)
            {
                if (!String.IsNullOrEmpty(game.ImgPath))
                {
                    var prevImage = Path.Combine(imageFolder, Path.GetFileName(game.ImgPath));
                    if (File.Exists(prevImage))
                    {
                        File.Delete(prevImage);
                    }
                }

                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                var fPath = Path.Combine(imageFolder, fName);

                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                game.ImgPath = $"{host}/images/{fName}";
                await _db.SaveChangesAsync();
            }

            return new ResponseData<string>(true, data: game.ImgPath);
        }

        public async Task UpdateProductAsync(int id, Game game, IFormFile? formFile)
        {
            _db.Games.Update(game);
            await _db.SaveChangesAsync();
        }
    }
}

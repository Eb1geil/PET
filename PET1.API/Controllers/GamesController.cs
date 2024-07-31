//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using PET1.API.Data;
//using PET1.Domain.Entities;
//using PET1.Domain.Models;
//using PET1.API.Services.CategoryService;
//using PET1.Services.ProductServices;
//using System.Configuration;

//namespace PET1.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GamesController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        private readonly ICategoryService _categoryService;
//        private readonly IProductService _productService;
//        private readonly string? categoryNormalizedName;
//        private readonly int pageNo;
//        private readonly int pageSize;
//        private readonly IWebHostEnvironment _env;
//        private readonly IConfiguration _configuration;
//        private readonly string _imagesPath;
//        private readonly string? _appUri;

//        public GamesController(AppDbContext context, IProductService productService, IWebHostEnvironment env, IConfiguration configuration)
//        {
//            _productService = productService;
//            _env = env;
//            _configuration = configuration;

//            _imagesPath = Path.Combine(_env.WebRootPath, "images");
//            _appUri = _configuration.GetSection("appUri").Value;

//        }

//        // GET: api/Games
//        [HttpGet]
//        public async Task<ActionResult<ResponseData<List<Game>>>> GetGames(
//            string? category,
//            int pageNo = 1,
//            int pageSize = 3)
//        {
//            return Ok(await _productService.GetProductListAsync(category, pageNo, pageSize));
//        }

//        // GET: api/Games/{id}
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Game>> GetGame(int id)
//        {
//            var result = await _productService.GetProductByIdAsync(id);
//            if (result == null)
//            {
//                return NotFound();
//            }

//            return Ok(result);
//        }

//        // PUT: api/Games/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutGame(int id, Game game, IFormFile formFile)
//        {
//            await _productService.UpdateProductAsync(id, game, formFile);

//            return Ok();
//        }

//        // POST: api/Games
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Game>> PostGame(Game game, IFormFile formFile)
//        {
//           await _productService.CreateProductAsync(game, formFile);

//            return Ok();
//        }

//        // DELETE: api/Games/5
//        [HttpDelete("{id}")]
//        public IActionResult DeleteGame(int id)
//        {
//            _productService.DeleteProductAsync(id);
//            return Ok();
//        }

//        private bool GameExists(int id)
//        {
//            return _context.Games.Any(e => e.Id == id);
//        }
//    }
//}

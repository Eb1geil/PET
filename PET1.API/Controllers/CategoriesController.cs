//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using PET1.API.Data;
//using PET1.API.Services.CategoryService;
//using PET1.Domain.Entities;

//namespace PET1.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CategoriesController : ControllerBase
//    {
//        private readonly ICategoryService _categoryService;
//        public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;
   

//        // GET: api/Categories
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
//        {
//          await _categoryService.GetCategoryListAsync();
//            return Ok();
//        }

//    }
//}

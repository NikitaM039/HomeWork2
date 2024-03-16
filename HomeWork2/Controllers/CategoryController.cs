using HomeWork2.Abstraction;
using HomeWork2.DB;
using HomeWork2.Models;
using HomeWork2.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;

namespace HomeWork2.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        
        private readonly IProductRepository _productRepository;
        private ProductContext _context;

        public CategoryController(IProductRepository productRepository, ProductContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        [HttpPost(template: "CreateCategory")]
        public IActionResult PostCategory([FromBody] GroupDto groupDto)
        {
            var result = _productRepository.AddGroup(groupDto);
            return Ok(result);
        }


        [HttpGet(template: "GetCategory")]
        public IActionResult GetCategory()
        {
            var result = _productRepository.GetGroups();
            return Ok(result);
        }


        [HttpPatch(template: "UpdateCategory")]
        public IActionResult UpdateCategory(string categoryName, string description, string newCategoryName)
        {
            try
            {
                using (_context)
                {
                    var entity = _context.Categories.FirstOrDefault(e => e.Name.Equals(categoryName));

                    if (entity == null)
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        entity.Name = newCategoryName;
                        entity.Description = description;

                        _context.SaveChanges();
                        return Ok(entity);
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete(template: "DeleteCategory")]
        public IActionResult DeleteCategory(string categoryName)
        {
            try
            {


                using (_context)
                {
                    var entity = _context.Categories.FirstOrDefault(x => x.Name == categoryName);
                    if (entity == null)
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        _context.Categories.Remove(entity);
                        _context.SaveChanges();
                        return Ok();
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }


    }
}
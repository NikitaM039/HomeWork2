using HomeWork2.Abstraction;
using HomeWork2.DB;
using HomeWork2.Models;
using HomeWork2.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace HomeWork2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private ProductContext _productContext;
        private IMemoryCache _memoryCache;

        public ProductController(IProductRepository productRepository, ProductContext productContext, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _productContext = productContext;
            _memoryCache = memoryCache;
        }

        [HttpPost(template: "CreateProduct")]
        public IActionResult PostProduct([FromBody] ProductDto productDto)
        {
            var result = _productRepository.AddProduct(productDto);
            return Ok(result);
        }


        [HttpGet(template: "GetProduct")]
        public IActionResult GetProduct()
        {
            var products = _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet(template: "GetCacheStatus")]
        public ActionResult<MemoryCacheStatistics> GetCacheStatus()
        {
            return _memoryCache.GetCurrentStatistics();

        }

        [HttpGet(template: "GetProductCSV")]
        public FileContentResult GetProductCSV()
        {
            var products = _productRepository.GetProducts();
            var content = GetCSV(products);

            return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "report.csv");
        }

        private string GetCSV(IEnumerable<ProductDto> products)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var product in products)
            {
                sb.Append(product.Name + ";" + product.Description + "\n");
            }
            return sb.ToString();
        }

        [HttpGet(template: "GetCacheCSVUrl")]
        public ActionResult<string> GetCacheCSVUrl()
        {
            var content = _memoryCache.ToString();

            string fileName = null;

            fileName = "Cache" + DateTime.Now.ToBinary().ToString() + ".csv";
            if (content != null)
            {
                System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), content);
                return "https://" + Request.Host.ToString() + "/static/" + fileName;
            }
            return "https://" + Request.Host.ToString() + "/static/" + fileName;

        }

        [HttpGet(template: "GetProductCSVUrl")]
        public ActionResult<string> GetProductCSVUrl()
        {
            var products = _productRepository.GetProducts();
            var content = GetCSV(products);

            string fileName = null;
            fileName = "Products" + DateTime.Now.ToBinary().ToString() + ".csv";
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), content);
            return "https://" + Request.Host.ToString() + "/static/" + fileName;

        }



        [HttpPatch(template: "UpdateProduct")]
        public IActionResult UpdateProduct(int Cost, string CategoryName, string ProductName, string description)
        {
            try
            {
                using (_productContext)
                {
                    var category = _productContext.Categories.FirstOrDefault(e => e.Name.Equals(CategoryName));
                    int numberCategory = category.Id;

                    var entity = _productContext.Products.FirstOrDefault(e => e.Name.Equals(ProductName));

                    if (entity == null)
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        entity.Cost = Cost;
                        //entity.CategoryId = numberCategory;
                        entity.Name = ProductName;
                        entity.Description = description;

                        _productContext.SaveChanges();
                        return Ok(entity);
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete(template: "DeleteProduct")]
        public IActionResult DeleteProduct(string ProductName)
        {
            try
            {


                using (_productContext)
                {
                    var entity = _productContext.Products.FirstOrDefault(x => x.Name == ProductName);
                    if (entity == null)
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        _productContext.Products.Remove(entity);
                        _productContext.SaveChanges();
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
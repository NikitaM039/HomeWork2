using HomeWork2.DB;
using HomeWork2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace HomeWork2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private ProductContext _productContext;

        public StorageController(ProductContext productContext)
        {
            _productContext = productContext;
        }

        [HttpPost(template: "CreateStorage")]
        public IActionResult PostStorage(string StorageName, string description)
        {
            try
            {
                using (_productContext)
                {
                    if (!_productContext.Storages.Any(x => x.Name.ToLower().Equals(StorageName)))
                    {

                        _productContext.Add(new Storage()
                        {

                            Name = StorageName,
                            Description = description

                        });

                        _productContext.SaveChanges();

                        return Ok();
                    }
                    else
                    {
                        return StatusCode(409);
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpGet(template: "GetStorage")]
        public IActionResult GetStorage(string StorageName)
        {
            try
            {
                using (_productContext)
                {

                    return Ok(_productContext.Storages.FirstOrDefault(e => e.Name.Equals(StorageName)));

                }
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpPatch(template: "UpdateStorage")]
        public IActionResult UpdateStorage(string StorageName, string description)
        {
            try
            {
                using (_productContext)
                {
                    var entity = _productContext.Storages.FirstOrDefault(e => e.Name.Equals(StorageName));

                    if (entity == null)
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        entity.Name = StorageName;
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

        [HttpDelete(template: "DeleteStorage")]
        public IActionResult DeleteStorage(string StorageName)
        {
            try
            {


                using (_productContext)
                {
                    var entity = _productContext.Storages.FirstOrDefault(x => x.Name == StorageName);
                    if (entity == null)
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        _productContext.Storages.Remove(entity);
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
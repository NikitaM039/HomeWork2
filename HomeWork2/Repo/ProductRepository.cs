using AutoMapper;
using HomeWork2.Abstraction;
using HomeWork2.DB;
using HomeWork2.Models;
using HomeWork2.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace HomeWork2.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private IMemoryCache _memoryCache;
        private ProductContext _productContext;

        public ProductRepository(IMapper mapper, IMemoryCache memoryCache, ProductContext productContext)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productContext = productContext;
        }

        public int AddGroup(GroupDto group)
        {

            using (_productContext)
            {
                var entityGroup = _productContext.Categories.FirstOrDefault(x => x.Name.ToLower() == group.Name.ToLower());
                if (entityGroup == null)
                {
                    entityGroup = _mapper.Map<Category>(group);
                    _productContext.Categories.Add(entityGroup);
                    _productContext.SaveChanges();
                    _memoryCache.Remove("groups");
                }
                return entityGroup.Id;
            }
        }

        public int AddProduct(ProductDto product)
        {
            using (_productContext)
            {
                var entityProduct = _productContext.Products.FirstOrDefault(x => x.Name.ToLower() == product.Name.ToLower());
                if (entityProduct == null)
                {
                    entityProduct = _mapper.Map<Product>(product);
                    _productContext.Products.Add(entityProduct);
                    _productContext.SaveChanges();
                    _memoryCache.Remove("products");

                }
                return entityProduct.Id;
            }
        }

        public IEnumerable<GroupDto> GetGroups()
        {
            if (_memoryCache.TryGetValue("groups", out List<GroupDto> groupss))
            {
                return groupss;
            }


            using (_productContext)
            {
                var groups = _productContext.Categories.Select(x => _mapper.Map<GroupDto>(x)).ToList();
                _memoryCache.Set("groups", groups, TimeSpan.FromMinutes(30));
                return groups;
            }
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            if (_memoryCache.TryGetValue("products", out List<ProductDto> productss))
            {
                return productss;
            }


            using (_productContext)
            {
                var products = _productContext.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
                _memoryCache.Set("products", products, TimeSpan.FromMinutes(30));
                return products;
            }
        }
    }
}
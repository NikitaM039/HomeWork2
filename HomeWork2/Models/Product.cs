using HomeWork2.Models;

namespace HomeWork2.Models
{
    public class Product : BaseModel
    {
        public int Cost { get; set; }

        //public int CategoryId { get; set; }
        //public virtual Category? Category { get; set; }

        public virtual List<ProductStorage> ProductStorage { get; set; } = new List<ProductStorage>();
    }
}
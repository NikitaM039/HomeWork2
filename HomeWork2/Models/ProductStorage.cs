namespace HomeWork2.Models
{
    public class ProductStorage : BaseModel
    {
        public int? ProductId { get; set; }
        public int? StorageId { get; set; }
        public Product? Product { get; set; }
        public Storage? Storage { get; set; }

        public string? ProductName { get; set; }
    }
}
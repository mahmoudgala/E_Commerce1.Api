namespace ECommerce.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int TopTraffic { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string MainImg { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Rate { get; set; }
        public bool Status { get; set; }

        public Cate? Cate { get; set; }
        public Brand? Brand { get; set; }
        public int CateId { get; set; }
        public int BrandId { get; set; }
    }
}

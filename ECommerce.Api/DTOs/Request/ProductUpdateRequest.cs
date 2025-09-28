using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.DTOs.Request
{
    public class ProductUpdateRequest
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]

        public IFormFile? MainImg { get; set; } = null!;
        [Required]

        public int Quantity { get; set; }
        [Required]

        public double Price { get; set; }

        public double Discount { get; set; }
        public bool Status { get; set; }
        [Required]

        public int CateId { get; set; }
        [Required]

        public int BrandId { get; set; }
    }
}

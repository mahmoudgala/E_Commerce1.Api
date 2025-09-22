using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string BrandName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

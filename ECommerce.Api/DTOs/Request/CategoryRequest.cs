using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.DTOs.Request
{
    public class CategoryRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string CatName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}

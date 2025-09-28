using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.DTOs.Request
{
    public class LoginRequest
    {
        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}

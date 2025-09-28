using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.DTOs.Request
{
    public class ForgetPasswordRequest
    {

        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.DTOs.Request
{
    public class ForgetPasswordDTO
    {

        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}

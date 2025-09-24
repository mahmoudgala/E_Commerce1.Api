namespace ECommerce.Api.DTOs.Request
{
    public class ResetPasswordDTO
    {
        public string OTPCode { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
    }
}

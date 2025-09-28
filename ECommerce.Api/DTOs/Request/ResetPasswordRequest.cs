namespace ECommerce.Api.DTOs.Request
{
    public class ResetPasswordRequest
    {
        public string OTPCode { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
    }
}

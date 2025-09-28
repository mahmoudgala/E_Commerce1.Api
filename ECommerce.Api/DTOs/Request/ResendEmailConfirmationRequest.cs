namespace ECommerce.Api.DTOs.Request
{
    public class ResendEmailConfirmationRequest
    {
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}

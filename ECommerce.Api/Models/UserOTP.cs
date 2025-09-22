namespace ECommerce.Api.Models
{
    public class UserOTP
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string OTPCode { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}

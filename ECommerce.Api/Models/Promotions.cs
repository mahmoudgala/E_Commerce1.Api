namespace ECommerce.Api.Models
{
    public class Promotions
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Discount { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Status { get; set; }
        public int TotalUsed { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

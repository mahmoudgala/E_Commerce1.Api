using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.DTOs.Response
{
    public class UpdatePersonalInfoResponse
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]

        public string FirstName { get; set; } = string.Empty ;
        [Required]

        public string SecondName { get; set; } = string.Empty;
        [Required, DataType(DataType.EmailAddress)]

        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Address { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.DTOs.Response
{
    public class UsersResponse
    {
        public string Id { get; set; }
        public string Name { get; set; } 

        public string FirstName { get; set; } 
        public string SecondName { get; set; } 
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Img { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
    }
}

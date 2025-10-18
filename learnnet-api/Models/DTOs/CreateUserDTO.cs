using System.ComponentModel.DataAnnotations;

namespace learnnet_api.Models.DTOs
{
    public class CreateUserDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public required string Email { get; set; }
        public string? Phone { get; set; }
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public required string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClothes.RequestsModels
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string City { get; set; }
        //public bool IsCompany { get; set; }
        public string Voivodeship { get; set; }
    }
}

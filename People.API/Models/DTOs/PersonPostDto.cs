using System.ComponentModel.DataAnnotations;

namespace People.API.Models.DTOs
{
    public class PersonPostDto
    {
        [StringLength(50, ErrorMessage = "Firstname is too long")]
        public string Firstname { get; set; }

        [StringLength(50, ErrorMessage = "Lastname is too long")]
        public string Lastname { get; set; }

        [StringLength(20, ErrorMessage = "Phone is too loong")]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessage = "Email is too long")]
        public string Email { get; set; }
    }
}
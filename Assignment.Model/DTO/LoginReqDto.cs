
using System.ComponentModel.DataAnnotations;
namespace Assignment.Model.DTO
{
    public class LoginReqDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

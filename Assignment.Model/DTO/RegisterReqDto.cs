
using System.ComponentModel.DataAnnotations;


namespace Assignment.Model.DTO
{
    public  class RegisterReqDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

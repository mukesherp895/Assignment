using System.ComponentModel.DataAnnotations;

namespace Assignment.Model.DTO
{
    public class CompanyInfoReqDto
    {
        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model.DTO
{
    public class CompanyInfoReqDto
    {
        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model.DTO
{
    public class ProductReqDto
    {
        [Required]
        public int CompanyInfoId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}

﻿
using System.ComponentModel.DataAnnotations;


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

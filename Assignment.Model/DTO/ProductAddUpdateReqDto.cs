
using System.ComponentModel.DataAnnotations;

namespace Assignment.Model.DTO
{
    public class ProductAddUpdateReqDto
    {
        public int Id { get; set; }
        public int CompanyInfoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}

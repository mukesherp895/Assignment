using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model.DTO
{
    public class ProductsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CompanyInfoId { get; set; }
        public string CompanyName { get; set; }
    }
}

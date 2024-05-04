using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model.DTO
{
    public class CommonListDto
    {
        public long RowNum { get; set; }
        public int RecCount { get; set; }
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDateTime { get; set;}
    }
}

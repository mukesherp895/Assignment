using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model.DTO
{
    public class DataTableParamDto
    {
        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public string? sSearch { get; set; }
        public int sEcho { get; set; }
        public string? sSortDir_0 { get; set; }
        public int iSortCol_0 { get; set; }
        public string? Schema { get; set; }
    }
}

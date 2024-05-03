using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model.DTO
{
    public class ResponseDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}

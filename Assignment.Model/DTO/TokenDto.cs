using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model.DTO
{
    public class TokenDto
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
    }
}

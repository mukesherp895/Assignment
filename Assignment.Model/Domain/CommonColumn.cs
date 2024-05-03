using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model.Domain
{
    public class CommonColumn
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string CreatedBy { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime CreatedDateTime { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string? UpdatedBy { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}

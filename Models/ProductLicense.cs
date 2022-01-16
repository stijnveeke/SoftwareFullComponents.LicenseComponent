using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareFullComponents.LicenseComponent.Models
{
    public class qProductLicense
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "int")]
        public string LicenseId { get; set; }
        [Column(TypeName = "int")]
        public string ProductId { get; set; }
    }
}

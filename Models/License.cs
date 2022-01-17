using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareFullComponents.LicenseComponent.Models
{
    public class License
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string LicenseKey { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string UserIdentifier { get; set; }
        
        public Guid ProductId { get; set; }
        
        public int TimesActivated { get; set; }
        
        public int ActivateableAmount { get; set; }
    }
}

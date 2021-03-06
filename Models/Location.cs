using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class Location : Audit
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public int UMK { set; get; }
        [Required]
        public int DistrictId { set; get; }
        public District District { set; get; }        
    }
}

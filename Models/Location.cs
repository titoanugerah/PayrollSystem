using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class Location : Audit
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public int DistrictId { set; get; }
        public District District { set; get; }
    }
}

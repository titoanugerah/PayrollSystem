using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class District : Audit
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public int UMK { set; get; }
        public string Remark { set; get; }
    }
}

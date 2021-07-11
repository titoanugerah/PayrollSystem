using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class FamilyStatus : Audit
    {
        [Key]
        [MaxLength(2)]
        public string Code { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public int PTKP { set; get; }
        public string Remark { set; get; }
    }
}

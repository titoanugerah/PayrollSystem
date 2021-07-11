using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class Bank : Audit
    {
        [Required]
        [MaxLength(4)]
        public string Code { set; get; }
        [Required]
        public string Name { set; get; }

    }
}

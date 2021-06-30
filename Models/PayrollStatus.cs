using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class PayrollStatus
    {
        [Required]
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string Remark { set; get; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class Customer : Audit
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string Remark { set; get; }     
        [DefaultValue(1)]
        public int MainCustomerId { set; get; }
        public MainCustomer MainCustomer { set; get; }
       
    }
}

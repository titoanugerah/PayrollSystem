using Payroll.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class Customer : Audit
    {
 //       public Customer(CreateCustomer createCustomer)
 //       {
 //           Name = createCustomer.Name;
 //           Remark = createCustomer.Remark;
 ////           CreateBy = User.Claims.Where(x => x.Type == type).Select(x => x.Value).FirstOrDefault();
 //       }
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string Remark { set; get; }
    }
}

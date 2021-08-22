using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class Employee : Audit
    {
        [Key]
        public int Id { set; get; }
        [MaxLength(4)]
        public int PrimaryNIK { set; get; }
        [MaxLength(16)]
        public string SecondaryNIK { set; get; }
        [Required]
        public string Password { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public int MainCustomerId { set; get; }
        public MainCustomer MainCustomer { set; get; }
        [DefaultValue(0)]
        public int BpjsStatusId { set; get; }
        [NotMapped]
        public string BpjsStatus
        {
            get
            {
                string result = null;
                if (BpjsStatusId != null)
                {
                    switch (BpjsStatusId)
                    {
                        case 0:
                            result = "Tidak ada";
                            break;
                        case 1:
                            result = "BU MBK";
                            break;
                        case 2:
                            result = "Lainnya";
                            break;
                    }
                }
                return result;
            }

        }
        public string PhoneNumber { set; get; }
        [MaxLength(16)]
        public string FamilyStatusCode { set; get; }
        public FamilyStatus FamilyStatus { set; get; }
        [MaxLength(14)]
        public string BankCode { set; get; }
        public Bank Bank { set; get; }
        public string AccountNumber { set; get; }
        public string AccountName { set; get; }
        [Required]
        public int PositionId { set; get; }
        public Position Position { set; get; }
        public int LocationId { set; get; }
        public Location Location { set; get; }
        public int CustomerId { set; get; }
        public Customer Customer { set; get; }
        [DefaultValue(2)]
        public int RoleId { set; get; }
        public Role Role { set; get; }
    }
}

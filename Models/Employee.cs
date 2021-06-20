using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class Employee : Audit
    {
        [Key]
        [MaxLength(5)]
        public int NIK { set; get; }
        [Required]
        public string Name { set; get; }
        [MaxLength(1)]
        [DefaultValue("L")]
        public char Sex { set; get; }
        [Required]
        public string BirthPlace { set; get; }
        [Required]
        public DateTime BirthDate { set; get; }
        [Required]
        public string Religion { set; get; }
        [Required]
        public string Address { set; get; }
        [Required]
        public string PhoneNumber { set; get; }
        [Required]
        [MaxLength(16)]
        public string KTP { set; get; }
        [Required]
        [MaxLength(16)]
        public string KK { set; get; }
        [NotMapped]
        public bool HasDriverLicense
        {
            get
            {
                if (DriverLicense == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [Required]
        public string Email { set; get; }
        [DefaultValue("user.jpg")]
        [Required]
        public string Image { set; get; }
        public string DriverLicense { set; get; }
        public DateTime DriverLicenseExpire { set; get; }
        [DefaultValue("L")]
        [Required]
        public string FamilyStatusCode { set; get; }
        public FamilyStatus FamilyStatus { set; get; }
        [Required]
        [MaxLength(14)]
        public string BpjsNumber { set; get; }
        public string BpjsRemark { set; get; }
        [Required]
        [MaxLength(12)]
        public string JamsostekNumber { set; get; }
        public string JamsostekRemark { set; get; }
        public string NPWP { set; get; }


        [Required]
        public DateTime JoinCompanyDate { set; get; }
        [Required]
        public DateTime StartContract { set; get; }
        [Required]
        public DateTime EndContract { set; get; }

        [Required]
        public string BankCode { set; get; }
        public Bank Bank { set; get; }
        [Required]
        public string AccountNumber { set; get; }
        [Required]
        public string AccountName { set; get; }
        [Required]
        public int EmploymentStatusId { set; get; }
        public EmploymentStatus EmploymentStatus { set; get; }

        [Column(TypeName = "bit")]
        [DefaultValue(false)]
        public bool HasUniform { set; get; }
        [Column(TypeName = "bit")]
        [DefaultValue(false)]
        public bool HasIdCard { set; get; }
        [Column(TypeName = "bit")]
        [DefaultValue(false)]
        public bool HasTraining { set; get; }

        [Required]
        public int PositionId { set; get; }
        public Position Position { set; get; }
        [Required]
        public int LocationId { set; get; }
        public Location Location { set; get; }
        [Required]
        public int CustomerId { set; get; }
        public Customer Customer { set; get; }
        [Required]
        public DateTime JoinCustomerDate { set; get; }
        [Required]        
        public int RoleId { set; get; }
        public Role Role { set; get; }

    }
}

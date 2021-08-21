using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class Employee : Audit
    {
        [Key]
        [MaxLength(16)]
        public string NIK { set; get; }
        [Required]
        public string Password { set; get; }
        [Required]
        public string Name { set; get; }
        //[MaxLength(1)]
        //[DefaultValue("L")]
        //public string Sex { set; get; }
        //public string BirthPlace { set; get; }
        //public DateTime BirthDate { set; get; }
        //public string Religion { set; get; }
        //public string Address { set; get; }
        public int MainCustomerId { set; get; }
        public MainCustomer MainCustomer { set; get; }
        public string PhoneNumber { set; get; }
        [MaxLength(16)]
        public string KTP { set; get; }
        //[MaxLength(16)]
        //public string KK { set; get; }
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

        public string DriverLicenseType { set; get; }
        public string DriverLicense { set; get; }
        //public DateTime DriverLicenseExpire { set; get; }
        //[DefaultValue("L")]
        public string FamilyStatusCode { set; get; }
        public FamilyStatus FamilyStatus { set; get; }
        [MaxLength(14)]
        public string BpjsNumber { set; get; }
        public string BpjsRemark { set; get; }
        [MaxLength(12)]
        public string JamsostekNumber { set; get; }
        public string JamsostekRemark { set; get; }
        public string NPWP { set; get; }

        //public DateTime JoinCompanyDate { set; get; }
        //public DateTime StartContract { set; get; }
        //public DateTime EndContract { set; get; }

        public string BankCode { set; get; }
        public Bank Bank { set; get; }
        public string AccountNumber { set; get; }
        public string AccountName { set; get; }
        //public int EmploymentStatusId { set; get; }
        //public EmploymentStatus EmploymentStatus { set; get; }

        //[Column(TypeName = "bit")]
        //[DefaultValue(false)]
        //public bool HasUniform { set; get; }
        //public DateTime UniformDeliveryDate { set; get; }
        //[Column(TypeName = "bit")]
        //[DefaultValue(false)]
        //public bool HasIdCard { set; get; }
        //public DateTime IdCardDeliveryDate { set; get; }
        //[Column(TypeName = "bit")]
        //[DefaultValue(false)]
        //public bool HasTraining { set; get; }
        //public string TrainingName { set; get; }
        //public string TrainingRemark { set; get; }
        //public string TrainingGrade { set; get; }
        //public DateTime TrainingDeliveryDate { set; get; }

        [Required]
        public int PositionId { set; get; }
        public Position Position { set; get; }
        public int LocationId { set; get; }
        public Location Location { set; get; }
        public int CustomerId { set; get; }
        public Customer Customer { set; get; }
        //public DateTime JoinCustomerDate { set; get; }
        [DefaultValue(2)]
        public int RoleId { set; get; }
        public Role Role { set; get; }
    }
}

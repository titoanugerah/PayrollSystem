using System;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class Employee : Audit
    {
        [Key]
        [MaxLength(5)]
        public int NIK { set; get; }
        [Required]
        public string Name { set; get; }
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
        public string FamilyStatusCode { set; get; }
        public FamilyStatus FamilyStatus { set; get; }



        [Required]
        public int PositionId { set; get; }
        public Position Position { set; get; }
        [Required]
        public int LocationId { set; get; }
        public Location Location { set; get; }
        [Required]
        public int CustomerId { set; get; }
        public Customer Customer { set; get; }
        public DateTime JoinCustomerDate { set; get; }
        

    }
}

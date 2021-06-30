using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class PayrollDetail
    {
        public int Id { set; get; }
        [Required]
        public int PayrollHistoryId { set; get; }
        public PayrollHistory PayrollHistory { set; get; }
        [Required]
        public int EmployeeId { set; get; }
        public Employee Employee { set; get; }
        [Required]
        public int MainSalaryBilling { set; get; }
        [DefaultValue(0)]
        public int JamsostekBilling { set; get; }
        [DefaultValue(0)]
        public int BpjsBilling { set; get; }
        [DefaultValue(0)]
        public int PensionBilling { set; get; }
        [DefaultValue(0)]
        public int AtributeBilling { set; get; }
        [DefaultValue(0)]
        public int MainPrice { set; get; }
        [DefaultValue(0)]
        public int ManagementFeeBilling { set; get; }
        [DefaultValue(0)]
        public int InsentiveBilling { set; get; }
        [DefaultValue(0)]
        public int AttendanceBilling { set; get; }
        [DefaultValue(0)]
        public int OvertimeBilling { set; get; }
        [DefaultValue(0)]
        public int SubtotalBilling { set; get; }
        [DefaultValue(0)]
        public int TaxBilling { set; get; }
        [DefaultValue(0)]
        public int GrandTotalBilling { set; get; }
        [DefaultValue(0)]
        public int ResultPayroll { set; get; }
        [DefaultValue(0)]
        public int FeePayroll { set; get; }
        [DefaultValue(0)]
        public int TotalPayroll { set; get; }
        [DefaultValue(0)]
        public int TaxPayroll { set; get; }
        [DefaultValue(0)]
        public int GrossPayroll { set; get; }
        [DefaultValue(0)]
        public int AttributePayroll { set; get; }
        [DefaultValue(0)]
        public int BpjsTkDeduction { set; get; }
        [DefaultValue(0)]
        public int BpjsKesehatanDeduction{ set; get; }
        [DefaultValue(0)]
        public int PensionDeduction{ set; get; }
        [DefaultValue(0)]
        public int PKP1{ set; get; }
        [DefaultValue(0)]
        public int PTKP{ set; get; }
        [DefaultValue(0)]
        public int PKP2{ set; get; }
        [DefaultValue(0)]
        public int PPH21{ set; get; }
        [DefaultValue(0)]
        public int PPH23{ set; get; }
        [DefaultValue(0)]
        public int Netto{ set; get; }
        [DefaultValue(0)]
        public int AnotherDeduction{ set; get; }
        [DefaultValue(0)]
        public string AnotherDeductionRemark{ set; get; }
    }
}

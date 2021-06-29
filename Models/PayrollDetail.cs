namespace Payroll.Models
{
    public class PayrollDetail
    {
        public int Id { set; get; }
        public int PayrollId { set; get; }
        public Payroll Payroll { set; get; }
        public int EmployeeId { set; get; }
        public Employee Employee { set; get; }
        public int MainSalaryBilling { set; get; }
        public int JamsostekBilling { set; get; }
        public int BpjsBilling { set; get; }
        public int PensionBilling { set; get; }
        public int AtributeBilling { set; get; }
        public int MainPrice { set; get; }
        public int ManagementFeeBilling { set; get; }
        public int InsentiveBilling { set; get; }
        public int AttendanceBilling { set; get; }
        public int OvertimeBilling { set; get; }
        public int SubtotalBilling { set; get; }
        public int TaxBilling { set; get; }
        public int GrandTotalBilling { set; get; }
        public int ResultPayroll { set; get; }
        public int FeePayroll { set; get; }
        public int TotalPayroll { set; get; }
        public int TaxPayroll { set; get; }
        public int GrossPayroll { set; get; }
        public int AttributePayroll { set; get; }
        public int BpjsTkDeduction { set; get; }
        public int BpjsKesehatanDeduction{ set; get; }
        public int PensionDeduction{ set; get; }
        public int PKP1{ set; get; }
        public int PTKP{ set; get; }
        public int PKP2{ set; get; }
        public int PPH21{ set; get; }
        public int PPH23{ set; get; }
        public int Netto{ set; get; }
        public int AnotherDeduction{ set; get; }
        public string AnotherDeductionRemark{ set; get; }
    }
}

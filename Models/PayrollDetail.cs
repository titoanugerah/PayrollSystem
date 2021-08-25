using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class PayrollDetail : Audit
    {
        public int Id { set; get; }
        [Required]
        public int PayrollHistoryId { set; get; }
        public PayrollHistory PayrollHistory { set; get; }
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { set; get; }
        public Employee Employee { set; get; }
        [DefaultValue(0)]
        public int AbsentDeduction { set; get; }
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
        public int TrainingBilling { set; get; }
        [DefaultValue(0)]
        public int RouteBilling { set; get; }
        [DefaultValue(0)]
        public int Thr { set; get; }
        [DefaultValue(0)]
        public int Rapel { set; get; }
        [DefaultValue(0)]
        public int BpjsReturn { set; get; }
        [DefaultValue(0)]
        public int AppreciationBilling { set; get; }
        [DefaultValue(0)]
        public int TransferFee { set; get; }
        [Column(TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsLateTransfer { set; get; }
        [DefaultValue(0)]
        public int MainPrice { set; get; }
        
        [DefaultValue(0)]
        public int ManagementFeeBilling { set; get; }
        
        [DefaultValue(0)]
        public int InsentiveBilling { set; get; }
        [DefaultValue(0)]
        public int PositionInsentiveBilling { set; get; }
        [DefaultValue(0)]
        public int AnotherInsentive { set; get; }
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
        [NotMapped]
        public int SuspectedGrandTotalBilling
        {
            get
            {
                return SubtotalBilling + TaxBilling;
            }
        }
        [NotMapped]
        [DefaultValue(false)]
        public bool IsValidGrandTotalBilling
        {
            get
            {
                if (GrandTotalBilling == SuspectedGrandTotalBilling)
                {
                    return true;
                }
                else if (GrandTotalBilling == SuspectedGrandTotalBilling +1)
                {
                    return true;
                }
                else if (GrandTotalBilling == SuspectedGrandTotalBilling -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
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
        public int BpjsKesehatanDeduction { set; get; }
        [DefaultValue(0)]
        public int PensionDeduction { set; get; }
        [DefaultValue(0)]
        public int PKP1 { set; get; }
        [DefaultValue(0)]
        public int PTKP { set; get; }
        [DefaultValue(0)]
        public int PKP2 { set; get; }
        [DefaultValue(0)]
        public int PPH21 { set; get; }
        [DefaultValue(0)]
        public int PPH23 { set; get; }
        [DefaultValue(0)]
        public int Netto { set; get; }
        [DefaultValue(0)]
        public int AnotherDeduction { set; get; }
        public int PulseAllowance { set; get; }
        [DefaultValue(0)]
        public string AnotherDeductionRemark{ set; get; }
        [DefaultValue(0)]
        public int TakeHomePay { set; get; }
        public int PayrollDetailStatusId { set; get; }
        [NotMapped]
        public string PayrollDetailStatus            
        {            
            get
            {
                string result = null;
                switch (PayrollDetailStatusId)
                {
                    case 1:
                        result = "Belum Upload";
                        break;
                    case 2:
                        result = "Sudah Upload";
                        break;
                    case 3:
                        result = "Gaji Dikirim";
                        break;
                    default:
                        result = null;
                        break;
                }
                return result;
            }
        }

    }
}

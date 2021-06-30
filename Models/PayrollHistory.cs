using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class PayrollHistory : Audit
    {
        public int Id { set; get; }
        [Required]
        public string Month { set; get; }
        [Required]
        public string Year { set; get; }
        [DefaultValue(1)]        
        public int StatusId { set; get; }
        [Required]
        public int JamsostekPercentage { set; get; }
        [Required]
        public int BpjsPercentage { set; get; }
        [Required]
        public int PensionPercentage { set; get; }
        [Required]
        public int ManagementFeePercentage { set; get; }
        [Required]
        public int PpnPercentage { set; get; }
        [Required]
        public int BpjsTk1Percentage { set; get; }
        [Required]
        public int BpjsKesehatanPercentage { set; get; }
        [Required]
        public int Pension1Percentage { set; get; }
        [Required]
        public int Pph21Percentage { set; get; }
        [Required]
        public int Pph23Percentage { set; get; }
    }
}

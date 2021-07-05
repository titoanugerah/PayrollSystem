using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class PayrollHistory : Audit
    {
        public int Id { set; get; }
        [Required]
        public string Month { set; get; }
        [Required]
        public string Year { set; get; }
        public int StatusId { set; get; }
        [NotMapped]
        public string Status {
            get
            {
                string result = null;
                switch (StatusId)
                {
                    case 1:
                        result = "Pendataan gaji belum diproses";
                        break;
                    case 2:
                        result = "Pendataan gaji sedang diproses";
                        break;
                    case 3:
                        result = "Pendataan gaji selesai diproses";
                        break;
                    case 4:
                        result = "Selesai";
                        break;
                    default:
                        result = null;
                        break;
                }
                return result;
            }
        }
        [Required]
        public decimal JamsostekPercentage { set; get; }
        [Required]
        public decimal BpjsPercentage { set; get; }
        [Required]
        public decimal PensionPercentage { set; get; }
        [Required]
        public decimal ManagementFeePercentage { set; get; }
        [Required]
        public decimal PpnPercentage { set; get; }
        [Required]
        public decimal BpjsTk1Percentage { set; get; }
        [Required]
        public decimal BpjsKesehatanPercentage { set; get; }
        [Required]
        public decimal Pension1Percentage { set; get; }
        [Required]
        public decimal Pph21Percentage { set; get; }
        [Required]
        public decimal Pph23Percentage { set; get; }
    }
}

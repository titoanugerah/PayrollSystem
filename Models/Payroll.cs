namespace Payroll.Models
{
    public class Payroll : Audit
    {
        public int Id { set; get; }
        public string Month { set; get; }
        public string Year { set; get; }
        public int StatusId { set; get; }
        public int JamsostekPercentage { set; get; }
        public int BpjsPercentage { set; get; }
        public int PensionPercentage { set; get; }
        public int ManagementFeePercentage { set; get; }
        public int PpnPercentage { set; get; }
        public int BpjsTk1Percentage { set; get; }
        public int BpjsKesehatanPercentage { set; get; }
        public int Pension1Percentage { set; get; }
        public int Pph21Percentage { set; get; }
        public int Pph23Percentage { set; get; }
    }
}

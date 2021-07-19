using System;

namespace Payroll.ViewModels
{
    public class EmployeeInput
    {
        public string Name { set; get; }
        public string BirthPlace { set; get; }
        public DateTime BirthDate { set; get; }
        public string Sex { set; get; }
        public string Religion { set; get; }
        public string Address { set; get; }
        public string PhoneNumber { set; get; }
        public string KTP { set; get; }
        public string KK { set; get; }
        public string NPWP { set; get; }
        public string JamsostekNumber { set; get; }
        public string JamsostekRemark { set; get; }
        public string BpjsNumber { set; get; }
        public string BpjsRemark { set; get; }
        public string DriverLicense { set; get; }
        public string DriverLicenseType { set; get; }
        public DateTime DriverLicenseExpire { set; get; }
        public string AccountNumber { set; get; }
        public string AccountName { set; get; }
        public string BankCode { set; get; }
        public string FamilyStatusCode { set; get; }
        public int EmploymentStatusId { set; get; }
        public int PositionId { set; get; }
        public int CustomerId { set; get; }
        public int LocationId { set; get; }
        public int RoleId { set; get; }
        public DateTime StartContract { set; get; }
        public DateTime EndContract { set; get; }
        public DateTime JoinCompanyDate { set; get; }
        public DateTime JoinCustomerDate { set; get; }
        public bool HasUniform { set; get; }
        public DateTime UniformDeliveryDate { set; get; }
        public bool HasIdCard { set; get; }
        public DateTime IdCardDeliveryDate { set; get; }
        public bool HasTraining { set; get; }
        public DateTime TrainingDeliveryDate { set; get; }
        public string TrainingName { set; get; }
        public string TrainingRemark { set; get; }
        public string TrainingGrade { set; get; }
        public bool IsExist { set; get; }
    }
}

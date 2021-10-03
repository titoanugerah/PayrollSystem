using System;

namespace Payroll.ViewModels
{
    public class EmployeeInput
    {
        public string Name { set; get; }
        public string NIK { set; get; }
        public string PhoneNumber { set; get; }
        public string AccountNumber { set; get; }
        public string AccountName { set; get; }
        public string BankCode { set; get; }
        public string FamilyStatusCode { set; get; }
        public int BpjsStatusId { set; get; }
        public int PositionId { set; get; }
        public int CustomerId { set; get; }
        public int LocationId { set; get; }
        public int RoleId { set; get; }
        public bool IsExist { set; get; }
    }
}

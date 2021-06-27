using Payroll.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Payroll.ViewModels
{
    public class EmployeeView
    {
        public List<Employee> Data { set; get; }
        public int RecordsTotal
        {
            get
            {
                return Data.Count();
            }
        }
        public int RecordsFiltered
        {
            get
            {
                return Data.Count();
            }
        }
        [DefaultValue(1)]
        public int Draw { set; get; }
    }

    //public class Employee
    //{
    //    public int NIK { set; get; }
    //    public string Name { set; get; }
    //    public string Position { set; get; }
    //    public string Location { set; get; }
    //    public string Customer { set; get; }

    //}
}

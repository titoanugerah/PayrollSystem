using Payroll.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Payroll.ViewModels
{
    public class PayrollHistoryView
    {
        public List<PayrollHistory> Data { set; get; }
        public int RecordsFiltered { set; get; }
        public int RecordsTotal
        {
            get
            {
                return Data.Count();
            }
        }
        [DefaultValue(1)]
        public int Draw { set; get; }
    }
}

using System.ComponentModel;

namespace Payroll.ViewModels
{
    public class BoolResult
    {
        [DefaultValue(false)]
        public bool IsSuccess { set; get; }
        public bool? Value { set; get; }
    }
}

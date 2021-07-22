using System.ComponentModel;

namespace Payroll.ViewModels
{
    public class StringResult
    {
        [DefaultValue(false)]
        public bool IsSuccess { set; get; }
        public string? Value { set; get; }
    }
}

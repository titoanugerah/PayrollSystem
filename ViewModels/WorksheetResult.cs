using OfficeOpenXml;

namespace Payroll.ViewModels
{
    public class WorksheetResult
    {
        public ExcelWorksheet Worksheet { set; get; }
        public bool IsError { set; get; }
        public string ErrorMessage { set; get; }
        public int LastRow { set; get; }

    }
}

using OfficeOpenXml;

namespace Payroll.ViewModels
{
    public class ExcelIntreader
    {
        public ExcelIntreader(ExcelWorksheet worksheet, string cell)
        {
            ExcelStringReader stringReader = new ExcelStringReader(worksheet, cell);
            IsExist = stringReader.IsExist;
            if (IsExist)
            {
                int value = 0;
                IsInteger = int.TryParse(stringReader.Value,  out value);
                Value = value;
            }
        }
        public bool IsExist { set; get; }
        public bool IsInteger { set; get; }
        public int Value { set; get; }
    }

}

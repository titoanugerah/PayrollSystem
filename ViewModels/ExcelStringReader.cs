using OfficeOpenXml;

namespace Payroll.ViewModels
{
    public class ExcelStringReader
    {
        public ExcelStringReader(ExcelWorksheet worksheet, string cell)
        {
            ExcelRange selectedCell = worksheet.Cells[$"{cell}"];
            IsExist = selectedCell.Value != null;
            if (IsExist)
            {
                Value = selectedCell.Value.ToString();
                ValueMerged = selectedCell.Value.ToString().ToLower().Replace(" ", string.Empty);
            }
            
        }
        public bool IsExist { set; get; }
        public string Value { set; get; }
        public string ValueMerged { set; get; }
    }
}

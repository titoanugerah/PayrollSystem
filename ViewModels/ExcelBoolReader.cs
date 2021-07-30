using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace Payroll.ViewModels
{
    public class ExcelBoolReader
    {
        public ExcelBoolReader(ExcelWorksheet worksheet, string cell, string valueStringIfTrue = null, string valueStringIfFalse = null)
        {
            ExcelRange selectedCell = worksheet.Cells[$"{cell}"];
            ExcelStringReader stringReader = new ExcelStringReader(worksheet, cell);
            IsExist = stringReader.IsExist;

            if (IsExist && valueStringIfTrue != null)
            {
                ValueIfTrue = valueStringIfTrue.Split(";").ToList();
                foreach (string value in ValueIfTrue)
                {
                    if (stringReader.ValueMerged == value.ToLower())
                    {
                        Value = true;
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            if (IsExist && valueStringIfFalse != null)
            {
                ValueIfFalse = valueStringIfFalse.Split(";").ToList();
                foreach (string value in ValueIfFalse)
                {
                    if (stringReader.ValueMerged == value.ToLower())
                    {
                        Value = false;
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
            }




            
        }
        public bool IsExist { set; get; }
        public bool Value { set; get; }
        public List<string> ValueIfTrue { set; get; }
        public List<string> ValueIfFalse { set; get; }

    }
}

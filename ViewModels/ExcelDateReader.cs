using OfficeOpenXml;
using System;

namespace Payroll.ViewModels
{
    public class ExcelDateReader
    {
        public ExcelDateReader(ExcelWorksheet worksheet, string cell)
        {
            ExcelRange selectedCell = worksheet.Cells[$"{cell}"];
            IsValidDate = new ExcelBoolReader(worksheet, cell, null , "SUDAH;NONDRIVER;BELUM");
            if (IsValidDate.IsExist != false)
            {
                if (IsValidDate.Value)
                {
                    IsExist = true;
                    DateTime dateTimeString;
                    if (DateTime.TryParse(selectedCell.Value.ToString(), out dateTimeString))
                    {
                        Value = dateTimeString;
                    }
                    else
                    {
                        Value = DateTime.FromOADate(int.Parse(selectedCell.Value.ToString()));
                    }
                }
            }            
        }
        public bool IsExist { set; get; }
        public DateTime Value { set; get; }
        public ExcelBoolReader IsValidDate { set; get; }

    }
}

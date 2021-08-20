using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Payroll.ViewModels
{
    public class AddressTTNT
    {
        public AddressTTNT(ExcelWorksheet excelWorksheet)
        {
            Worksheet = excelWorksheet;
            HeaderRow = FindHeaderRow();
            if (HeaderRow != 0)
            {
                DataStartRow = HeaderRow + 1;
                NIK = FindHeaderColumn("nik");
                Name = FindHeaderColumn("nama;name");
                MainSalaryBilling = FindHeaderColumn("gajipokok");
                TrainingBilling = FindHeaderColumn("training");
                RouteBilling = FindHeaderColumn("rute");
                
                JamsostekBilling = FindHeaderColumn("jamsostek");
                BpjsBilling = FindHeaderColumn("bpjskesehatan");
                PensionBilling = FindHeaderColumn("jaminanpensiun");
                Thr = FindHeaderColumn("thr");
                InsentiveBilling = FindHeaderColumn("insentiflainnya");
                GrandTotalBilling = FindHeaderColumn("grandtotal");
                AnotherDeduction = FindHeaderColumn("potongan");

                IsValid = NIK != null && Name != null && MainSalaryBilling != null && JamsostekBilling != null && BpjsBilling != null && PensionBilling != null  && GrandTotalBilling != null;
                if (IsValid)
                {
                    DataEndRow = FindEndDataRow();
                }

            }        


        }

        public int FindHeaderRow()
        {
            int headerRow = 0;
            if (Worksheet.Dimension == null)
            {
                return headerRow;
            }

            for (int currentRow = Worksheet.Dimension.Start.Row; currentRow <= Worksheet.Dimension.End.Row; currentRow++)
            {
                ExcelRange selectedCell = Worksheet.Cells[$"A{currentRow}"];
                if (selectedCell.Style.Fill.BackgroundColor.Indexed == 8)
                {
                    headerRow = currentRow;
                    break;
                }
                else
                {
                    if (selectedCell.Value!=null)
                    {
                        if (GetStringValue(selectedCell.Value) == "no")
                        {
                            headerRow = currentRow;
                            break;
                        }
                    }
                }
            }
            return headerRow;
        }

        public int FindEndDataRow()
        {
            int DataRow = 0;
            for (int currentRow = DataStartRow; currentRow <= Worksheet.Dimension.End.Row; currentRow++)
            {
                ExcelRange selectedCell1 = Worksheet.Cells[$"{NIK}{currentRow}"];
                ExcelRange selectedCell2 = Worksheet.Cells[$"{Name}{currentRow}"];
                if (selectedCell1.Value == null && selectedCell2.Value == null)
                {
                    DataRow = currentRow;
                    break;
                }
            }
            return DataRow; 
        }

        public string FindHeaderColumn(string keyword)
        {
            string headerColumn = null;
            List<string> keywordLists = new List<string>();
            foreach (string keywordList in keyword.Split(";").ToList())
            {
                keywordLists.Add(GetStringValue(keywordList));
            }

            for (int currentColumn = Worksheet.Dimension.Start.Column;  currentColumn <= Worksheet.Dimension.End.Column; currentColumn++)
            {
                ExcelRange cell = Worksheet.Cells[HeaderRow, currentColumn];
                if (cell.Value != null)
                {
                    string cellValue = GetStringValue(cell.Value);
                    if (keywordLists.Contains(cellValue))
                    {
                        headerColumn = Regex.Replace(Worksheet.Cells[HeaderRow, currentColumn].Address, @"[\d-]", string.Empty);
                        break;
                    }
                }

            }
            return headerColumn;
        }

        public string GetStringValue(object value)
        {
            return new string(value.ToString().Where(char.IsLetter).ToArray()).ToLower();
        }

        public ExcelWorksheet Worksheet { set; get; }      
        public int HeaderRow { set; get; }
        public int DataStartRow { set; get; }
        public int DataEndRow { set; get; }
        public bool IsValid { set; get; }
        public string NIK { set; get; }      
        public string Name { set; get; }

        public string MainSalaryBilling { set; get; }
        public string TrainingBilling { set; get; }
        public string RouteBilling { set; get; }
        public string InsentiveBilling { set; get; }


        public string JamsostekBilling { set; get; }
        public string BpjsBilling { set; get; }
        public string PensionBilling { set; get; }
        public string Thr { set; get; }
       
        public bool IsAnyInsentiveBilling
        {
            get
            {
                if (InsentiveBilling != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string AnotherDeduction { set; get; }
        public bool IsAnyAnotherDeduction
        {
            get
            {
                if (AnotherDeduction != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public string GrandTotalBilling { set; get; }
    }
}

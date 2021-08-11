using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Payroll.ViewModels
{
    public class AddressPayroll
    {
        public AddressPayroll(ExcelWorksheet excelWorksheet)
        {
            Worksheet = excelWorksheet;
            HeaderRow = FindHeaderRow();
            if (HeaderRow != 0)
            {
                DataStartRow = HeaderRow + 1;
                //DataEndRow = FindDataRow();
                NIK = FindHeaderColumn("nik");
                Name = FindHeaderColumn("nama;name");
                MainSalaryBilling = FindHeaderColumn("umk;gajipokok");
                AbsentDeduction = FindHeaderColumn("potabsensi");
                JamsostekBilling = FindHeaderColumn("jamsostek4.89%;jamsostek");
                BpjsBilling = FindHeaderColumn("bpjskesehatan4%;bpjskesehatan");
                PensionBilling = FindHeaderColumn("jaminanpensiun2%;jaminanpensiun");
                AtributeBilling = FindHeaderColumn("perlengkapan");
                MainPrice = FindHeaderColumn("hargapokok");
                ManagementFeeBilling = FindHeaderColumn("managementfee");
                InsentiveBilling = FindHeaderColumn("insentif");
                AttendanceBilling = FindHeaderColumn("premihadir;premikehadiran");


            }        


        }

        public int FindHeaderRow()
        {
            int headerRow = 0;
            for (int currentRow = Worksheet.Dimension.Start.Row; currentRow <= Worksheet.Dimension.End.Row; currentRow++)
            {
                ExcelRange selectedCell = Worksheet.Cells[$"A{currentRow}"];
                if (selectedCell.Style.Fill.BackgroundColor.Indexed == 8)
                {
                    headerRow = currentRow;
                }
            }
            return headerRow;
        }

        //public int FindDataRow()
        //{
        //    int DataRow = 0;
        //    for (int currentRow = DataStartRow; currentRow <= Worksheet.Dimension.End.Row; currentRow++)
        //    {
        //        ExcelRange selectedCell = Worksheet.Cells[$"A{currentRow}"];
        //        if (selectedCell.Value == "")
        //        {

        //        }
        //    }
        //}

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

        public string NIK { set; get; }
        public string Name { set; get; }
        public string UMK { set; get; }
        public string AbsentDeduction { set; get; }
        public string MainSalaryBilling { set; get; }
        public string JamsostekBilling { set; get; }
        public string BpjsBilling { set; get; }
        public string PensionBilling { set; get; }
        public string AtributeBilling { set; get; }
        public string MainPrice { set; get; }
        public string AppreciationBilling { set; get; }
        public string ManagementFeeBilling { set; get; }
        public string InsentiveBilling { set; get; }
        public string AttendanceBilling { set; get; }
        public string OvertimeBilling { set; get; }
        public string SubtotalBilling { set; get; }
        public string TaxBilling { set; get; }
        public string GrandTotalBilling { set; get; }
        public string AnotherDeduction { set; get; }
    }
}

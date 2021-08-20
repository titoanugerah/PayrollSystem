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
                NIK = FindHeaderColumn("nik");
                Name = FindHeaderColumn("nama;name");
                MainSalaryBilling = FindHeaderColumn("umk;gajipokok");
                AbsentDeduction = FindHeaderColumn("potabsensi; potabsen");
                JamsostekBilling = FindHeaderColumn("jamsostek4.89%;jamsostek");
                BpjsBilling = FindHeaderColumn("bpjskesehatan4%;bpjskesehatan");
                PensionBilling = FindHeaderColumn("jaminanpensiun2%;jaminanpensiun");
                AtributeBilling = FindHeaderColumn("perlengkapan");
                MainPrice = FindHeaderColumn("hargapokok");
                ManagementFeeBilling = FindHeaderColumn("managementfee");
                InsentiveBilling = FindHeaderColumn("insentif");
                PulseAllowance = FindHeaderColumn("tunjpulsa");
                AttendanceBilling = FindHeaderColumn("premihadir;premikehadiran");
                AppreciationBilling = FindHeaderColumn("tjgp;apresiasi");
                OvertimeBilling = FindHeaderColumn("overtime");
                SubtotalBilling = FindHeaderColumn("subtotal");
                TaxBilling = FindHeaderColumn("ppn");
                GrandTotalBilling = FindHeaderColumn("grandtotal");
                AnotherDeduction = FindHeaderColumn("potongan");
                IsValid = NIK != null && Name != null && MainSalaryBilling != null && JamsostekBilling != null && BpjsBilling != null && PensionBilling != null && MainPrice != null && ManagementFeeBilling != null && SubtotalBilling != null && TaxBilling != null && GrandTotalBilling != null;
                if (IsValid)
                {
                    DataEndRow = FindEndDataRow();
                }

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
        public string AbsentDeduction { set; get; }
        public bool IsAnyAbsentDeduction
        {
            get
            {
                if (AbsentDeduction != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string MainSalaryBilling { set; get; }
        public string JamsostekBilling { set; get; }
        public string BpjsBilling { set; get; }
        public string PensionBilling { set; get; }
        public string AtributeBilling { set; get; }
        public string MainPrice { set; get; }
        public string AppreciationBilling { set; get; }
        public bool IsAnyAppreciationBilling
        {
            get
            {
                if (AppreciationBilling != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string ManagementFeeBilling { set; get; }
        public string InsentiveBilling { set; get; }
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
        public string AttendanceBilling { set; get; }
        public bool IsAnyAttendanceBilling
        {
            get
            {
                if (AttendanceBilling != null)
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
        public string PulseAllowance { set; get; }
        public bool IsAnyPulseAllowance
        {
            get
            {
                if (PulseAllowance != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string OvertimeBilling { set; get; }
        public bool IsAnyOvertimeBilling
        {
            get
            {
                if (OvertimeBilling != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string SubtotalBilling { set; get; }
        public string TaxBilling { set; get; }
        public string GrandTotalBilling { set; get; }
    }
}

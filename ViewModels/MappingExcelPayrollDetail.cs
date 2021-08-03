
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace Payroll.ViewModels
{
    public class MappingExcelPayrollDetail
    {
        public MappingExcelPayrollDetail(ExcelWorksheet worksheet, int initialRow = 1)
        {
            Worksheet = worksheet;
            if (Worksheet != null && Worksheet.Dimension != null)
            {
                StartCell = worksheet.Dimension.Start;
                EndCell = worksheet.Dimension.End;
                No = FindCell("NO", initialRow);
                InRowStart = FindRow(No, "1", initialRow);
                NIK = FindCell("Nik", initialRow);
                Name = FindCell("Name", initialRow);
                MainSalaryBilling = FindCell("GajiPokok", initialRow);
                JamsostekBilling = FindCell("Jamsostek4.89%", initialRow);
                BpjsBilling = FindCell("BPJSKesehatan4%", initialRow);
                PensionBilling = FindCell("JaminanPensiun2%", initialRow);
                AtributeBilling = FindCell("Perlengkapan", initialRow);
                MainPrice = FindCell("HargaPokok", initialRow);
                ManagementFeeBilling = FindCell("ManagementFee", initialRow);
                InsentiveBilling = FindCell("Insentif", initialRow);
                AttendanceBilling = FindCell("PremiKehadiran", initialRow);
                OvertimeBilling = FindCell("Overtime", initialRow);
                SubtotalBilling = FindCell("Subtotal", initialRow);
                TaxBilling = FindCell("PPN", initialRow);
                GrandTotalBilling = FindCell("GrandTotal", initialRow);
                GrandTotalBilling = FindCell("Apresiasi", initialRow);
                AnotherDeduction = FindCell("Potongan", initialRow);
                for (int currentRow = InRowStart; currentRow <= EndCell.Row + 1; currentRow++)
                {
                    ExcelRange selectedCell = Worksheet.Cells[$"{No}{currentRow}"];
                    if (selectedCell.Value == null)
                    {
                        InRowEnd = currentRow - 1;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (NIK != null && Name != null && MainSalaryBilling != null && JamsostekBilling != null && BpjsBilling != null && PensionBilling != null && AtributeBilling != null && MainPrice != null && ManagementFeeBilling != null && InsentiveBilling != null && AttendanceBilling != null && OvertimeBilling != null && SubtotalBilling != null && TaxBilling != null && GrandTotalBilling != null)
                {
                    IsAcceptable = true;
                }
                else
                {
                    IsAcceptable = false;

                }
            }
            else
            {
                IsAcceptable = false;

            }
        }
        private string FindCell(string name, int row = 1)
        {
            string cell = null;
            for (int currentRow = row; currentRow <= EndCell.Row; currentRow++)
            {
                for (int currentCollumn = 1; currentCollumn < EndCell.Column; currentCollumn++)
                {
                    object cellObject = Worksheet.Cells[currentRow, currentCollumn].Value;
                    if (cellObject != null)
                    {
                        if (cellObject.ToString().ToLower().Replace(" ", string.Empty).Replace($"\n", string.Empty) == name.ToLower())
                        {
                            cell = Regex.Replace(Worksheet.Cells[currentRow, currentCollumn].Address, @"[\d-]", string.Empty);
                            return cell;
                        }
                        else
                        {
                            continue;
                        }

                    }

                }
            }
            return cell;
        }

        private int FindRow(string cell, string value, int startRow = 1)
        {
            int suspectedRow = 0;
            for (int row = startRow; row <= EndCell.Row; row++)
            {
                ExcelRange selectedCell = Worksheet.Cells[$"{cell}{row}"];
                if (selectedCell.Value != null)
                {
                    string cellValue = selectedCell.Value.ToString().ToLower().Replace(" ", string.Empty);
                    string inputValue = value.ToString().ToLower().Replace(" ", string.Empty);
                    if (cellValue == inputValue)
                    {
                        suspectedRow = row;
                        break;
                    }
                }
                else
                {
                    continue;
                }
            }
            return suspectedRow;
        }

        public ExcelCellAddress? StartCell { set; get; }
        public ExcelCellAddress? EndCell { set; get; }
        public ExcelWorksheet? Worksheet { set; get; }
        public int InRowStart { set; get; }
        public int InRowEnd { set; get; }
        public bool IsAcceptable { set; get; }

        public string No { set; get; }
        public string NIK { set; get; }
        public string Name { set; get; }
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

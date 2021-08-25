﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Payroll.ViewModels
{
    public class AddressStaff
    {
        public AddressStaff(ExcelWorksheet excelWorksheet)
        {
            Worksheet = excelWorksheet;
            HeaderRow = FindHeaderRow();
            if (HeaderRow != 0)
            {
                DataStartRow = HeaderRow + 1;
                NIK = FindHeaderColumn("nik");
                Name = FindHeaderColumn("nama;name");
                MainSalaryBilling = FindHeaderColumn("gapok");
                InsentiveBilling = FindHeaderColumn("tunjangan gp");
                PulseAllowance = FindHeaderColumn("uang pulsa");
                PositionInsentiveBilling = FindHeaderColumn("tunjangan jabatan");
                AnotherInsentiveBilling = FindHeaderColumn("tunjangan lainnya");
                Rapel = FindHeaderColumn("rapel");
                
                AnotherDeduction = FindHeaderColumn("potongan kasbon");

                IsValid = NIK != null && Name != null && MainSalaryBilling != null && InsentiveBilling !=null && PulseAllowance != null && PositionInsentiveBilling !=null && AnotherInsentiveBilling != null;
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
            for (int currentRow = Worksheet.Dimension.End.Row; currentRow >= Worksheet.Dimension.Start.Row; currentRow--)
            {
                ExcelRange selectedCell1 = Worksheet.Cells[$"{NIK}{currentRow}"];
                ExcelRange selectedCell2 = Worksheet.Cells[$"{Name}{currentRow}"];
                if (selectedCell1.Value != null && selectedCell2.Value != null)
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
        public string InsentiveBilling { set; get; }



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
        
        public string Rapel { set; get; }
        public string AnotherInsentiveBilling { set; get; }
        public string PositionInsentiveBilling { set; get; }
        public string PulseAllowance { set; get; }
    }
}

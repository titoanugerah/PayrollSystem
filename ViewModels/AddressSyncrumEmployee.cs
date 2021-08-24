using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Payroll.ViewModels
{
    public class AddressSyncrumEmployee
    {
        public AddressSyncrumEmployee(ExcelWorksheet excelWorksheet)
        {
            Worksheet = excelWorksheet;
            No = GetCellAddress("no");
            NIK = GetCellAddress("nik");
            Name = GetCellAddress("nama");
            PhoneNumber = GetCellAddress("no hp");
            PositionId = GetCellAddress("jabatan");
            FamilyStatusCode = GetCellAddress("statuskeluarga");
            //BpjsStatusId = GetCellAddress("BPJS Kes");
            //BankCode = GetCellAddress("Bank");
            BankCode = "J";
            //AccountNumber = GetCellAddress("No REK");
            AccountNumber = "K";
            //string cellStart = (Worksheet.MergedCells
            //   .Where(cell => cell.Contains(GetCell("no").Address))
            //   .LastOrDefault().Split(":").LastOrDefault());
            string cellStart = "5A";
            DataStartRow = int.Parse(Regex.Replace(cellStart, @"[^\d]", "")) + 1;
            if (Worksheet.Dimension == null)
            {
                IsValid = false;
            }
            else
            {
                DataEndRow = Worksheet.Dimension.End.Row;              
                IsValid = (No != null & Name!=null & PositionId != null && FamilyStatusCode != null && BankCode != null && AccountNumber != null );
            }
        }
       

        public string GetCellAddress(string keyword)
        {
            string cellAddress = null;
            //List<string> mergedAddresess = Worksheet.MergedCells.ToList();
            List<string> keywordLists = new List<string>();
            foreach (string keywordList in keyword.Split(";").ToList())
            {
                keywordLists.Add(GetStringValue(keywordList));
            }

            bool isAny = Worksheet.Cells
                .Where(cell => keywordLists.Contains(GetStringValue(cell.Value)))
                .Select(cell => cell.Address)
                .Any();
            if (isAny)
            {
                cellAddress = Regex.Replace(Worksheet.Cells
                .Where(cell => keywordLists.Contains(GetStringValue(cell.Value)))
                .Select(cell => cell.Address)
                .FirstOrDefault(), @"[\d-]", string.Empty);
            }

            return cellAddress;
        }

        public ExcelRangeBase GetCell(string keyword)
        {
            List<string> keywordLists = new List<string>();
            foreach (string keywordList in keyword.Split(";").ToList())
            {
                keywordLists.Add(GetStringValue(keywordList));
            }

            bool isAny = Worksheet.Cells
                .Where(cell => keywordLists.Contains(GetStringValue(cell.Value)))
                .Select(cell => cell.Address)
                .Any();
            if (isAny)
            {
                return Worksheet.Cells
                .Where(cell => keywordLists.Contains(GetStringValue(cell.Value)))
                .Select(cell => cell)
                .FirstOrDefault();
            }
            else
            {
                return null;
            }

        }


        public string GetStringValue(object value)
        {
            if (value != null)
            {
                return new string(value.ToString().Where(char.IsLetter).ToArray()).ToLower();
            }
            else
            {
                return null;
            }
        }

        public ExcelWorksheet Worksheet { set; get; }
        public int DataStartRow { set; get; }
        public int DataEndRow { set; get; }
        public bool IsValid { set; get; }

        public string No { set; get; }
        public string NIK { set; get; }
        public string Name { set; get; }
        public string PhoneNumber { set; get; }
        public string PositionId { set; get; }
        public string FamilyStatusCode { set; get; }
        public string BpjsStatusId { set; get; }
        public string AccountName { set; get; }
        public string BankCode { set; get; }
        public string AccountNumber { set; get; }
    }
}

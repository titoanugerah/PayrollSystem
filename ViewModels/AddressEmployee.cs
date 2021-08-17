using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Payroll.ViewModels
{
    public class AddressEmployee
    {
        public AddressEmployee(ExcelWorksheet excelWorksheet)
        {
            Worksheet = excelWorksheet;            
            IsExist = GetCellAddress("aktif");
            No = GetCellAddress("no");
            NIK = GetCellAddress("nik");
            Name = GetCellAddress("nama");
            PhoneNumber = GetCellAddress("no handphone;no hp");
            PositionId = GetCellAddress("jabatan");
            if (Worksheet.MergedCells.Where(cell => cell.StartsWith(PositionId)).Any())
            {
                DriverPosition = GetCellAddress("driver");
                HelperPosition = GetCellAddress("helper");
                CheckerPosition = GetCellAddress("checker");
                NonDriverPosition = GetCellAddress("nondriver");
                IsPositionMultiColumn = HelperPosition != null && CheckerPosition != null & NonDriverPosition != null;                
            }
            LocationId = GetCellAddress("lokasi; rute");
            CustomerId = GetCellAddress("customer; costomer");
            FamilyStatusCode = GetCellAddress("status l - k1, 2, 3;status keluarga");
            DriverLicenseType = GetCellAddress("sim");
            DriverLicense = GetCellAddress("no sim");
            //DriverLicenseExpire = GetCellAddress("masa berlaku;sim berlaku");                        
            BpjsNumber = GetCellAddress("BPJS Kesehatan; BPJS KES");
            BpjsRemark = GetCellAddress("KETERANGAN BPJS Kesehatan; BPJS KES");
            JamsostekNumber = GetCellAddress("jamsostek; BPJS TK");
            JamsostekRemark = GetCellAddress("KET JAMSOSTEK");
            KTP = GetCellAddress("No. KTP; No KTP");
            BankCode = GetCellAddress("Bank");
            AccountName = GetCellAddress("Nama di Bank");
            AccountNumber = GetCellAddress("No.Account;No REK");
            string cellStart = (Worksheet.MergedCells
               .Where(cell => cell.Contains(GetCell("no").Address))
               .FirstOrDefault().Split(":").LastOrDefault());
            DataStartRow = int.Parse(Regex.Replace(cellStart, @"[^\d]", "")) + 1;
            DataEndRow = Worksheet.Dimension.End.Row;

            IsValid = (IsExist != null && No != null & Name!=null & PositionId != null && FamilyStatusCode != null && KTP != null && BankCode != null && AccountNumber != null );
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
        public bool IsPositionMultiColumn { set; get; }

        public string No { set; get; }
        public string NIK { set; get; }
        public string Name { set; get; }
        public string PhoneNumber { set; get; }
        public string KTP { set; get; }
        public string DriverLicenseType { set; get; }
        public string DriverLicense { set; get; }
        public string DriverLicenseExpire { set; get; }
        public string FamilyStatusCode { set; get; }
        public string BpjsNumber { set; get; }
        public string BpjsRemark { set; get; }
        public string JamsostekNumber { set; get; }
        public string JamsostekRemark { set; get; }
        public string NPWP { set; get; }
        public string BankCode { set; get; }
        public string AccountNumber { set; get; }
        public string AccountName { set; get; }
        public string PositionId { set; get; }
        public string LocationId { set; get; }
        public string CustomerId { set; get; }
        public string IsExist { set; get; }
        public string DriverPosition { set; get; }
        public string HelperPosition { set; get; }
        public string CheckerPosition { set; get; }
        public string NonDriverPosition { set; get; }
    }
}

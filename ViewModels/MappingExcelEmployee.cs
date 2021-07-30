using OfficeOpenXml;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Payroll.ViewModels
{
    public class MappingExcelEmployee
    {
        public MappingExcelEmployee(ExcelWorksheet worksheet, int initialRow = 1)
        {
            Worksheet = worksheet;
            if (Worksheet!=null)
            {
                StartCell = worksheet.Dimension.Start;
                EndCell = worksheet.Dimension.End;
                No = FindCell("NO", initialRow);
                InRowStart = FindRow(No, "1", initialRow);
                NIK = FindCell("NIK", initialRow);
                Name = FindCell("NAMA", initialRow);
                PhoneNumber = FindCell("NOHANDPHONE", initialRow);
                LocationId = FindCell("LOKASI", initialRow);
                CustomerId = FindCell("COSTUMER", initialRow);
                JoinCustomerDate = FindCell("TANGGALBERGABUNGDICUSTOMER", initialRow);
                Address = FindCell("ALAMAT", initialRow);
                FamilyStatusCode = FindCell("STATUSL-K1,2,3", initialRow);
                Religion = FindCell("AGAMA", initialRow);
                Sex = FindCell("JENISKELAMIN", initialRow);
                BirthPlace = FindCell("TEMPATLAHIR", initialRow);
                BirthDate = FindCell("TANGGALLAHIR", initialRow);
                StartContract = FindCell("START", initialRow);
                JoinCompanyDate = FindCell("START", initialRow);
                EndContract = FindCell("FINISH", initialRow);
                EmploymentStatusId = FindCell("STATUSKONTRAKPKWT", initialRow);
                DriverLicenseType = FindCell("SIM", initialRow);
                DriverLicense = FindCell("NOSIM", initialRow);
                DriverLicenseExpire = FindCell("SIMBERLAKU", initialRow);
                HasUniform = FindCell("SERAGAM", initialRow);
                UniformDeliveryDate = FindCell("TGLPENGIRIMAN", initialRow);
                HasIdCard = FindCell("IDCARD", initialRow);
                IdCardDeliveryDate = FindCell("TGLPENGIRIMAN", initialRow);
                TrainingName = FindCell("TRAINING", initialRow);
                TrainingRemark = FindCell("KETTRAINING", initialRow);
                TrainingGrade = FindCell("NILAITRAINING", initialRow);
                HasTraining = FindCell("STATUSTRAINING", initialRow);
                BpjsNumber = FindCell("BPJSKESEHATAN", initialRow);
                BpjsRemark = FindCell("KETERANGANPROSESBPJSKesehatan", initialRow);
                JamsostekNumber= FindCell("Jamsostek", initialRow);
                JamsostekRemark = FindCell("KETJAMSOSTEK", initialRow);
                NPWP = FindCell("NPWP", initialRow);
                AccountName = FindCell("NAMADIBANK", initialRow);
                BankCode = FindCell("BANK", initialRow);
                AccountNumber = FindCell("NO.ACCOUNT", initialRow);
                KK = FindCell("NOKK", initialRow);
                KTP = FindCell("No.KTP", initialRow);
                IsDriverPosition = FindCell("DRIVER", initialRow);
                IsHelperPosition = FindCell("HELPER", initialRow);
                IsCheckerPosition = FindCell("CHECKER", initialRow);
                IsNonDriverPosition = FindCell("NONDRIVER", initialRow);
                IsExist = FindCell("AKTIF", initialRow);


                for (int currentRow = InRowStart; currentRow < EndCell.Row;  currentRow++)
                {
                    ExcelRange selectedCell = Worksheet.Cells[$"{No}{currentRow}"];
                    if (selectedCell.Value == null)
                    {
                        InRowEnd = currentRow;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (NIK != null && Name != null && FamilyStatusCode != null )
                {
                    IsAcceptable = true;
                }
                else
                {
                    IsAcceptable = false;

                }
            }
        }

        private string FindCell(string name, int row = 1)
        {
            string cell = null;
            for (int currentRow = row; currentRow < EndCell.Row; currentRow++)
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
            for (int row = startRow; row < EndCell.Row ; row++)
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

        public bool IsAcceptable { set; get; }
        public ExcelCellAddress? StartCell{ set; get; }
        public ExcelCellAddress? EndCell{ set; get; }
        public int InRowStart { set; get; }
        public int InRowEnd { set; get; }
        public int Row { set; get; }

        public ExcelWorksheet? Worksheet { set; get; }
        public string IsExist { set; get; }
        public string No { set; get; }
        public string NIK { set; get; }
        public string Name { set; get; }
        public string Sex { set; get; }
        public string BirthPlace { set; get; }
        public string BirthDate { set; get; }
        public string Religion { set; get; }
        public string Address { set; get; }
        public string PhoneNumber { set; get; }
        public string KTP { set; get; }
        public string KK { set; get; }
        public string DriverLicenseType { set; get; }
        public string DriverLicense { set; get; }
        public string DriverLicenseExpire { set; get; }
        public string FamilyStatusCode { set; get; }
        public string BpjsNumber { set; get; }
        public string BpjsRemark { set; get; }
        public string JamsostekNumber { set; get; }
        public string JamsostekRemark { set; get; }
        public string NPWP { set; get; }

        public string JoinCompanyDate { set; get; }
        public string StartContract { set; get; }
        public string EndContract { set; get; }

        public string BankCode { set; get; }
        public string AccountNumber { set; get; }
        public string AccountName { set; get; }
        public string EmploymentStatusId { set; get; }
        public string HasUniform { set; get; }
        public string UniformDeliveryDate { set; get; }
        
        public string HasIdCard { set; get; }
        public string IdCardDeliveryDate { set; get; }
        public string HasTraining { set; get; }
        public string TrainingName { set; get; }
        public string TrainingRemark { set; get; }
        public string TrainingGrade { set; get; }
        public string TrainingDeliveryDate { set; get; }

        public string IsDriverPosition { set; get; }
        public string IsHelperPosition { set; get; }
        public string IsCheckerPosition { set; get; }
        public string IsNonDriverPosition { set; get; }
        public string LocationId { set; get; }
        public string CustomerId { set; get; }
        public string JoinCustomerDate { set; get; }

    }
}

using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Payroll.ViewModels
{
    public class MappingExcelEmployee
    {
        public MappingExcelEmployee(ExcelWorksheet worksheet)
        {
            Worksheet = worksheet;
            if (Worksheet!=null)
            {
                StartCell = worksheet.Dimension.Start;
                EndCell = worksheet.Dimension.End;
                NIK = FindCell("NIK");
                Name = FindCell("NAMA");
                PhoneNumber = FindCell("NOHANDPHONE");
                LocationId = FindCell("LOKASI");
                CustomerId = FindCell("COSTUMER");
                JoinCustomerDate = FindCell("TANGGALBERGABUNGDICUSTOMER");
                Address = FindCell("ALAMAT");
                FamilyStatusCode = FindCell("STATUSL-K1,2,3");
                Religion = FindCell("AGAMA");
                Sex = FindCell("JENISKELAMIN");
                BirthPlace = FindCell("TEMPATLAHIR");
                BirthDate = FindCell("TANGGALLAHIR");
                StartContract = FindCell("START");
                JoinCompanyDate = FindCell("START");
                EndContract = FindCell("FINISH");
                EmploymentStatusId = FindCell("STATUSKONTRAKPKWT");
                DriverLicenseType = FindCell("SIM");
                DriverLicense = FindCell("NOSIM");
                DriverLicenseExpire = FindCell("SIMBERLAKU");
                HasUniform = FindCell("SERAGAM");
                UniformDeliveryDate = FindCell("TGLPENGIRIMAN");
                HasIdCard = FindCell("IDCARD");
                IdCardDeliveryDate = FindCell("TGLPENGIRIMAN");
                TrainingName = FindCell("TRAINING");
                TrainingRemark = FindCell("KETTRAINING");
                TrainingGrade = FindCell("NILAITRAINING");
                HasTraining = FindCell("STATUSTRAINING");
                BpjsNumber = FindCell("BPJSKESEHATAN");
                BpjsRemark = FindCell("KETERANGANPROSESBPJSKesehatan");
                JamsostekNumber= FindCell("Jamsostek");
                JamsostekRemark = FindCell("KETJAMSOSTEK");
                NPWP = FindCell("NPWP");
                AccountName = FindCell("NAMADIBANK");
                BankCode = FindCell("BANK");
                AccountNumber = FindCell("NO.ACCOUNT");
                KK = FindCell("NOKK");
                KTP = FindCell("No.KTP");
                PositionId = FindCell("JABATAN");
                IsExist = FindCell("AKTIF");
            }
        }

        private string FindCell(string name)
        {
            string cell = null;
            for (int currentRow = 1; currentRow < EndCell.Row; currentRow++)
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

        public ExcelCellAddress? StartCell{ set; get; }
        public ExcelCellAddress? EndCell{ set; get; }
        public int Row { set; get; }

        public ExcelWorksheet? Worksheet { set; get; }
        public string IsExist { set; get; }
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

        public string PositionId { set; get; }
        public string LocationId { set; get; }
        public string CustomerId { set; get; }
        public string JoinCustomerDate { set; get; }

    }
}

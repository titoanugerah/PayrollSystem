using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class Audit
    {
        [Column(TypeName = "bit")]
        [DefaultValue(true)]
        public bool IsExist { set; get; }
        public int CreateBy { set; get; }
        public int? ModifyBy { set; get; }
        [NotMapped]
        public string CreateByName { set; get; }
        [NotMapped]
        public string ModifyByName { set; get; }
        private DateTime createDateUtc { set; get; }
        public DateTime CreateDateUtc 
        { 
            set
            {
                createDateUtc = value;
            }
            get
            {
                if (createDateUtc.Kind == DateTimeKind.Unspecified)
                {
                    return DateTime.SpecifyKind(createDateUtc, DateTimeKind.Utc);
                }
                else
                {
                    return createDateUtc;
                }
            }
        }

        private DateTime modifyTimeUtc { set; get; }
        public DateTime ModifyTimeUtc
        {
            set
            {
                modifyTimeUtc = value;
            }
            get
            {
                if (modifyTimeUtc.Kind == DateTimeKind.Unspecified)
                {
                    return DateTime.SpecifyKind(modifyTimeUtc, DateTimeKind.Utc);
                }
                else
                {
                    return modifyTimeUtc;
                }
            }
        }


    }
}

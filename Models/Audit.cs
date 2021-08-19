using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class Audit
    {
        private bool isExist;

        [Column(TypeName = "bit")]
        public bool IsExist {
            set 
            {
                if (value == null)
                {
                    isExist = true;
                } 
                else
                {
                    isExist = value;
                }
            }
            get 
            {
                return isExist;                 
            }
        }
        public string CreateBy { set; get; }
        public string? ModifyBy { set; get; }
        [NotMapped]
        public string CreateByName { set; get; }
        [NotMapped]
        public string ModifyByName { set; get; }
        private DateTime createDateUtc;
        public DateTime CreateDateUtc
        {
            get
            {
                if (createDateUtc.Kind == DateTimeKind.Unspecified)
                {
                    return DateTime.SpecifyKind(createDateUtc, DateTimeKind.Utc);
                }
                return createDateUtc;
            }
            set
            {
                createDateUtc = value;
            }
        }

        private DateTime? modifyDateUtc;
        public DateTime? ModifyDateUtc
        {
            get
            {
                if (modifyDateUtc.HasValue && modifyDateUtc.Value.Kind == DateTimeKind.Unspecified)
                {
                    return DateTime.SpecifyKind(modifyDateUtc.Value, DateTimeKind.Utc);
                }
                return modifyDateUtc;
            }
            set
            {
                modifyDateUtc = value;
            }
        }


    }
}

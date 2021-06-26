﻿using System.ComponentModel.DataAnnotations;

namespace Payroll.Models
{
    public class District : Audit
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string Remark { set; get; }
    }
}

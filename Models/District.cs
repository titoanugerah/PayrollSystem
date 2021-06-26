﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Models
{
    public class District : Audit
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        public string Remark { set; get; }
        [NotMapped]
        public string Button
        {
            get
            {
                return $"<button type='button' onclick='showEditForm({Id})' class='btn btn-info'>Edit</button>";
            }
        }
    }
}

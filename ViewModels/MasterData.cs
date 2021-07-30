﻿using Payroll.Models;
using System.Collections.Generic;

namespace Payroll.ViewModels
{
    public class MasterData
    {
        public List<Customer> Customers { set; get; }
        public List<Position> Positions { set; get; }
        public List<Location> Locations { set; get; }
        public List<FamilyStatus> FamilyStatuses { set; get; }
        public List<EmploymentStatus> EmploymentStatuses { set; get; }
        public List<Employee> Employees { set; get; }
        public List<District> Districts { set; get; }
        public List<Bank> Banks { set; get; }
        public List<Role> Roles { set; get; }

    }
}
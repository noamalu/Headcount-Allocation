using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.Domain;
using static HeadcountAllocation.Domain.Enums;

namespace API.Models
{
    public class Employee
    {
        public string? EmployeeName { get; set; }

        public int EmployeeId { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public int TimeZone { get; set; }

        public List<Language> ForeignLanguages { get; set; } = new();

        public List<Skill> Skills { get; set; } = new();

        public int YearsExperience { get; set; }

        public double JobPercentage { get; set; }
    }
}
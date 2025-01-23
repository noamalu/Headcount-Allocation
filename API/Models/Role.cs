using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        
        public int ProjectId { get; set; }
        
        public int? EmployeeId { get; set; }
        public int TimeZone{get;set;}
        
        public List<Language> ForeignLanguages {get; set;}

        public List<Skill> Skills {get; set;}

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}

    }
}
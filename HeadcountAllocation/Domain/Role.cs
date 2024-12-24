using System.Collections.Concurrent;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class Role{

        public string? RoleName{get;set;}

        public int RoleId{get;set;}

        public int EmployeeId{get;set;}

        public int ProjectId{get;set;}

        public TimeZones TimeZone{get;set;}
        
        public List<string>? ForeignLanguages{get;set;} = new();

        public ConcurrentDictionary<int, Skill> Skills{get;set;} = new();

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}
    }
}
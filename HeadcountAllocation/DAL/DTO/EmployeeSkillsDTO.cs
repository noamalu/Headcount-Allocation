using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.Domain;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.DAL.DTO
{
    [Table("EmployeeSkills")]
    
    public class EmployeeSkillsDTO
    {
        public int SkillTypeId {get;set;} 
        
        [ForeignKey("Employees")]
        public int EmployeeId { get; }
        public int Level {get;set;} 
        public int Priority {get; set;}

        public EmployeeSkillsDTO() { }
        public EmployeeSkillsDTO(int skillTypeId,  int level, int prioriry)
        {
            SkillTypeId = skillTypeId;
            Level = level;
            Priority = prioriry;
        }

            public EmployeeSkillsDTO(Skill skill)
        {
            SkillTypeId = Enums.GetId(skill.SkillType);
            Level = skill.Level;
            Priority = skill.Priority;
        }
        
    }
}
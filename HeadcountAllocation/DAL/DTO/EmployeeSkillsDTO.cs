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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int SkillId {get;set;} 
        
        [ForeignKey("Employees")]
        public int EmployeeId { get; }
        public int SkillTypeId {get;set;} 
        public int Level {get;set;} 
        public int Priority {get; set;}

        public EmployeeSkillsDTO() { }
        public EmployeeSkillsDTO(int skillId, int skillTypeId,  int level, int prioriry)
        {
            SkillId = skillId;
            SkillTypeId = skillTypeId;
            Level = level;
            Priority = prioriry;
        }

            public EmployeeSkillsDTO(Skill skill)
        {
            SkillId = skill.SkillId;
            SkillTypeId = Enums.GetId(skill.SkillType);
            Level = skill.Level;
            Priority = skill.Priority;
        }
        
    }
}
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
    [Table("RoleSkills")]
    
    public class RoleSkillsDTO
    {
        public int SkillTypeId {get;set;} 
        
        [ForeignKey("Roles")]
        public int RoleId { get; }
        public int Level {get;set;} 
        public int Priority {get;set;}

        public RoleSkillsDTO() { }
        public RoleSkillsDTO(int skillTypeId,  int level, int prioriry)
        {
            SkillTypeId = skillTypeId;
            Level = level;
            Priority = prioriry;
        }

        public RoleSkillsDTO(Skill skill)
        {
            SkillTypeId = Enums.GetId(skill.SkillType);
            Level = skill.Level;
            Priority = skill.Priority;
        }

        
    }
}
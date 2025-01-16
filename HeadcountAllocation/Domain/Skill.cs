using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain
{
    public class Skill
    {
        public int SkillId {get;set;} 
        public Skills SkillType {get;set;} 
        public int Level {get;set;}
        public int Priority {get; set;} 

        public Skill(Skills skillType, int level, int priority){
            SkillId = Enums.GetId(skillType);
            SkillType = skillType;
            Level = level;
            Priority = priority;
        }

        public Skill(EmployeeSkillsDTO skill)
        {
            SkillId = skill.SkillTypeId;
            SkillType = GetValueById<Skills>(skill.SkillTypeId);
            Level = skill.Level;
            Priority = skill.Priority;
        }

        public Skill(RoleSkillsDTO skill)
        {
            SkillId = skill.SkillTypeId;
            SkillType = GetValueById<Skills>(skill.SkillTypeId);
            Level = skill.Level;
            Priority = skill.Priority;
        }

        
    }
}
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

        public Skill(int skillId, Skills skillType, int level){
            SkillId = skillId;
            SkillType = skillType;
            Level = level;
        }

        public Skill(EmployeeSkillsDTO skill)
        {
            SkillId = skill.SkillId;
            SkillType = GetValueById<Skills>(skill.SkillTypeId);
        }

        public Skill(RoleSkillsDTO skill)
        {
            SkillId = skill.SkillId;
            SkillType = GetValueById<Skills>(skill.SkillTypeId);
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Skill
    {
        public int SkillId {get;set;} 
        public int SkillTypeId {get;set;} 
        public int Level {get;set;}
        public int Priority {get; set;} 

        public static explicit operator HeadcountAllocation.Domain.Skill(Skill skill)
        {
            return 
                new HeadcountAllocation.Domain.Skill
                    (HeadcountAllocation.Domain.Enums
                        .GetValueById<HeadcountAllocation.Domain.Enums.Skills>
                            (skill.SkillTypeId),
                    skill.Level, 
                    skill.Priority);
            
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain
{
    public class Skill
    {
       public int SkillId {get;set;} 
       public Skills SkillType {get;set;} 
       public int Level {get;set;} 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Skill
    {
        public int SkillId {get;set;} 
        public string SkillType {get;set;} 
        public int Level {get;set;}
        public int Priority {get; set;} 

    }
}
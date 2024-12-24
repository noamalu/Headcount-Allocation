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
    [Table("Skills")]
    
    public class SkillDTO
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int SkillId {get;set;} 
       public Skills SkillType {get;set;} 
       public int Level {get;set;} 

    public SkillDTO() { }
        public SkillDTO(int skillId, Skills skillType,  int level)
        {
            SkillId = skillId;
            SkillType = skillType;
            Level = level;
        }

         public SkillDTO(Skill skill)
        {
            SkillId = skill.SkillId;
            SkillType = skill.SkillType;
            Level = skill.Level;
        }

        
    }
}
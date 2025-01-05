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
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int SkillId {get;set;} 
        
        [ForeignKey("Roles")]
        public int RoleId { get; }
       public int SkillTypeId {get;set;} 
       public int Level {get;set;} 

    public RoleSkillsDTO() { }
    public RoleSkillsDTO(int skillId, int skillTypeId,  int level)
    {
        SkillId = skillId;
        SkillTypeId = skillTypeId;
        Level = level;
    }

        //  public SkillDTO(Skill skill)
        // {
        //     SkillId = skill.SkillId;
        //     SkillTypeId = skill.SkillType;
        //     Level = skill.Level;
        // }

        
    }
}
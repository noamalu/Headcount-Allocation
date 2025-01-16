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
    [Table("SkillTypes")]
    
    public class SkillTypesDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int SkillTypeId {get;set;} 
        public string SkillTypeName { get; set;}

        public SkillTypesDTO() { }
        public SkillTypesDTO(int skillTypeId, string skillTypeName)
        {
            SkillTypeId = skillTypeId;
            SkillTypeName = skillTypeName;
        }

        public SkillTypesDTO(Enums.Skills skills)
        {
            SkillTypeId = Enums.GetId(skills);
            SkillTypeName = skills.ToString();
        }

        
    }
}
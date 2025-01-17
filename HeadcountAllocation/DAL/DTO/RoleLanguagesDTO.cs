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
    [Table("RoleLanguages")]
    public class RoleLanguagesDTO
    {
        
        public int LanguageTypeId { get; set; }

        [ForeignKey("Roles")]
        public int RoleId { get; }
        public int Level { get; set; }


        public RoleLanguagesDTO() { }
        public RoleLanguagesDTO(int languageTypeId, int roleId, int level)
        {
            LanguageTypeId = languageTypeId;
            RoleId = roleId;
            Level = level;
        }

         public RoleLanguagesDTO(Language language)
        {
            LanguageTypeId = Enums.GetId(language.LanguageType);
            Level = language.Level;
        }
    }
}
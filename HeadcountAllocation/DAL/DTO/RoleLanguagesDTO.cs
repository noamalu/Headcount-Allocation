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
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        
        public int LanguageID { get; set; }

        [ForeignKey("Roles")]
        public int RoleId { get; }
        public int LanguageTypeId {get; set;}
        public int Level { get; set; }


        public RoleLanguagesDTO() { }
        public RoleLanguagesDTO(int LanguageID, int LanguageTypeId, int Level)
        {
            LanguageID = LanguageID;
            LanguageTypeId = LanguageTypeId;
            Level = Level;
        }

         public RoleLanguagesDTO(Language language)
        {
            LanguageID = language.LanguageID;
            LanguageTypeId = Enums.GetId(language.LanguageType);
            Level = language.Level;
        }
    }
}
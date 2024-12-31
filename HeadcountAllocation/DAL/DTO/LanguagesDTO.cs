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
    [Table("Languages")]
    public class LanguagesDTO
    {
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        
        public int LanguageID { get; set; }

        [ForeignKey("Employees")]
        public int? EmployeeId { get; }

        [ForeignKey("Employees")]
        public int? RoleId { get; }
        public Languages LanguageType {get; set;}
        public int Level { get; set; }


        public LanguagesDTO() { }
        public LanguagesDTO(int LanguageID, Languages LanguageType, int Level)
        {
            LanguageID = LanguageID;
            LanguageType = LanguageType;
            Level = Level;
        }

         public LanguagesDTO(Language language)
        {
            LanguageID = language.LanguageID;
            LanguageType = language.LanguageType;
            Level = language.Level;
        }
    }
}
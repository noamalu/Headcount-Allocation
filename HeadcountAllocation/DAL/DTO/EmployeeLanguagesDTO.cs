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
    [Table("EmployeeLanguages")]
    public class EmployeeLanguagesDTO
    {
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        
        public int LanguageID { get; set; }

        [ForeignKey("Employees")]
        public int EmployeeId { get; }
        public int LanguageTypeId {get; set;}
        public int Level { get; set; }


        public EmployeeLanguagesDTO() { }
        public EmployeeLanguagesDTO(int LanguageID, int LanguageTypeId, int Level)
        {
            LanguageID = LanguageID;
            LanguageTypeId = LanguageTypeId;
            Level = Level;
        }

         public EmployeeLanguagesDTO(Language language)
        {
            LanguageID = language.LanguageID;
            LanguageTypeId = Enums.GetId(language.LanguageType);
            Level = language.Level;
        }
    }
}
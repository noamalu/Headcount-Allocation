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
        public int LanguageTypeId { get; set; }

        [ForeignKey("Employees")]
        public int EmployeeId { get; }
        public int Level { get; set; }


        public EmployeeLanguagesDTO() { }
        public EmployeeLanguagesDTO(int languageTypeId, int employeeId, int level)
        {
            LanguageTypeId = languageTypeId;
            EmployeeId = employeeId;
            Level = level;
        }

         public EmployeeLanguagesDTO(Language language)
        {
            LanguageTypeId = Enums.GetId(language.LanguageType);
            Level = language.Level;
        }
    }
}
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
    [Table("LanguageTypes")]
    
    public class LanguageTypesDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int LanguageTypeId {get;set;} 
        public string LanguageTypeName { get; set;}

        public LanguageTypesDTO() { }
        public LanguageTypesDTO(int languageTypeId, string languageTypeName)
        {
            LanguageTypeId = languageTypeId;
            LanguageTypeName = languageTypeName;
        }

        public LanguageTypesDTO(Enums.Languages language)
        {
            LanguageTypeId = Enums.GetId(language);
            LanguageTypeName = language.ToString();
        }

        
    }
}
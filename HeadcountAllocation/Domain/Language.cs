using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using Microsoft.Identity.Client;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain
{
    public class Language
    {
       public int LanguageID {get;set;} 
       public Languages LanguageType {get;set;} 
       public int Level {get;set;} 

       public Language(int languageId, Languages languageType, int level){
            LanguageID = languageId;
            LanguageType = languageType;
            Level = level;
       }

        public Language(EmployeeLanguagesDTO language)
        {
            LanguageID = language.LanguageID;
            LanguageType = GetValueById<Languages>(language.LanguageTypeId);
            Level = language.Level;
        }

        public Language(RoleLanguagesDTO language)
        {
            LanguageID = language.LanguageID;
            LanguageType = GetValueById<Languages>(language.LanguageTypeId);
            Level = language.Level;
        }

    }
}
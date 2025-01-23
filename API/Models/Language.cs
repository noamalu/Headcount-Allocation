using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Language
    {
        public int LanguageID {get;set;} 
        public int LanguageTypeId {get;set;} 
        public int Level {get;set;} 
        
        public static explicit operator HeadcountAllocation.Domain.Language(Language language)
        {
            return 
                new HeadcountAllocation.Domain.Language
                    (HeadcountAllocation.Domain.Enums
                        .GetValueById<HeadcountAllocation.Domain.Enums.Languages>
                            (language.LanguageTypeId),
                    language.Level);
            
        }
    }
}
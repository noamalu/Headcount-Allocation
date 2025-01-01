using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain
{
    public class Language
    {
       public int LanguageID {get;set;} 
       public Languages LanguageType {get;set;} 
       public int Level {get;set;} 
    }
}
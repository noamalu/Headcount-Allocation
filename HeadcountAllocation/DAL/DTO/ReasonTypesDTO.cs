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
    [Table("ReasonTypes")]
    
    public class ReasonTypesDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int ReasonTypeId {get;set;} 
        public string ReasonTypeName { get; set;}

        public ReasonTypesDTO() { }
        public ReasonTypesDTO(int reasonTypeId, string reasonTypeName)
        {
            ReasonTypeId = reasonTypeId;
            ReasonTypeName = reasonTypeName;
        }

        public ReasonTypesDTO(Enums.Reasons reasons)
        {
            ReasonTypeId = Enums.GetId(reasons);
            ReasonTypeName = reasons.ToString();
        }

        
    }
}
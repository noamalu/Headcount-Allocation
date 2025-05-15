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
    [Table("TicketReasons")]
    
    public class TicketReasonsDTO
    {
        public int ReasonTypeId {get;set;} 
        
        [ForeignKey("Tickets")]
        public int TicketId { get; }
        public TicketReasonsDTO() { }
        public TicketReasonsDTO(int reasonTypeId)
        {
            ReasonTypeId = reasonTypeId;
        }

        public TicketReasonsDTO(Reason reason)
        {
            ReasonTypeId = Enums.GetId(reason.ReasonType);
        }

        
    }
}
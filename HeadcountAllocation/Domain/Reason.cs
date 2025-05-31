using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain
{
    public class Reason
    {
        public int ReasonId {get;set;} 
        public Reasons ReasonType {get;set;}

        public Reason(Reasons reasonType){
            ReasonId = Enums.GetId(reasonType);
            ReasonType = reasonType;
        }

        public Reason(TicketReasonsDTO reason)
        {
            ReasonId = reason.ReasonTypeId;
            ReasonType = GetValueById<Reasons>(reason.ReasonTypeId);
        }        
    }
}
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
    [Table("TimeZones")]
    
    public class TimeZonesDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int TimeZoneId {get;set;} 
        public string TimeZoneName { get; set;}

        public TimeZonesDTO() { }
        public TimeZonesDTO(int timeZoneId, string timeZoneName)
        {
            TimeZoneId = timeZoneId;
            TimeZoneName = timeZoneName;
        }

        public TimeZonesDTO(Enums.TimeZones timeZones)
        {
            TimeZoneId = Enums.GetId(timeZones);
            TimeZoneName = timeZones.ToString();
        }

        
    }
}
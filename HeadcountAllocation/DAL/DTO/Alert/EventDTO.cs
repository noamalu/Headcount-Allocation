using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeadcountAllocation.DAL.DTO.Alert
{
    [Table("Events")]
    public class EventDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public EmployeeDTO Listener { get; set; }
        public EventDTO() { }
        public EventDTO(string name, EmployeeDTO listener)
        {
            Name = name;
            Listener = listener;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeadcountAllocation.Domain.Alert
{
    [Table("Events")]
    public class EventDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        // public UserDto Listener { get; set; }
        public EventDTO() { }
        public EventDTO(string name, int projectId)//, MemberDTO listener)
        {
            Name = name;
            ProjectId = projectId;
            // Listener = listener;
        }
    }
}

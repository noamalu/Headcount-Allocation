using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.DTO
{
    [Table("Tickets")]
    public class TicketDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TicketId { get; set; }
        [ForeignKey("Employees")]
        public int EmployeeId {get;set;}
        public string EmployeeName {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get;set;}
        public string Description {get;set;}
        public bool Open {get;set;}

        public TicketDTO(){}
        public TicketDTO (int ticketId, int employeeId, string employeeName, DateTime startDate ,DateTime endDate, string description, bool open){
            TicketId = ticketId;
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Open = open;
        }

        public TicketDTO (Ticket ticket){
            TicketId = ticket.TicketId;
            EmployeeId = ticket.EmployeeId;
            EmployeeName = ticket.EmployeeName;
            StartDate = ticket.StartDate;
            EndDate = ticket.EndDate;
            Description = ticket.Description;
            Open = ticket.Open;
        }
    }
}
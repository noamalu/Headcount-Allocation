using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;

namespace HeadcountAllocation.Domain
{
    public class Ticket
    {
        public int TicketId {get;set;}
        public int EmployeeId {get;set;}
        public string EmployeeName {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get;set;}
        public string Description {get;set;}
        public bool Open {get;set;}

        public Ticket (int ticketId, int employeeId, string employeeName, DateTime startDate ,DateTime endDate, string description){
            TicketId = ticketId;
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Open = true;
        }

        public Ticket (TicketDTO ticketDTO){
            TicketId = ticketDTO.TicketId;
            EmployeeId = ticketDTO.EmployeeId;
            EmployeeName = ticketDTO.EmployeeName;
            StartDate = ticketDTO.StartDate;
            EndDate = ticketDTO.EndDate;
            Description = ticketDTO.Description;
            Open = ticketDTO.Open;
        }

        public void CloseTicket(){
            Open = false;
        }

    }
}
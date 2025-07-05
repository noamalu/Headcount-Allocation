using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;

namespace HeadcountAllocation.Domain
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool Open { get; set; }
        public Reason Reason {get; set;}

        public Ticket(int ticketId, int employeeId, string employeeName, DateTime startDate, DateTime endDate, string description, Reason reason)
        {
            TicketId = ticketId;
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Open = true;
            Reason = reason;
        }

        public Ticket(int ticketId, int employeeId, string employeeName, DateTime startDate, DateTime endDate, string description, Reason reason, bool open)
        {
            TicketId = ticketId;
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Open = open;
            Reason = reason;
        }

        public Ticket(TicketDTO ticketDTO)
        {
            TicketId = ticketDTO.TicketId;
            EmployeeId = ticketDTO.EmployeeId;
            EmployeeName = ticketDTO.EmployeeName;
            StartDate = ticketDTO.StartDate;
            EndDate = ticketDTO.EndDate;
            Description = ticketDTO.Description;
            Open = ticketDTO.Open;
            Reason = new Reason(ticketDTO.Reason);
        }
        public Ticket() { }

        public void CloseTicket()
        {
            Open = false;
        }

        public string TicketMessage()
        {
            var description = Description;

            var span = GetReadableDuration(StartDate, EndDate);
            var message =
            $@"{EmployeeName} Opened a ticket, 
            And will be out for: {span}. 
            Starting on {StartDate}, to {EndDate}
            Reason - {Reason}
            Description - {description}";
            return message;
        }

        public string TicketTitle()
        {
            var title = $@"Ticket: {EmployeeName} - {Reason}";

            return title;            
        }

        public static string GetReadableDuration(DateTime startDate, DateTime endDate)
        {
            TimeSpan duration = endDate - startDate;

            if (duration.TotalDays < 7)
                return $"{(int)duration.TotalDays} day{(duration.TotalDays >= 2 ? "s" : "")}";

            if (duration.TotalDays < 30)
            {
                int weeks = (int)Math.Round(duration.TotalDays / 7.0);
                return $"{weeks} week{(weeks > 1 ? "s" : "")}";
            }

            int totalMonths = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;
            if (endDate.Day < startDate.Day)
                totalMonths--;

            if (totalMonths < 12)
                return $"{totalMonths + 1} month{(totalMonths >= 1 ? "s" : "")}";

            int years = totalMonths / 12;
            return $"{years} year{(years > 1 ? "s" : "")}";
        }


    }
}
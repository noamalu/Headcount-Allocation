

using HeadcountAllocation.Domain;

namespace API.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public string AbsenceReason { get; set; } = "Other";
        public bool Open { get; set; } = true;

        public static explicit operator HeadcountAllocation.Domain.Ticket(Ticket ticket)
        {
            var reason = Enum.TryParse(ticket.AbsenceReason, out HeadcountAllocation.Domain.Enums.Reasons parsedReason)
                ? parsedReason
                : Enums.Reasons.Other;
            return new HeadcountAllocation.Domain.Ticket(
                ticket.TicketId,
                ticket.EmployeeId,
                ticket.EmployeeName,
                ticket.StartDate,
                ticket.EndDate,
                ticket.Description,
                new(reason)
                );
        }

        public void CloseTicket()
        {
            Open = false;
        }

    }
}
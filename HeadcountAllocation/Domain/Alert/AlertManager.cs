using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Hangfire;
using HeadcountAllocation.DAL.Repositories;
using WebSocketSharp.Server;

namespace HeadcountAllocation.Domain.Alert
{
    public class AlertManager
    {
        private static AlertManager _alertsManager = null;
        private static object _lock = new object();
        private WebSocketServer _alertsServer;

        private AlertManager(WebSocketServer alertsServer) 
        {
            _alertsServer = alertsServer;
        }
        private AlertManager() 
        {}

        public static AlertManager GetInstance(WebSocketServer alertsServer)
        {            
            if (_alertsManager is not null){
                _alertsManager._alertsServer = alertsServer;
                return _alertsManager;
            }
            lock (_lock)
            {
                _alertsManager ??= new AlertManager(alertsServer);
            }
            return _alertsManager;
        }

        public static AlertManager GetInstance()
        {            
            if (_alertsManager is not null)
                return _alertsManager;
            lock (_lock)
            {
                _alertsManager ??= new AlertManager();
            }
            return _alertsManager;
        }

        public void SendAlert(Ticket ticket, string username, MailAddress email, bool reminder = true)
        {
            SendEmail(ticket.TicketTitle(), ticket.TicketMessage(), email);
            
            // 1. Get ticket reminder date
            var reminderDate = ticket.StartDate.AddDays(-5);

            // 2. Pick closest Monday *before* the reminder
            var scheduledDate = GetClosestMonday(reminderDate);

            if(reminder)
                // 3. Schedule the job to run at that time
                BackgroundJob.Schedule(
                    () => AlertManager.CheckAndSendEmail(ticket.TicketId, email.Address),
                    scheduledDate - DateTime.Now
                );
            
            var relativePath = $"/{username}-alerts";

            if (_alertsServer is null || _alertsServer.WebSocketServices[relativePath] is null)
                return;

            var webSocketService = _alertsServer.WebSocketServices[relativePath];
            if (webSocketService is null || webSocketService.Sessions.Count <= 0)
                return;
            var message = ticket.TicketMessage();
            var json = JsonSerializer.Serialize(new { message });

            lock (_lock)
            {
                foreach (var session in webSocketService.Sessions.Sessions)
                {
                    try
                    {
                        var webSocket = session.Context.WebSocket;
                        if (webSocket?.IsAlive ?? false)
                            webSocket.Send(json);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }            

        }        

        public static void SendEmail(string title, string message, MailAddress email)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("headcount.allocation@gmail.com", "pldebnnskqbstqln"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("headcount.allocation@gmail.com"),
                    Subject = title,
                    Body = message,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        public static void CheckAndSendEmail(int ticketId, string email)
        {
            var ticket = TicketRepo.GetInstance().GetById(ticketId);
            if (ticket.Open )//&& ticket.StartDate.AddDays(5) <= DateTime.Now
            {
                SendEmail(
                    "Reminder: Employee Out Soon",
                    $"Reminder: {ticket.EmployeeName} will be out starting {ticket.StartDate}",
                    new MailAddress(email)
                );
            }            
        }

        public static DateTime GetClosestMonday(DateTime target)
        {
            // If it's already Monday, return as-is
            if (target.DayOfWeek == DayOfWeek.Monday)
                return target;

            // Choose the Monday BEFORE the target
            int diff = (7 + (target.DayOfWeek - DayOfWeek.Monday)) % 7;
            return target.AddDays(-diff);
        }

    }
}

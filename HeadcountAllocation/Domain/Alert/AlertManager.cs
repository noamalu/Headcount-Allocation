using System.Net;
using System.Net.Mail;
using System.Text.Json;
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

        public void SendAlert(string title, string message, string username, MailAddress email)
        {
            SendEmail(title, message, email);
            var relativePath = $"/{username}-alerts";

            if (_alertsServer is null || _alertsServer.WebSocketServices[relativePath] is null)
                return;

            var webSocketService = _alertsServer.WebSocketServices[relativePath];
            if (webSocketService is null || webSocketService.Sessions.Count <= 0)
                return;

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

        public void SendEmail(string title, string message, MailAddress email)
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
    }
}

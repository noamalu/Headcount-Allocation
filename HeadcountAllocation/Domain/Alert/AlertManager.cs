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

            try
            {
                var smtpClient = new SmtpClient("smtp.example.com") // Replace with your SMTP server
                {
                    Port = 587, // Replace with your SMTP port
                    Credentials = new NetworkCredential("headcount.allocation@gmail.com", "HeadcountAllocation1"), // Replace with your email credentials
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("headcount.allocation@gmail.com"), // Replace with your email address
                    Subject = title,
                    Body = message,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exceptions related to email sending
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace EcommerceAPI.initialize
{
    public static class WebSocketServerFactory
    {
        public static WebSocketServer CreateWebSocketServer()
        {
            // string port = configurate.Parse();
            WebSocketServer alertServer = new WebSocketServer($"ws://{GetLocalIPAddress()}:{4562}");
            return alertServer;
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }

}
using ProxiedTcpClient.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Filemail.ProxiedTcpClient
{
    public static class ProxiedTcpClient
    {
        /// <summary>
        /// Checks system proxy settings for given destination uri
        /// </summary>
        /// <param name="destination">Destination against which proxy settings are going to be checked</param>
        /// <returns>Proxy status</returns>
        public static ProxyStatus GetProxyStatusFor(Uri destination)
        {
            var proxy = WebRequest.DefaultWebProxy.GetProxy(destination);

            return new ProxyStatus
            {
                IsEnabled = proxy.Host != destination.Host,
                Uri = proxy
            };
        }

        /// <summary>
        /// Automatically detects system proxy settings and creates TcpClient that uses proxy if configured in the system
        /// </summary>
        /// <param name="destination">Destination address</param>
        /// <returns>TcpClient that use proxy if configured in the system</returns>
        public static TcpClient Create(Uri destination)
        {
            var proxyStatus = GetProxyStatusFor(destination);

            if (proxyStatus.IsEnabled)
                return CreateProxied(proxyStatus.Uri, destination);
            else
                return new TcpClient(destination.Host, destination.Port);
        }

        /// <summary>
        /// Create TcpClient that connects to the destination through proxy
        /// </summary>
        /// <param name="proxy">Proxy address</param>
        /// <param name="destination">Destination address</param>
        /// <returns>Tcp client connected through proxy</returns>
        public static TcpClient CreateProxied(Uri proxy, Uri destination)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(proxy.Host, proxy.Port);

            var connectMessage = Encoding.UTF8.GetBytes($"CONNECT {destination.Host}:{destination.Port} HTTP/1.1{Environment.NewLine}{Environment.NewLine}");
            socket.Send(connectMessage);

            byte[] receiveBuffer = new byte[1024];
            var received = socket.Receive(receiveBuffer);

            var response = ASCIIEncoding.ASCII.GetString(receiveBuffer, 0, received);

            if (response.IndexOf("200 OK", StringComparison.OrdinalIgnoreCase) == -1 && response.IndexOf("200 Connection Established", StringComparison.OrdinalIgnoreCase) == -1)
            {
                throw new Exception($"Error connecting to proxy server {destination.Host}:{destination.Port}. Response: {response}");
            }

            return new TcpClient
            {
                Client = socket
            };
        }
    }
}

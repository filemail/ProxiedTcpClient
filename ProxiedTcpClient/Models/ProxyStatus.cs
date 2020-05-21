using System;
using System.Collections.Generic;
using System.Text;

namespace ProxiedTcpClient.Models
{
    public class ProxyStatus
    {
        public bool IsEnabled { get; set; }
        public Uri Uri { get; set; }
    }
}

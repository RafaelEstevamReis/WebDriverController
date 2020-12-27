using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RafaelEstevam.WebDriverController.Proxy
{
    public class ProxyClient
    {
        string currentHost = null;
        public TcpClient Browser { get; }
        public TcpClient External { get; private set; }

        public ProxyClient(TcpClient browserSide)
        {
            Browser = browserSide;
        }

        public async Task ConnectTo(string host, int port)
        {
            if (External == null)
            {
                External = new TcpClient();
            }

            if (currentHost == host)
            {
                if (External.Connected) return;
            }

            currentHost = host;
            await External.ConnectAsync(host, port);
        }
    }
}

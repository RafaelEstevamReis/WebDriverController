using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RafaelEstevam.WebDriverController.Lib.Proxy
{
    public class ProxyServer
    {
        public int ListeningPort { get; }
        public TcpListener TcpListener { get; private set; } = null;

        CancellationTokenSource  tokenSource;
        List<ProxyClient> clients;

        public ProxyServer(int ListeningPort)
        {
            this.ListeningPort = ListeningPort;
            tokenSource = new CancellationTokenSource();
            clients = new List<ProxyClient>();
        }

        public void Start()
        {
            TcpListener = new TcpListener(new IPEndPoint(0, ListeningPort));
            TcpListener.Start();

            var tk = tokenSource.Token;
            tk.Register(TcpListener.Stop);
            // fire and forget
            Task.Run(() => mainLoop(tk));
        }

        public void Stop()
        {
            tokenSource.Cancel();
        }

        // fire and forget
        private async void mainLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await checkNewConnectionsAsync();
                await checkClientsListAsync();
                cleanClientsList();
            }
        }
        private async Task checkNewConnectionsAsync()
        {
            if (TcpListener.Pending())
            {
                TcpClient client = await TcpListener.AcceptTcpClientAsync();
                clients.Add(new ProxyClient(client));
            }
        }

        private void cleanClientsList()
        {
            var closed = clients.Where(c => !c.Browser.Connected)
                                .ToArray(); // force enumerate

            foreach (var c in closed)
            {
                clients.Remove(c);
            }
        }

        private async Task checkClientsListAsync()
        {
            foreach (var c in clients)
            {
                if (c == null) continue; // will be removed
                if(!c.Browser.Connected) continue;

                if (c.Browser.Available > 0)
                {
                    await processIncomingDataAsync(c);
                }
                if (c.External != null && c.External.Available > 0)
                {
                    await processOutgoingDataAsync(c);
                }
            }
        }

        private async Task processIncomingDataAsync(ProxyClient client)
        {
            var stream = client.Browser.GetStream();

            int len;
            byte[] buffer = new byte[1024];
            while (client.Browser.Available > 0 )
            {
                len = await stream.ReadAsync(buffer, 0, buffer.Length);
                // process GET and POST commands to get the "Host:" header
                await bufferHookAsync(client, buffer, len, true);
                
                await client.External.GetStream().WriteAsync(buffer, 0, len);
            }
        }

        private async Task processOutgoingDataAsync(ProxyClient client)
        {
            var stream = client.External.GetStream();

            int len;
            byte[] buffer = new byte[1024];
            while (client.External.Available > 0)
            {
                len = await stream.ReadAsync(buffer, 0, buffer.Length);

                await bufferHookAsync(client, buffer, len, false);

                await client.Browser.GetStream().WriteAsync(buffer, 0, len);
            }
        }

        private async Task bufferHookAsync(ProxyClient client, byte[] buffer, int len, bool fromBrowser)
        {
            if (!fromBrowser) return;

            if (buffer[0] == 'G' || buffer[0] == 'P')
            {
                var ascii = Encoding.ASCII.GetString(buffer, 0, len);
                // capture HOST
                foreach (var line in ascii.Split('\n'))
                {
                    if (!line.StartsWith("Host:")) continue;

                    string host = line.Split(':')[1].Trim();
                    await client.ConnectTo(host, 80);
                    break;
                }
            }

        }


    }
}

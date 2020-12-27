using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RafaelEstevam.WebDriverController.Proxy
{
    public class Tunnel
    {
        const int BlockSize = 4096;

        long dataBlockId = 0;

        private TcpListener tcpListener;
        private List<ProxyClient> clients;

        public delegate void DataCallback(TunnelDataArgs args);
        DataCallback dataCallback = null;

        public long TotalDownload { get; private set; } = 0;
        public long TotalUpload { get; private set; } = 0;

        public Tunnel(TcpListener tcpListener, List<ProxyClient> clients)
        {
            this.tcpListener = tcpListener;
            this.clients = clients;
        }

        public void Create(CancellationToken tk, DataCallback dataCallback)
        {
            this.dataCallback = dataCallback;
            Task.Run(() => mainLoop(tk));
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
            if (tcpListener.Pending())
            {
                TcpClient client = await tcpListener.AcceptTcpClientAsync();
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
                if (!c.Browser.Connected) continue;

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
            byte[] buffer = new byte[BlockSize];
            while (client.Browser.Available > 0)
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
            byte[] buffer = new byte[BlockSize];
            while (client.External.Available > 0)
            {
                len = await stream.ReadAsync(buffer, 0, buffer.Length);

                await bufferHookAsync(client, buffer, len, false);

                await client.Browser.GetStream().WriteAsync(buffer, 0, len);
            }
        }

        private async Task bufferHookAsync(ProxyClient client, byte[] buffer, int len, bool fromBrowser)
        {
            if (fromBrowser)
            {
                await detectHostHeaderAsync(client, buffer, len);
                TotalUpload += len;
            }
            else
            {
                TotalDownload += len;
            }

            if (dataCallback == null) return;

            var newId = Interlocked.Increment(ref dataBlockId);

            byte[] cropedData = new byte[len];
            Buffer.BlockCopy(buffer, 0, cropedData, 0, len);

            var data = new TunnelDataArgs()
            {
                PacketId = newId,
                FlowDirection = fromBrowser
                    ? TunnelDataArgs.Direction.FromBrowser
                    : TunnelDataArgs.Direction.ToBrowser,
                Timestamp = DateTime.Now,
                Data = cropedData,
            };
            // do not await
            dataCallBackFire(data);
        }
        private async void dataCallBackFire(TunnelDataArgs data)
        {
            await Task.Run(() => dataCallback(data));
        }

        private async Task detectHostHeaderAsync(ProxyClient client, byte[] buffer, int len)
        {
            if (buffer[0] == 'G' || buffer[0] == 'P') // Get/Post
            {
                var ascii = Encoding.ASCII.GetString(buffer, 0, len);
                // capture HOST
                foreach (var line in ascii.Split('\n'))
                {
                    if (!line.StartsWith("Host:")) continue;

                    string host = line.Split(':')[1].Trim();
                    await client.ConnectTo(host, 80);

                    return;
                }
            }
            if (buffer[0] == 'C')
            {
                var ascii = Encoding.ASCII.GetString(buffer, 0, len);
                /*
                     CONNECT accounts.google.com:443 HTTP/1.1
                     Host: accounts.google.com:443
                     Proxy-Connection: keep-alive
                     User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36
                 */
                var hostPair = ascii.Split(' ')[1].Trim().Split(':');
                if (!int.TryParse(hostPair[1], out int port)) port = 443;

                await client.ConnectTo(hostPair[0], port);
            }
        }
    }
}

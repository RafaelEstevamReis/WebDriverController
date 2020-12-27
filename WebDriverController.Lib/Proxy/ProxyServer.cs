using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RafaelEstevam.WebDriverController.Proxy
{
    public class ProxyServer
    {
        public event EventHandler<ProxyDataEventArgs> ProxyData;
        public event EventHandler<TunnelDataArgs> RawTunnelData;

        public int ListeningPort { get; }
        public TcpListener TcpListener { get; private set; } = null;

        CancellationTokenSource  tokenSource;
        List<ProxyClient> clients;
        private Tunnel tunnel;

        public long TotalDownload => tunnel.TotalDownload;
        public long TotalUpload => tunnel.TotalUpload;

        public ProxyServer(int ListeningPort)
        {
            this.ListeningPort = ListeningPort;
            tokenSource = new CancellationTokenSource();
            clients = new List<ProxyClient>();
        }

        public void Start()
        {
            var tk = tokenSource.Token;

            TcpListener = new TcpListener(new IPEndPoint(0, ListeningPort));
            TcpListener.Start();
            tk.Register(TcpListener.Stop);

            tunnel = new Tunnel(TcpListener, clients);

            tunnel.Create(tk, dataCallBack);
        }
        List<int> lstDataLen = new List<int>();
        private void dataCallBack(TunnelDataArgs args)
        {
            RawTunnelData?.Invoke(this, args);
            lstDataLen.Add(args.Data.Length);

            if (ProxyData == null) return;

            var hdrCollection = tryDecodeHttpHeader(args);

            if (hdrCollection != null)
            {
                ProxyHttptHeaderEventArgs header;

                if (args.FlowDirection == TunnelDataArgs.Direction.FromBrowser)
                {
                    header = new ProxyHttpRequestHeaderEventArgs()
                    {
                        Headers = hdrCollection,
                        PacketId = args.PacketId,
                        Timestamp = args.Timestamp,
                    };
                }
                else
                {
                    header = new ProxyHttpResponseHeaderEventArgs()
                    {
                        Headers = hdrCollection,
                        PacketId = args.PacketId,
                        Timestamp = args.Timestamp,
                    };
                }
                ProxyData(this, header);
            }
        }

        private HttpHeader tryDecodeHttpHeader(TunnelDataArgs args)
        {
            if (args.Data.Length < 5) return null;

            switch ((char)args.Data[0])
            {
                case 'G': // Get
                case 'P': // Post/Put
                case 'D': // Delete
                case 'F': // Fetch
                case 'H': // Http response
                    break;
                default:
                    return null;
            }
            switch ((char)args.Data[1])
            {
                case 'E': // Get/Fetch/Delete
                case 'e':
                case 'O': // Post
                case 'o':
                case 'U': // Put
                case 'u': 
                case 'T': // Http response
                    break;
                default:
                    return null;
            }

            using (var ms = new MemoryStream(args.Data))
            {
                var lst = new List<KeyValuePair<string, string>>();

                var sr = new StreamReader(ms, Encoding.ASCII);
                string line;
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    int idx = line.IndexOf(":");

                    if (idx <= 0)
                    {
                        lst.Add(new KeyValuePair<string, string>("", line));
                    }
                    else
                    {
                        lst.Add(new KeyValuePair<string, string>(line.Substring(0, idx), line.Substring(idx + 1)));
                    }
                }
                return HttpHeader.Build(lst);
            }
        }

        public void Stop()
        {
            tokenSource.Cancel();

            var avg = lstDataLen.Average();
            var min = lstDataLen.Min();
            var max = lstDataLen.Max();
        }

    }
}

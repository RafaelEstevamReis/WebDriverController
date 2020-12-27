using System;
using System.Collections.Specialized;

namespace RafaelEstevam.WebDriverController.Proxy
{
    public class ProxyDataEventArgs : EventArgs
    {
        public long PacketId { get; internal set; }
        public DateTime Timestamp { get; internal set; }
    }
    public class ProxyHttptHeaderEventArgs : ProxyDataEventArgs
    {
        public HttpHeader Headers { get; set; }
    }
    public class ProxyHttpRequestHeaderEventArgs : ProxyHttptHeaderEventArgs
    { }
    public class ProxyHttpResponseHeaderEventArgs : ProxyHttptHeaderEventArgs
    { }
}
using System;

namespace RafaelEstevam.WebDriverController.Proxy
{
    public class TunnelDataArgs
    {
        public enum Direction
        {
            FromBrowser,
            ToBrowser
        }

        public long PacketId { get; internal set; }
        public byte[] Data { get; internal set; }
        public Direction FlowDirection { get; internal set; }
        public DateTime Timestamp { get; internal set; }
    }
}
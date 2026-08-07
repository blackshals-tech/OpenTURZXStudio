using System;

namespace OpenTURZXStudio.Desktop.Models
{
    /// <summary>
    /// Representa um dispositivo conectado.
    /// </summary>
    public class DeviceModel
    {
        public string PortName { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public int BaudRate { get; set; } = 115200;
        public bool IsConnected { get; set; }
        public DateTime ConnectedAt { get; set; }
        public int TransfersCount { get; set; }
    }
}
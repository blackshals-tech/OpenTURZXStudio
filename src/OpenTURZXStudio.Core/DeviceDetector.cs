using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Linq;

namespace OpenTURZXStudio.Core
{
    /// <summary>
    /// Detecta e gerencia dispositivos USB e portas seriais disponíveis.
    /// </summary>
    public class DeviceDetector
    {
        private readonly Logger _logger;
        private List<string> _availablePorts = new();

        public event EventHandler<DeviceEventArgs>? DeviceConnected;
        public event EventHandler<DeviceEventArgs>? DeviceDisconnected;

        public DeviceDetector(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtém todas as portas seriais disponíveis.
        /// </summary>
        public List<string> GetAvailablePorts()
        {
            try
            {
                var ports = SerialPort.GetPortNames().ToList();
                _availablePorts = ports;
                _logger.Info($"Portas detectadas: {string.Join(", ", ports)}");
                return ports;
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao detectar portas seriais", ex);
                return new List<string>();
            }
        }

        /// <summary>
        /// Obtém informações detalhadas sobre dispositivos USB.
        /// </summary>
        public List<UsbDeviceInfo> GetUsbDevices()
        {
            var devices = new List<UsbDeviceInfo>();

            try
            {
                var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE ClassGuid='{36FC9E60-C465-11CF-8056-444553540000}'");

                foreach (var device in searcher.Get())
                {
                    devices.Add(new UsbDeviceInfo
                    {
                        Name = device["Name"]?.ToString() ?? "Unknown",
                        DeviceId = device["DeviceID"]?.ToString() ?? "Unknown",
                        Status = device["Status"]?.ToString() ?? "Unknown"
                    });
                }

                _logger.Info($"Dispositivos USB detectados: {devices.Count}");
            }
            catch (Exception ex)
            {
                _logger.Warning($"Erro ao detectar dispositivos USB: {ex.Message}");
            }

            return devices;
        }

        /// <summary>
        /// Verifica se uma porta específica está disponível.
        /// </summary>
        public bool IsPortAvailable(string portName)
        {
            return _availablePorts.Contains(portName);
        }

        protected virtual void OnDeviceConnected(DeviceEventArgs e)
        {
            DeviceConnected?.Invoke(this, e);
        }

        protected virtual void OnDeviceDisconnected(DeviceEventArgs e)
        {
            DeviceDisconnected?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Informações sobre um dispositivo USB.
    /// </summary>
    public class UsbDeviceInfo
    {
        public string Name { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// Argumentos de evento para dispositivos.
    /// </summary>
    public class DeviceEventArgs : EventArgs
    {
        public string DeviceName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
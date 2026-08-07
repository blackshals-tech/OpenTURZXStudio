using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OpenTURZXStudio.Core;
using OpenTURZXStudio.Desktop.Models;

namespace OpenTURZXStudio.Desktop.ViewModels
{
    /// <summary>
    /// ViewModel principal da aplicação.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly Logger _logger;
        private readonly DeviceDetector _deviceDetector;
        private readonly SerialManager _serialManager;
        private bool _isConnected;
        private string _statusMessage = "Desconectado";
        private int _detectedDevices;

        public ObservableCollection<string> AvailablePorts { get; } = new();
        public ObservableCollection<TransferModel> Transfers { get; } = new();
        public ObservableCollection<string> LogMessages { get; } = new();

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public int DetectedDevices
        {
            get => _detectedDevices;
            set => SetProperty(ref _detectedDevices, value);
        }

        public MainViewModel()
        {
            _logger = new Logger("logs/app.log");
            _deviceDetector = new DeviceDetector(_logger);
            _serialManager = new SerialManager(_logger);

            _logger.Info("Aplicação iniciada");
            RefreshDevices();
        }

        /// <summary>
        /// Atualiza a lista de dispositivos disponíveis.
        /// </summary>
        public void RefreshDevices()
        {
            try
            {
                AvailablePorts.Clear();
                var ports = _deviceDetector.GetAvailablePorts();
                foreach (var port in ports)
                {
                    AvailablePorts.Add(port);
                }

                var usbDevices = _deviceDetector.GetUsbDevices();
                DetectedDevices = ports.Count + usbDevices.Count;

                LogMessage($"[INFO] {ports.Count} portas seriais detectadas");
            }
            catch (Exception ex)
            {
                LogMessage($"[ERROR] Erro ao atualizar dispositivos: {ex.Message}");
                _logger.Error("Erro ao atualizar dispositivos", ex);
            }
        }

        /// <summary>
        /// Conecta a um dispositivo.
        /// </summary>
        public async Task ConnectAsync(string portName)
        {
            try
            {
                var success = await _serialManager.OpenAsync(portName);
                if (success)
                {
                    IsConnected = true;
                    StatusMessage = $"Conectado em {portName}";
                    LogMessage($"[INFO] Conectado em {portName}");
                }
                else
                {
                    StatusMessage = "Falha na conexão";
                    LogMessage("[ERROR] Falha ao conectar");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"[ERROR] {ex.Message}");
                _logger.Error("Erro ao conectar", ex);
            }
        }

        /// <summary>
        /// Desconecta do dispositivo.
        /// </summary>
        public void Disconnect()
        {
            _serialManager.Close();
            IsConnected = false;
            StatusMessage = "Desconectado";
            LogMessage("[INFO] Desconectado");
        }

        /// <summary>
        /// Adiciona uma mensagem ao log.
        /// </summary>
        public void LogMessage(string message)
        {
            LogMessages.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            if (LogMessages.Count > 1000) // Manter apenas últimas 1000 linhas
            {
                LogMessages.RemoveAt(0);
            }
        }

        /// <summary>
        /// Limpa o log.
        /// </summary>
        public void ClearLog()
        {
            LogMessages.Clear();
        }
    }

    /// <summary>
    /// Classe base para ViewModels com suporte a INotifyPropertyChanged.
    /// </summary>
    public class ViewModelBase
    {
        protected void SetProperty<T>(ref T backingField, T value, string propertyName = "")
        {
            if (!Equals(backingField, value))
            {
                backingField = value;
            }
        }
    }
}
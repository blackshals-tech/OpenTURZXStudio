using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace OpenTURZXStudio.Core
{
    /// <summary>
    /// Gerencia comunicação serial com dispositivos.
    /// </summary>
    public class SerialManager : IDisposable
    {
        private SerialPort? _serialPort;
        private readonly Logger _logger;
        private bool _isConnected = false;

        public event EventHandler<SerialEventArgs>? DataReceived;
        public event EventHandler<EventArgs>? ConnectionChanged;

        public bool IsConnected => _isConnected;

        public SerialManager(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Abre conexão com a porta serial.
        /// </summary>
        public async Task<bool> OpenAsync(string portName, int baudRate = 115200, int dataBits = 8, StopBits stopBits = StopBits.One, Parity parity = Parity.None)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
                    {
                        ReadTimeout = 1000,
                        WriteTimeout = 1000,
                        Handshake = Handshake.None
                    };

                    _serialPort.DataReceived += OnDataReceived;
                    _serialPort.Open();
                    _isConnected = true;

                    _logger.Info($"Conexão aberta: {portName} @ {baudRate}bps");
                    ConnectionChanged?.Invoke(this, EventArgs.Empty);

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.Error($"Erro ao abrir porta serial {portName}", ex);
                    _isConnected = false;
                    return false;
                }
            });
        }

        /// <summary>
        /// Fecha a conexão serial.
        /// </summary>
        public void Close()
        {
            if (_serialPort?.IsOpen == true)
            {
                try
                {
                    _serialPort.Close();
                    _isConnected = false;
                    _logger.Info("Conexão serial fechada");
                    ConnectionChanged?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    _logger.Error("Erro ao fechar porta serial", ex);
                }
            }
        }

        /// <summary>
        /// Envia dados pela porta serial.
        /// </summary>
        public async Task<bool> SendAsync(byte[] data)
        {
            if (!IsConnected || _serialPort == null)
            {
                _logger.Warning("Tentativa de envio sem conexão ativa");
                return false;
            }

            return await Task.Run(() =>
            {
                try
                {
                    _serialPort.Write(data, 0, data.Length);
                    _logger.Debug($"Dados enviados: {data.Length} bytes");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.Error("Erro ao enviar dados", ex);
                    return false;
                }
            });
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort == null) return;

            try
            {
                int bytesToRead = _serialPort.BytesToRead;
                byte[] buffer = new byte[bytesToRead];
                _serialPort.Read(buffer, 0, bytesToRead);

                _logger.Debug($"Dados recebidos: {bytesToRead} bytes");
                DataReceived?.Invoke(this, new SerialEventArgs { Data = buffer });
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao receber dados", ex);
            }
        }

        public void Dispose()
        {
            Close();
            _serialPort?.Dispose();
        }
    }

    /// <summary>
    /// Argumentos de evento para dados seriais.
    /// </summary>
    public class SerialEventArgs : EventArgs
    {
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }
}
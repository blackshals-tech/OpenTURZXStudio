using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace OpenTURZXStudio.Core
{
    /// <summary>
    /// Gerencia o envio de imagens e GIFs para dispositivos via protocolo USB.
    /// </summary>
    public class ImageSender
    {
        private readonly Logger _logger;
        private readonly SerialManager _serialManager;
        private readonly UsbProtocol _usbProtocol;
        private const int MAX_CHUNK_SIZE = 1024; // bytes

        public event EventHandler<TransferProgressEventArgs>? TransferProgress;
        public event EventHandler<EventArgs>? TransferCompleted;

        public ImageSender(Logger logger, SerialManager serialManager, UsbProtocol usbProtocol)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serialManager = serialManager ?? throw new ArgumentNullException(nameof(serialManager));
            _usbProtocol = usbProtocol ?? throw new ArgumentNullException(nameof(usbProtocol));
        }

        /// <summary>
        /// Envia uma imagem para o dispositivo.
        /// </summary>
        public async Task<bool> SendImageAsync(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                _logger.Error($"Arquivo de imagem não encontrado: {imagePath}");
                return false;
            }

            try
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                return await SendImageDataAsync(imageData);
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao enviar imagem", ex);
                return false;
            }
        }

        /// <summary>
        /// Envia dados de imagem para o dispositivo.
        /// </summary>
        public async Task<bool> SendImageDataAsync(byte[] imageData)
        {
            if (!_serialManager.IsConnected)
            {
                _logger.Warning("Dispositivo não conectado");
                return false;
            }

            try
            {
                int packetNumber = 0;
                int totalBytes = imageData.Length;
                int sentBytes = 0;

                while (sentBytes < totalBytes)
                {
                    int chunkSize = Math.Min(MAX_CHUNK_SIZE, totalBytes - sentBytes);
                    byte[] chunk = new byte[chunkSize];
                    Array.Copy(imageData, sentBytes, chunk, 0, chunkSize);

                    byte[] packet = _usbProtocol.CreateImagePacket(chunk, packetNumber);
                    bool success = await _serialManager.SendAsync(packet);

                    if (!success)
                    {
                        _logger.Error("Falha ao enviar pacote de imagem");
                        return false;
                    }

                    sentBytes += chunkSize;
                    packetNumber++;

                    OnTransferProgress(new TransferProgressEventArgs
                    {
                        BytesSent = sentBytes,
                        TotalBytes = totalBytes,
                        PercentComplete = (sentBytes * 100) / totalBytes
                    });

                    await Task.Delay(10); // Pequeno atraso para não sobrecarregar
                }

                _logger.Info($"Imagem enviada: {sentBytes} bytes");
                TransferCompleted?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao enviar dados de imagem", ex);
                return false;
            }
        }

        /// <summary>
        /// Envia um GIF frame por frame.
        /// </summary>
        public async Task<bool> SendGifAsync(GifData gifData)
        {
            if (!_serialManager.IsConnected)
            {
                _logger.Warning("Dispositivo não conectado");
                return false;
            }

            try
            {
                int totalFrames = gifData.Frames.Count;

                for (int i = 0; i < totalFrames; i++)
                {
                    byte[] frameData = gifData.Frames[i].ImageData;
                    byte[] packet = _usbProtocol.CreateGifPacket(frameData, i);

                    bool success = await _serialManager.SendAsync(packet);
                    if (!success)
                    {
                        _logger.Error($"Falha ao enviar frame {i}");
                        return false;
                    }

                    OnTransferProgress(new TransferProgressEventArgs
                    {
                        BytesSent = i + 1,
                        TotalBytes = totalFrames,
                        PercentComplete = ((i + 1) * 100) / totalFrames
                    });

                    await Task.Delay(gifData.Frames[i].Duration);
                }

                _logger.Info($"GIF enviado: {totalFrames} frames");
                TransferCompleted?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao enviar GIF", ex);
                return false;
            }
        }

        protected virtual void OnTransferProgress(TransferProgressEventArgs e)
        {
            TransferProgress?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Argumentos de evento para progresso de transferência.
    /// </summary>
    public class TransferProgressEventArgs : EventArgs
    {
        public int BytesSent { get; set; }
        public int TotalBytes { get; set; }
        public int PercentComplete { get; set; }
    }
}
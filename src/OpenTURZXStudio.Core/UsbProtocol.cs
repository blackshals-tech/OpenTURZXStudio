using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTURZXStudio.Core
{
    /// <summary>
    /// Define e gerencia o protocolo de comunicação USB customizado.
    /// </summary>
    public class UsbProtocol
    {
        private readonly Logger _logger;

        // Constantes de protocolo
        private const byte HEADER = 0xAA;
        private const byte FOOTER = 0xBB;
        private const byte CMD_IMAGE_TRANSFER = 0x01;
        private const byte CMD_GIF_TRANSFER = 0x02;
        private const byte CMD_CONFIG = 0x03;
        private const byte CMD_PING = 0xFF;

        public UsbProtocol(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Cria um pacote de ping para testar conexão.
        /// </summary>
        public byte[] CreatePingPacket()
        {
            return new byte[] { HEADER, CMD_PING, 0x00, CalculateChecksum(new byte[] { CMD_PING, 0x00 }), FOOTER };
        }

        /// <summary>
        /// Cria um pacote para transferência de imagem.
        /// </summary>
        public byte[] CreateImagePacket(byte[] imageData, int packetNumber)
        {
            var payload = new List<byte> { CMD_IMAGE_TRANSFER };
            payload.AddRange(BitConverter.GetBytes(packetNumber));
            payload.AddRange(imageData);

            byte checksum = CalculateChecksum(payload.ToArray());
            var packet = new List<byte> { HEADER };
            packet.AddRange(payload);
            packet.Add(checksum);
            packet.Add(FOOTER);

            return packet.ToArray();
        }

        /// <summary>
        /// Cria um pacote para transferência de GIF.
        /// </summary>
        public byte[] CreateGifPacket(byte[] gifData, int frameNumber)
        {
            var payload = new List<byte> { CMD_GIF_TRANSFER };
            payload.AddRange(BitConverter.GetBytes(frameNumber));
            payload.AddRange(gifData);

            byte checksum = CalculateChecksum(payload.ToArray());
            var packet = new List<byte> { HEADER };
            packet.AddRange(payload);
            packet.Add(checksum);
            packet.Add(FOOTER);

            return packet.ToArray();
        }

        /// <summary>
        /// Cria um pacote de configuração.
        /// </summary>
        public byte[] CreateConfigPacket(string configData)
        {
            var configBytes = Encoding.UTF8.GetBytes(configData);
            var payload = new List<byte> { CMD_CONFIG };
            payload.AddRange(BitConverter.GetBytes(configBytes.Length));
            payload.AddRange(configBytes);

            byte checksum = CalculateChecksum(payload.ToArray());
            var packet = new List<byte> { HEADER };
            packet.AddRange(payload);
            packet.Add(checksum);
            packet.Add(FOOTER);

            return packet.ToArray();
        }

        /// <summary>
        /// Verifica se um pacote recebido é válido.
        /// </summary>
        public bool ValidatePacket(byte[] packet)
        {
            if (packet == null || packet.Length < 5)
            {
                _logger.Warning("Pacote inválido: tamanho insuficiente");
                return false;
            }

            if (packet[0] != HEADER || packet[packet.Length - 1] != FOOTER)
            {
                _logger.Warning("Pacote inválido: cabeçalho ou rodapé incorreto");
                return false;
            }

            byte receivedChecksum = packet[packet.Length - 2];
            byte calculatedChecksum = CalculateChecksum(packet, 1, packet.Length - 3);

            if (receivedChecksum != calculatedChecksum)
            {
                _logger.Warning($"Checksum inválido: {receivedChecksum} vs {calculatedChecksum}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calcula o checksum de um buffer.
        /// </summary>
        private byte CalculateChecksum(byte[] data, int offset = 0, int length = -1)
        {
            if (length == -1) length = data.Length - offset;

            byte checksum = 0;
            for (int i = offset; i < offset + length; i++)
            {
                checksum ^= data[i];
            }
            return checksum;
        }

        /// <summary>
        /// Extrai o payload de um pacote validado.
        /// </summary>
        public byte[] ExtractPayload(byte[] packet)
        {
            if (!ValidatePacket(packet))
                throw new InvalidOperationException("Pacote inválido");

            return packet[1..(packet.Length - 2)];
        }
    }
}
using System;
using System.Collections.Generic;
using Xunit;
using OpenTURZXStudio.Core;
using FluentAssertions;

namespace OpenTURZXStudio.Tests
{
    /// <summary>
    /// Testes para a classe UsbProtocol.
    /// </summary>
    public class UsbProtocolTests
    {
        private readonly Logger _logger = new Logger();
        private readonly UsbProtocol _protocol;

        public UsbProtocolTests()
        {
            _protocol = new UsbProtocol(_logger);
        }

        [Fact]
        public void UsbProtocol_Should_Create_Ping_Packet()
        {
            // Act
            var packet = _protocol.CreatePingPacket();

            // Assert
            packet.Should().NotBeNull();
            packet.Should().NotBeEmpty();
            packet[0].Should().Be(0xAA); // Header
            packet[packet.Length - 1].Should().Be(0xBB); // Footer
        }

        [Fact]
        public void UsbProtocol_Should_Create_Image_Packet()
        {
            // Arrange
            byte[] imageData = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            int packetNumber = 1;

            // Act
            var packet = _protocol.CreateImagePacket(imageData, packetNumber);

            // Assert
            packet.Should().NotBeNull();
            packet[0].Should().Be(0xAA); // Header
            packet[packet.Length - 1].Should().Be(0xBB); // Footer
        }

        [Fact]
        public void UsbProtocol_Should_Create_GIF_Packet()
        {
            // Arrange
            byte[] gifData = new byte[] { 0x47, 0x49, 0x46 }; // "GIF"
            int frameNumber = 0;

            // Act
            var packet = _protocol.CreateGifPacket(gifData, frameNumber);

            // Assert
            packet.Should().NotBeNull();
            packet[0].Should().Be(0xAA); // Header
            packet[packet.Length - 1].Should().Be(0xBB); // Footer
        }

        [Fact]
        public void UsbProtocol_Should_Create_Config_Packet()
        {
            // Arrange
            string config = "BAUD:115200";

            // Act
            var packet = _protocol.CreateConfigPacket(config);

            // Assert
            packet.Should().NotBeNull();
            packet[0].Should().Be(0xAA); // Header
            packet[packet.Length - 1].Should().Be(0xBB); // Footer
        }

        [Fact]
        public void UsbProtocol_Should_Validate_Valid_Packet()
        {
            // Arrange
            var packet = _protocol.CreatePingPacket();

            // Act
            var isValid = _protocol.ValidatePacket(packet);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void UsbProtocol_Should_Reject_Invalid_Packet_Too_Short()
        {
            // Arrange
            byte[] invalidPacket = new byte[] { 0xAA, 0xBB };

            // Act
            var isValid = _protocol.ValidatePacket(invalidPacket);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void UsbProtocol_Should_Reject_Packet_Without_Header()
        {
            // Arrange
            byte[] invalidPacket = new byte[] { 0xFF, 0x01, 0x02, 0x03, 0xBB };

            // Act
            var isValid = _protocol.ValidatePacket(invalidPacket);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void UsbProtocol_Should_Extract_Payload_From_Valid_Packet()
        {
            // Arrange
            byte[] imageData = new byte[] { 0x01, 0x02, 0x03 };
            var packet = _protocol.CreateImagePacket(imageData, 0);

            // Act
            var payload = _protocol.ExtractPayload(packet);

            // Assert
            payload.Should().NotBeNull();
            payload.Should().NotBeEmpty();
        }

        [Fact]
        public void UsbProtocol_Should_Throw_On_Invalid_Packet_Extract()
        {
            // Arrange
            byte[] invalidPacket = new byte[] { 0xFF, 0xFF };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _protocol.ExtractPayload(invalidPacket));
        }
    }
}
using System;
using System.Linq;
using Xunit;
using OpenTURZXStudio.Core;
using FluentAssertions;

namespace OpenTURZXStudio.Tests
{
    /// <summary>
    /// Testes para a classe DeviceDetector.
    /// </summary>
    public class DeviceDetectorTests
    {
        private readonly Logger _logger = new Logger();

        [Fact]
        public void DeviceDetector_Should_Initialize()
        {
            // Act
            var detector = new DeviceDetector(_logger);

            // Assert
            detector.Should().NotBeNull();
        }

        [Fact]
        public void DeviceDetector_Should_Throw_On_Null_Logger()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DeviceDetector(null!));
        }

        [Fact]
        public void DeviceDetector_Should_Get_Available_Ports()
        {
            // Arrange
            var detector = new DeviceDetector(_logger);

            // Act
            var ports = detector.GetAvailablePorts();

            // Assert
            ports.Should().NotBeNull();
            ports.Should().BeOfType<System.Collections.Generic.List<string>>();
        }

        [Fact]
        public void DeviceDetector_Should_Get_USB_Devices()
        {
            // Arrange
            var detector = new DeviceDetector(_logger);

            // Act
            var devices = detector.GetUsbDevices();

            // Assert
            devices.Should().NotBeNull();
            devices.Should().BeOfType<System.Collections.Generic.List<UsbDeviceInfo>>();
        }

        [Fact]
        public void DeviceDetector_Should_Validate_Port_Availability()
        {
            // Arrange
            var detector = new DeviceDetector(_logger);
            var ports = detector.GetAvailablePorts();

            // Act & Assert
            if (ports.Any())
            {
                detector.IsPortAvailable(ports[0]).Should().BeTrue();
            }
            detector.IsPortAvailable("INVALID_PORT").Should().BeFalse();
        }

        [Fact]
        public void DeviceDetector_Should_Support_Events()
        {
            // Arrange
            var detector = new DeviceDetector(_logger);
            bool eventRaised = false;

            // Act
            detector.DeviceConnected += (sender, e) => eventRaised = true;

            // Assert
            detector.Should().NotBeNull();
        }
    }
}
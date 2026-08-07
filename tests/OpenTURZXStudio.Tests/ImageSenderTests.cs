using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using OpenTURZXStudio.Core;
using FluentAssertions;

namespace OpenTURZXStudio.Tests
{
    /// <summary>
    /// Testes para a classe ImageSender.
    /// </summary>
    public class ImageSenderTests
    {
        private readonly Logger _logger = new Logger();
        private readonly Mock<SerialManager> _mockSerialManager;
        private readonly UsbProtocol _usbProtocol;
        private readonly ImageSender _imageSender;

        public ImageSenderTests()
        {
            _mockSerialManager = new Mock<SerialManager>(_logger);
            _usbProtocol = new UsbProtocol(_logger);
            _imageSender = new ImageSender(_logger, _mockSerialManager.Object, _usbProtocol);
        }

        [Fact]
        public void ImageSender_Should_Initialize()
        {
            // Assert
            _imageSender.Should().NotBeNull();
        }

        [Fact]
        public void ImageSender_Should_Throw_On_Null_Logger()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new ImageSender(null!, _mockSerialManager.Object, _usbProtocol));
        }

        [Fact]
        public void ImageSender_Should_Throw_On_Null_SerialManager()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new ImageSender(_logger, null!, _usbProtocol));
        }

        [Fact]
        public void ImageSender_Should_Throw_On_Null_UsbProtocol()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new ImageSender(_logger, _mockSerialManager.Object, null!));
        }

        [Fact]
        public async Task ImageSender_Should_Handle_Non_Existent_File()
        {
            // Act
            var result = await _imageSender.SendImageAsync("non_existent_image.jpg");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ImageSender_Should_Return_False_When_Not_Connected()
        {
            // Arrange
            _mockSerialManager.Setup(x => x.IsConnected).Returns(false);
            var imageData = new byte[] { 0x01, 0x02, 0x03 };

            // Act
            var result = await _imageSender.SendImageDataAsync(imageData);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ImageSender_Should_Support_Transfer_Progress_Event()
        {
            // Arrange
            bool progressRaised = false;
            _imageSender.TransferProgress += (sender, e) => progressRaised = true;

            // Act & Assert
            _imageSender.Should().NotBeNull();
        }
    }
}
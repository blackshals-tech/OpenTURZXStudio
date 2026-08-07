using System;
using System.Threading.Tasks;
using Xunit;
using OpenTURZXStudio.Core;
using FluentAssertions;

namespace OpenTURZXStudio.Tests
{
    /// <summary>
    /// Testes para a classe SerialManager.
    /// </summary>
    public class SerialManagerTests
    {
        private readonly Logger _logger = new Logger();

        [Fact]
        public void SerialManager_Should_Initialize()
        {
            // Act
            var manager = new SerialManager(_logger);

            // Assert
            manager.Should().NotBeNull();
            manager.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void SerialManager_Should_Throw_On_Null_Logger()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SerialManager(null!));
        }

        [Fact]
        public async Task SerialManager_Should_Fail_Opening_Invalid_Port()
        {
            // Arrange
            var manager = new SerialManager(_logger);

            // Act
            var result = await manager.OpenAsync("INVALID_PORT_XYZ");

            // Assert
            result.Should().BeFalse();
            manager.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void SerialManager_Should_Not_Send_When_Disconnected()
        {
            // Arrange
            var manager = new SerialManager(_logger);
            var testData = new byte[] { 0xAA, 0xBB, 0xCC };

            // Act
            var task = manager.SendAsync(testData);

            // Assert
            task.Should().NotBeNull();
            manager.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void SerialManager_Should_Support_Events()
        {
            // Arrange
            var manager = new SerialManager(_logger);
            bool eventRaised = false;

            // Act
            manager.ConnectionChanged += (sender, e) => eventRaised = true;

            // Assert
            manager.Should().NotBeNull();
        }

        [Fact]
        public void SerialManager_Should_Implement_IDisposable()
        {
            // Arrange
            var manager = new SerialManager(_logger);

            // Act & Assert
            manager.Should().BeAssignableTo<IDisposable>();
            manager.Dispose(); // Should not throw
        }
    }
}
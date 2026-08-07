using System;
using Xunit;
using OpenTURZXStudio.Core;
using FluentAssertions;

namespace OpenTURZXStudio.Tests
{
    /// <summary>
    /// Testes para a classe Logger.
    /// </summary>
    public class LoggerTests
    {
        private readonly string _testLogPath = "test_logs/test.log";

        [Fact]
        public void Logger_Should_Create_Log_File()
        {
            // Arrange
            var logger = new Logger(_testLogPath);

            // Act
            logger.Info("Test message");

            // Assert
            System.IO.File.Exists(_testLogPath).Should().BeTrue();

            // Cleanup
            if (System.IO.File.Exists(_testLogPath))
                System.IO.File.Delete(_testLogPath);
        }

        [Fact]
        public void Logger_Should_Write_Info_Message()
        {
            // Arrange
            var logger = new Logger(_testLogPath);
            string message = "Info test message";

            // Act
            logger.Info(message);

            // Assert
            string logContent = System.IO.File.ReadAllText(_testLogPath);
            logContent.Should().Contain(message).And.Contain("INFO");

            // Cleanup
            if (System.IO.File.Exists(_testLogPath))
                System.IO.File.Delete(_testLogPath);
        }

        [Fact]
        public void Logger_Should_Write_Error_Message_With_Exception()
        {
            // Arrange
            var logger = new Logger(_testLogPath);
            var exception = new Exception("Test exception");

            // Act
            logger.Error("Error occurred", exception);

            // Assert
            string logContent = System.IO.File.ReadAllText(_testLogPath);
            logContent.Should().Contain("ERROR").And.Contain("Test exception");

            // Cleanup
            if (System.IO.File.Exists(_testLogPath))
                System.IO.File.Delete(_testLogPath);
        }

        [Fact]
        public void Logger_Should_Write_Warning_Message()
        {
            // Arrange
            var logger = new Logger(_testLogPath);

            // Act
            logger.Warning("Warning test message");

            // Assert
            string logContent = System.IO.File.ReadAllText(_testLogPath);
            logContent.Should().Contain("WARN");

            // Cleanup
            if (System.IO.File.Exists(_testLogPath))
                System.IO.File.Delete(_testLogPath);
        }

        [Fact]
        public void Logger_Should_Write_Debug_Message()
        {
            // Arrange
            var logger = new Logger(_testLogPath);

            // Act
            logger.Debug("Debug test message");

            // Assert
            string logContent = System.IO.File.ReadAllText(_testLogPath);
            logContent.Should().Contain("DEBUG");

            // Cleanup
            if (System.IO.File.Exists(_testLogPath))
                System.IO.File.Delete(_testLogPath);
        }

        [Fact]
        public void Logger_Should_Support_Disabling()
        {
            // Arrange
            var logger = new Logger(_testLogPath);
            logger.SetEnabled(false);

            // Act
            logger.Info("This should not be logged");

            // Assert
            bool fileExists = System.IO.File.Exists(_testLogPath);
            fileExists.Should().BeFalse();
        }
    }
}
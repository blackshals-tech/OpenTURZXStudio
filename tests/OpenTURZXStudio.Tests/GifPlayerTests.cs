using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using OpenTURZXStudio.Core;
using FluentAssertions;

namespace OpenTURZXStudio.Tests
{
    /// <summary>
    /// Testes para a classe GifPlayer.
    /// </summary>
    public class GifPlayerTests
    {
        private readonly Logger _logger = new Logger();

        [Fact]
        public void GifPlayer_Should_Initialize()
        {
            // Act
            var player = new GifPlayer(_logger);

            // Assert
            player.Should().NotBeNull();
            player.IsPlaying.Should().BeFalse();
            player.CurrentFrameIndex.Should().Be(0);
        }

        [Fact]
        public void GifPlayer_Should_Throw_On_Null_Logger()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new GifPlayer(null!));
        }

        [Fact]
        public void GifPlayer_Should_Load_GIF()
        {
            // Arrange
            var player = new GifPlayer(_logger);
            var gifData = new GifData
            {
                Width = 100,
                Height = 100,
                Frames = new List<GifFrame>
                {
                    new GifFrame { Duration = 100, ImageData = new byte[] { 0x01 } },
                    new GifFrame { Duration = 100, ImageData = new byte[] { 0x02 } }
                }
            };

            // Act
            player.LoadGif(gifData);

            // Assert
            player.CurrentFrameIndex.Should().Be(0);
        }

        [Fact]
        public void GifPlayer_Should_Throw_On_Null_GIF()
        {
            // Arrange
            var player = new GifPlayer(_logger);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => player.LoadGif(null!));
        }

        [Fact]
        public void GifPlayer_Should_Go_To_Frame()
        {
            // Arrange
            var player = new GifPlayer(_logger);
            var gifData = new GifData
            {
                Width = 100,
                Height = 100,
                Frames = new List<GifFrame>
                {
                    new GifFrame { Duration = 100, ImageData = new byte[] { 0x01 } },
                    new GifFrame { Duration = 100, ImageData = new byte[] { 0x02 } },
                    new GifFrame { Duration = 100, ImageData = new byte[] { 0x03 } }
                }
            };
            player.LoadGif(gifData);

            // Act
            player.GoToFrame(2);

            // Assert
            player.CurrentFrameIndex.Should().Be(2);
        }

        [Fact]
        public void GifPlayer_Should_Support_Events()
        {
            // Arrange
            var player = new GifPlayer(_logger);
            bool frameChangedRaised = false;

            // Act
            player.FrameChanged += (sender, e) => frameChangedRaised = true;

            // Assert
            player.Should().NotBeNull();
        }

        [Fact]
        public void GifPlayer_Should_Implement_IDisposable()
        {
            // Arrange
            var player = new GifPlayer(_logger);

            // Act & Assert
            player.Should().BeAssignableTo<IDisposable>();
            player.Dispose(); // Should not throw
        }
    }
}
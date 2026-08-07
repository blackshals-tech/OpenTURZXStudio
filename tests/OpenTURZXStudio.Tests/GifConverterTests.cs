using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using OpenTURZXStudio.Core;
using FluentAssertions;

namespace OpenTURZXStudio.Tests
{
    /// <summary>
    /// Testes para a classe GifConverter.
    /// </summary>
    public class GifConverterTests
    {
        private readonly Logger _logger = new Logger();
        private readonly GifConverter _converter;

        public GifConverterTests()
        {
            _converter = new GifConverter(_logger);
        }

        [Fact]
        public async Task GifConverter_Should_Handle_Non_Existent_File()
        {
            // Act
            var result = await _converter.LoadGifAsync("non_existent_file.gif");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GifConverter_Should_Create_GifData_Structure()
        {
            // Arrange
            var gifData = new GifData
            {
                Width = 100,
                Height = 100,
                Frames = new System.Collections.Generic.List<GifFrame>
                {
                    new GifFrame { Duration = 100, ImageData = new byte[] { 0x01, 0x02 } }
                }
            };

            // Act & Assert
            gifData.Should().NotBeNull();
            gifData.Width.Should().Be(100);
            gifData.Height.Should().Be(100);
            gifData.Frames.Should().HaveCount(1);
        }

        [Fact]
        public void GifConverter_Should_Create_GifFrame_With_Default_Duration()
        {
            // Arrange
            var frame = new GifFrame();

            // Act & Assert
            frame.Duration.Should().Be(100);
            frame.ImageData.Should().BeEmpty();
        }
    }
}
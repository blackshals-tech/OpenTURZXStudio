using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace OpenTURZXStudio.Core
{
    /// <summary>
    /// Converte e processa arquivos GIF para formato otimizado.
    /// </summary>
    public class GifConverter
    {
        private readonly Logger _logger;

        public GifConverter(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Carrega um arquivo GIF e retorna seus frames.
        /// </summary>
        public async Task<GifData?> LoadGifAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        _logger.Error($"Arquivo GIF não encontrado: {filePath}");
                        return null;
                    }

                    using (var image = Image.FromFile(filePath))
                    {
                        var gifData = new GifData
                        {
                            Width = image.Width,
                            Height = image.Height,
                            Frames = new List<GifFrame>()
                        };

                        var frameDimension = new FrameDimension(image.FrameDimensionsList[0]);
                        int frameCount = image.GetFrameCount(frameDimension);

                        for (int i = 0; i < frameCount; i++)
                        {
                            image.SelectActiveFrame(frameDimension, i);
                            
                            var bitmap = new Bitmap(image);
                            var frame = new GifFrame
                            {
                                ImageData = BitmapToByteArray(bitmap),
                                Duration = 100 // ms
                            };
                            
                            gifData.Frames.Add(frame);
                        }

                        _logger.Info($"GIF carregado: {frameCount} frames, {gifData.Width}x{gifData.Height}");
                        return gifData;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Erro ao carregar GIF: {filePath}", ex);
                    return null;
                }
            });
        }

        /// <summary>
        /// Redimensiona as frames do GIF.
        /// </summary>
        public GifData? ResizeGif(GifData gifData, int targetWidth, int targetHeight)
        {
            try
            {
                var resizedGif = new GifData
                {
                    Width = targetWidth,
                    Height = targetHeight,
                    Frames = new List<GifFrame>()
                };

                foreach (var frame in gifData.Frames)
                {
                    var bitmap = ByteArrayToBitmap(frame.ImageData, gifData.Width, gifData.Height);
                    var resized = new Bitmap(bitmap, targetWidth, targetHeight);
                    
                    resizedGif.Frames.Add(new GifFrame
                    {
                        ImageData = BitmapToByteArray(resized),
                        Duration = frame.Duration
                    });
                }

                _logger.Info($"GIF redimensionado para {targetWidth}x{targetHeight}");
                return resizedGif;
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao redimensionar GIF", ex);
                return null;
            }
        }

        /// <summary>
        /// Converte um Bitmap para array de bytes.
        /// </summary>
        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converte um array de bytes para Bitmap.
        /// </summary>
        private Bitmap ByteArrayToBitmap(byte[] data, int width, int height)
        {
            using (var ms = new MemoryStream(data))
            {
                return new Bitmap(Image.FromStream(ms));
            }
        }
    }

    /// <summary>
    /// Dados estruturados de um GIF.
    /// </summary>
    public class GifData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<GifFrame> Frames { get; set; } = new();
    }

    /// <summary>
    /// Representa um frame individual de um GIF.
    /// </summary>
    public class GifFrame
    {
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public int Duration { get; set; } = 100; // em ms
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace OpenTURZXStudio.Core
{
    /// <summary>
    /// Reproduz e controla animações GIF.
    /// </summary>
    public class GifPlayer : IDisposable
    {
        private readonly Logger _logger;
        private GifData? _currentGif;
        private CancellationTokenSource? _cancellationTokenSource;
        private int _currentFrameIndex = 0;
        private bool _isPlaying = false;

        public event EventHandler<FrameEventArgs>? FrameChanged;
        public event EventHandler<EventArgs>? PlaybackCompleted;

        public bool IsPlaying => _isPlaying;
        public int CurrentFrameIndex => _currentFrameIndex;

        public GifPlayer(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Carrega um GIF para reprodução.
        /// </summary>
        public void LoadGif(GifData gifData)
        {
            if (gifData == null)
                throw new ArgumentNullException(nameof(gifData));

            Stop();
            _currentGif = gifData;
            _currentFrameIndex = 0;
            _logger.Info($"GIF carregado: {gifData.Frames.Count} frames");
        }

        /// <summary>
        /// Inicia a reprodução do GIF.
        /// </summary>
        public async Task PlayAsync(bool loop = true)
        {
            if (_currentGif == null)
            {
                _logger.Warning("Tentativa de reproduzir sem GIF carregado");
                return;
            }

            if (_isPlaying)
                return;

            _isPlaying = true;
            _cancellationTokenSource = new CancellationTokenSource();

            await Task.Run(async () =>
            {
                try
                {
                    do
                    {
                        for (int i = 0; i < _currentGif.Frames.Count; i++)
                        {
                            if (_cancellationTokenSource.Token.IsCancellationRequested)
                                return;

                            _currentFrameIndex = i;
                            OnFrameChanged(new FrameEventArgs
                            {
                                FrameIndex = i,
                                TotalFrames = _currentGif.Frames.Count,
                                Frame = _currentGif.Frames[i]
                            });

                            await Task.Delay(_currentGif.Frames[i].Duration, _cancellationTokenSource.Token);
                        }
                    } while (loop && !_cancellationTokenSource.Token.IsCancellationRequested);

                    _logger.Info("Reprodução concluída");
                    PlaybackCompleted?.Invoke(this, EventArgs.Empty);
                }
                catch (OperationCanceledException)
                {
                    _logger.Debug("Reprodução cancelada");
                }
                catch (Exception ex)
                {
                    _logger.Error("Erro durante reprodução", ex);
                }
                finally
                {
                    _isPlaying = false;
                }
            }, _cancellationTokenSource.Token);
        }

        /// <summary>
        /// Pausa a reprodução.
        /// </summary>
        public void Pause()
        {
            if (_isPlaying)
            {
                _cancellationTokenSource?.Cancel();
                _isPlaying = false;
                _logger.Info("Reprodução pausada");
            }
        }

        /// <summary>
        /// Para a reprodução.
        /// </summary>
        public void Stop()
        {
            Pause();
            _currentFrameIndex = 0;
        }

        /// <summary>
        /// Vai para um frame específico.
        /// </summary>
        public void GoToFrame(int frameIndex)
        {
            if (_currentGif == null || frameIndex < 0 || frameIndex >= _currentGif.Frames.Count)
                return;

            _currentFrameIndex = frameIndex;
            OnFrameChanged(new FrameEventArgs
            {
                FrameIndex = frameIndex,
                TotalFrames = _currentGif.Frames.Count,
                Frame = _currentGif.Frames[frameIndex]
            });
        }

        protected virtual void OnFrameChanged(FrameEventArgs e)
        {
            FrameChanged?.Invoke(this, e);
        }

        public void Dispose()
        {
            Stop();
            _cancellationTokenSource?.Dispose();
        }
    }

    /// <summary>
    /// Argumentos de evento para mudança de frame.
    /// </summary>
    public class FrameEventArgs : EventArgs
    {
        public int FrameIndex { get; set; }
        public int TotalFrames { get; set; }
        public GifFrame Frame { get; set; } = new();
    }
}
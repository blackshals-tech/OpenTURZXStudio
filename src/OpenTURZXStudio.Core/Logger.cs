using System;
using System.IO;
using System.Text;

namespace OpenTURZXStudio.Core
{
    /// <summary>
    /// Gerenciador centralizado de logging para a aplicação.
    /// </summary>
    public class Logger
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();
        private bool _isEnabled = true;

        public Logger(string logFilePath = "logs/app.log")
        {
            _logFilePath = logFilePath;
            InitializeLogFile();
        }

        private void InitializeLogFile()
        {
            try
            {
                var directory = Path.GetDirectoryName(_logFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar arquivo de log: {ex.Message}");
            }
        }

        public void Info(string message)
        {
            Log("INFO", message);
        }

        public void Warning(string message)
        {
            Log("WARN", message);
        }

        public void Error(string message, Exception? ex = null)
        {
            var fullMessage = ex != null ? $"{message}\n{ex}" : message;
            Log("ERROR", fullMessage);
        }

        public void Debug(string message)
        {
            Log("DEBUG", message);
        }

        private void Log(string level, string message)
        {
            if (!_isEnabled) return;

            lock (_lockObject)
            {
                try
                {
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    var logLine = $"[{timestamp}] [{level}] {message}";

                    Console.WriteLine(logLine);

                    File.AppendAllText(_logFilePath, logLine + Environment.NewLine, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao gravar log: {ex.Message}");
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
        }
    }
}
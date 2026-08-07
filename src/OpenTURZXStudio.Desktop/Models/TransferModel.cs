using System;

namespace OpenTURZXStudio.Desktop.Models
{
    /// <summary>
    /// Representa uma transferência de arquivo.
    /// </summary>
    public class TransferModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ProgressPercent { get; set; }
        public TransferStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Status de uma transferência.
    /// </summary>
    public enum TransferStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Cancelled
    }
}
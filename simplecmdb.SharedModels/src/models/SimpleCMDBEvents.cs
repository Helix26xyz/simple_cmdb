namespace simplecmdb.SharedModels.models


{
    public class SimpleCMDBEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SimpleCMDBId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? Payload { get; set; }
        public required SimpleCMDB SimpleCMDB { get; set; }
        public SimpleCMDBEventStatus Status { get; set; }
        public SimpleCMDBEventSubStatus SubStatus { get; set; }
        public string? StatusResultText { get; set; }
    }

    public class SimpleCMDBEventWorkResponse
    {
        public SimpleCMDBEventSubStatus Status { get; set; }
        public string? ResultText { get; set; }
    }

    public class SimpleCMDBEventWorkResponseCollection{
        public int TotalCount { get; set; }
        public int ProcessedCount { get; set; }
        public int FailedCount { get; set; }
    }
    public enum SimpleCMDBEventStatus
    {
        New = 1,
        Received = 2,
        Processed = 3
    }

    public class SimpleCMDBEventStatusEntity
    {
        public int Id { get; set; }
        public SimpleCMDBEventStatus Status { get; set; }
    }

    public enum SimpleCMDBEventSubStatus
    {
        Pending = 0,
            Success = 1,
            Failed = 2,
            Retry = 3,
            Skipped = 4 
    }

    public class SimpleCMDBEventSubStatusEntity
    {
        public int Id { get; set; }
        public SimpleCMDBEventSubStatus Status { get; set; }
    }
}

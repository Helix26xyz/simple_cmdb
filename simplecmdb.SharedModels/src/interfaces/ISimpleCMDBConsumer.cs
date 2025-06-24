namespace simplecmdb.SharedModels.models
{
    public interface ISimpleCMDBConsumer
    {
        public Task<SimpleCMDBEventWorkResponse> ProcessSimpleCMDBEventAsync(SimpleCMDBEvent webhookEvent);
        public string Name { get; }
        public SimpleCMDB SimpleCMDB { get; }
        public string Script { get; }
    }
}

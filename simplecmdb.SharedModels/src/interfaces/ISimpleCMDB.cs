namespace simplecmdb.SharedModels.models
{
    public interface ISimpleCMDB
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Url { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Owner { get; set; }
        public string Project { get; set; }

        public SimpleCMDBStatus Status{ get; set; }
    }
}

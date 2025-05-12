namespace NoteMicroservice.Identity.Domain.Configurations
{
    public class DatabaseSetting
    {
        public string ConnectionString { get; set; }
        public bool ApplyMigration { get; set; }
        public bool ApplySeeding { get; set; }
    }
}

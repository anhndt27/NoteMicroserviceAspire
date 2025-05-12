namespace NoteMicroservice.Identity.Domain.Configurations;

public static class ConfigManager
{
    public static DatabaseSetting DatabaseSetting { get; private set; }
    public static RabbitMqSetting RabbitMqSetting { get; set; }

    public static void LoadConfig(IConfiguration configuration)
    {
        DatabaseSetting = configuration.GetSection("DatabaseSetting").Get<DatabaseSetting>();
        RabbitMqSetting = configuration.GetSection("RabbitMqSetting").Get<RabbitMqSetting>();
    }
}
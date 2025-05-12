namespace NoteMicroservice.Identity.Domain.Configurations;

public record RabbitMqSetting
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string VHost { get; set; }
    public string User { get; set; }
    public string Pass { get; set; }
    public bool UserSsl { get; set; }
}
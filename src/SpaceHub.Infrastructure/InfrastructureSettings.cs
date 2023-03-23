namespace SpaceHub.Infrastructure;

public class InfrastructureSettings
{
    public ApisSettings Api { get; set; } = new();
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public string DatabaseName { get; set; } = string.Empty;
    public bool HangfireEnabled { get; set; }
}

public class ApisSettings
{
    public ApiSettings Article { get; set; } = new();
    public ApiSettings Launch { get; set; } = new();
}

public class ApiSettings
{
    public string BaseAddress { get; set; } = string.Empty;
}

public class ConnectionStrings
{
    public string MongoDB { get; set; } = string.Empty;
}

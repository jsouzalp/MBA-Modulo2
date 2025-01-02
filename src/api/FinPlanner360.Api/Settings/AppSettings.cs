﻿namespace FinPlanner360.Api.Settings;

public sealed class AppSettings
{
    public JwtSettings JwtSettings { get; set; }
    public DatabaseSettings DatabaseSettings { get; set; }
}

public sealed class DatabaseSettings
{
    public string ConnectionStringFinPlanner360 { get; set; }
    public string ConnectionStringIdentity { get; set; }
}

public sealed class JwtSettings
{
    public string Secret { get; set; }
    public int ExpirationInHours { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}

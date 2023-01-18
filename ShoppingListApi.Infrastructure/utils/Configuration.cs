namespace ShoppingListApi.Infrastructure.utils;

public static class Configuration
{
    public static string GetDbConnectionString()
    {
        var envVariables = Environment.GetEnvironmentVariables();
        var host = envVariables["SECRET_HOST"];
        var port = envVariables["SECRET_PORT"];
        var userName = envVariables["SECRET_USERNAME"];
        var password = envVariables["SECRET_PASSWORD"];
        var dbName = envVariables["SECRET_DB_NAME"];
        
        return $"Server={host};Port={port};Database={dbName};User Id={userName};Password={password};";
    }
}

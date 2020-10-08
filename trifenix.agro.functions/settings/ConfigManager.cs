using System;

using trifenix.connect.db;

namespace trifenix.agro.functions.settings
{
    public static class ConfigManager
    {

        public static AzureSettings GetSettings => new AzureSettings {
            CosmosDbName = Environment.GetEnvironmentVariable("CosmosDbName", EnvironmentVariableTarget.Process),
            CosmosDbPrimaryKey = Environment.GetEnvironmentVariable("CosmosDbPrimaryKey", EnvironmentVariableTarget.Process),
            CosmosDbUri = Environment.GetEnvironmentVariable("CosmosDbUri", EnvironmentVariableTarget.Process),
            AzureSearchKey = Environment.GetEnvironmentVariable("AzureSearchKey", EnvironmentVariableTarget.Process),
            AzureSearchName = Environment.GetEnvironmentVariable("AzureSearchName", EnvironmentVariableTarget.Process),

        };

        public static CosmosDbArguments GetDbArguments => new CosmosDbArguments
        {
            EndPointUrl = GetSettings.CosmosDbUri,
            NameDb = GetSettings.CosmosDbName,
            PrimaryKey = GetSettings.CosmosDbPrimaryKey,
           
        };

    }
}

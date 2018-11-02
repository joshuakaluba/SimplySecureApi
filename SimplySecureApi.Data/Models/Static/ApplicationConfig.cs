using System;

namespace SimplySecureApi.Data.Models.Static
{
    public static class ApplicationConfig
    {
        public static string Port
            = Environment.GetEnvironmentVariable
                ("SIMPLY_SECURE_API_APPLICATION_PORT", target: EnvironmentVariableTarget.Process);

        internal static string DatabaseName
            = Environment.GetEnvironmentVariable
                ("SIMPLY_SECURE_API_DB_NAME", target: EnvironmentVariableTarget.Process);

        internal static string DatabaseUser
            = Environment.GetEnvironmentVariable
                ("SIMPLY_SECURE_API_DB_USER", target: EnvironmentVariableTarget.Process);

        internal static string DatabasePassword
            = Environment.GetEnvironmentVariable
                ("SIMPLY_SECURE_API_DB_PASSWORD", target: EnvironmentVariableTarget.Process);

        internal static string DatabaseHost
            = Environment.GetEnvironmentVariable
                ("SIMPLY_SECURE_API_DB_HOST", target: EnvironmentVariableTarget.Process);

        public static string JwtTokenKey
            = Environment.GetEnvironmentVariable
                ("SIMPLY_SECURE_API_JWT_KEY", target: EnvironmentVariableTarget.Process);
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplySecureApi.Data.Models.Static
{
    public static class ApplicationKeys
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
    }
}

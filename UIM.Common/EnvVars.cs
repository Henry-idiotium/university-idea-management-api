using System;

namespace UIM.Common
{
    public static class EnvVars
    {
        public static class AspNetCore
        {
            public static string Environment => GetEnvVar("ASPNETCORE_ENVIRONMENT");
        }

        public static class Auth
        {
            public static string ValidLocations => GetEnvVar("AUTH_VALID_LOCATIONS");
        }

        public static class Jwt
        {
            public static string AccessExpiredDate => GetEnvVar("JWT_ACCESS_EXPIRED_DATE");
            public static string Audience => GetEnvVar("JWT_AUDIENCE");
            public static string Issuer => GetEnvVar("JWT_ISSUER");
            public static string RefreshExpiredDate => GetEnvVar("JWT_REFRESH_EXPIRED_DATE");
            public static string Secret => GetEnvVar("JWT_SECRET");
        }

        public static class Pgsql
        {
            public static string Db => GetEnvVar("PGSQL_DB");
            public static string Host => GetEnvVar("PGSQL_HOST");
            public static string IntegratedSecurity => GetEnvVar("PGSQL_INTEGRATED_SECURITY");
            public static string Password => GetEnvVar("PGSQL_PASSWORD");
            public static string Pooling => GetEnvVar("PGSQL_POOLING");
            public static string Port => GetEnvVar("PGSQL_PORT");
            public static string SslMode => GetEnvVar("PGSQL_SSL_MODE");
            public static string TrustServer => GetEnvVar("PGSQL_TRUST_SERVER_CERTIFICATE");
            public static string UserId => GetEnvVar("PGSQL_USER_ID");
        }

        public static class SocialAuth
        {
            public static string GoogleClientId => GetEnvVar("AUTH_GOOGLE_CLIENT_ID");
        }

        private static string GetEnvVar(string variable) => Environment.GetEnvironmentVariable(variable);
    }
}
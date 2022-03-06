using System;

namespace UIM.Core.Helpers
{
    public static class EnvVars
    {
        public static string CoreEnv => GetEnvVar("ASPNETCORE_ENVIRONMENT").ToLower();

        public static bool DisableInitRolePwrUser =>
            bool.Parse(GetEnvVar("SYSTEM_INITIALIZE_ROLE_PWRUSER_DISABLE"));

        public static string[] ValidLocations => GetEnvVar("AUTH_VALID_LOCATIONS").Split(';');

        private static string GetEnvVar(string variable) => Environment.GetEnvironmentVariable(variable);

        public static class Jwt
        {
            public static int AccessExpiredDate => int.Parse(GetEnvVar("JWT_ACCESS_EXPIRED_DATE"));
            public static string Audience => GetEnvVar("JWT_AUDIENCE");
            public static string Issuer => GetEnvVar("JWT_ISSUER");
            public static int RefreshExpiredDate => int.Parse(GetEnvVar("JWT_REFRESH_EXPIRED_DATE"));
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

        public static class System
        {
            public static class PwrUserAuth
            {
                public static string Email => GetEnvVar("AUTH_PWDUSER_EMAIL");
                public static string Password => GetEnvVar("AUTH_PWDUSER_PASSWORD");
                public static string UserName => GetEnvVar("AUTH_PWDUSER_USERNAME");
            }

            public static class Role
            {
                public static string Manager => GetEnvVar("SYSTEM_ROLE_MANAGER");
                public static string PwrUser => GetEnvVar("SYSTEM_ROLE_PWRUSER");
                public static string Staff => GetEnvVar("SYSTEM_ROLE_STAFF");
                public static string Supervisor => GetEnvVar("SYSTEM_ROLE_SUP");
            }
        }
    }
}
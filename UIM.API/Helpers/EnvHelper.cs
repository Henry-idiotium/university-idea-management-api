using System;

namespace UIM.API.Helpers
{
    public static class EnvHelpers
    {
        public static string GetEnvVar(string variable) => Environment.GetEnvironmentVariable(variable);
    }
}
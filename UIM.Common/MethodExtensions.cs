using System;

namespace UIM.Common
{
    public static class MethodExtensions
    {
        public static string ToString(this DateTime? dt, string format) =>
            dt != null ? ((DateTime)dt).ToString(format) : null;
    }
}
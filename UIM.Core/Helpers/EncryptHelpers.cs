namespace UIM.Core.Helpers;

public static class EncryptHelpers
{
    public static string EncodeBase64Url(string? sequence) =>
        sequence != null ? WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(sequence))
                         : string.Empty;

    public static string DecodeBase64Url(string? sequence) =>
        sequence != null ? Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(sequence))
                         : string.Empty;

    public static byte[] EncodeASCII(string? sequence) =>
        sequence != null ? Encoding.ASCII.GetBytes(sequence)
                         : System.Array.Empty<byte>();
}
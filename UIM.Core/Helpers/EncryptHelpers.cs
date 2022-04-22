using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace UIM.Core.Helpers;

public static class EncryptHelpers
{
    public static byte[] EncodeASCII(string? sequence) =>
        sequence != null ? Encoding.ASCII.GetBytes(sequence) : Array.Empty<byte>();

    public static string DecodeBase64Url(string? sequence) =>
        sequence != null
            ? Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(sequence))
            : string.Empty;

    public static string EncodeBase64Url(string? sequence) =>
        sequence != null
            ? WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(sequence))
            : string.Empty;

    public static string EncryptRsa(string text)
    {
        var encryptedBytes = GetRSAPublicKey().Encrypt(Encoding.UTF8.GetBytes(text), false);
        return Convert.ToBase64String(encryptedBytes);
    }

    public static string DecryptRsa(string encrypted)
    {
        var decryptedBytes = GetRSAPrivateKey().Decrypt(Convert.FromBase64String(encrypted), false);
        return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
    }

    private static RSACryptoServiceProvider GetRSAPublicKey()
    {
        var textReader = new StringReader(EnvVars.Rsa.PublicKey);
        var publicKeyParam = (RsaKeyParameters)new PemReader(textReader).ReadObject();
        var rsaParam = DotNetUtilities.ToRSAParameters(publicKeyParam);
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(rsaParam);
        return csp;
    }

    private static RSACryptoServiceProvider GetRSAPrivateKey()
    {
        var textReader = new StringReader(EnvVars.Rsa.PrivateKey);
        var readKeyPair = (AsymmetricCipherKeyPair)new PemReader(textReader).ReadObject();
        var rsaParam = DotNetUtilities.ToRSAParameters(
            (RsaPrivateCrtKeyParameters)readKeyPair.Private
        );
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(rsaParam);
        return csp;
    }
}

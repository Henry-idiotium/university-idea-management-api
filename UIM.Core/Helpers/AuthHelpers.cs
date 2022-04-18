namespace UIM.Core.Helpers;

public static class AuthHelpers
{
    private static readonly char[] punctuations = "!@#$%^&*()_-+=[{]};:>|./\\?".ToCharArray();

    public static string GeneratePasswordWithNonAlphanumericCharacters(
        int length,
        bool isFirstLetterCapital = false,
        int numberOfNonAlphanumericCharacters = 0
    )
    {
        if (length < 1 || length > 128)
            throw new ArgumentException(null, nameof(length));

        if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            throw new ArgumentException(null, nameof(numberOfNonAlphanumericCharacters));

        using var rng = RandomNumberGenerator.Create();
        var byteBuffer = new byte[length];

        rng.GetBytes(byteBuffer);

        var count = 0;
        var characterBuffer = new char[length];

        for (var iter = 0; iter < length; iter++)
        {
            var i = byteBuffer[iter] % 87;

            if (iter == 0 && isFirstLetterCapital)
            {
                characterBuffer[iter] = (char)(
                    'A' + i
                    - (
                        (i > 10)
                            ? (i > 36)
                                ? (i > 62)
                                    ? 62
                                    : 36
                                : 10
                            : i
                    )
                );
                continue;
            }

            if (i < 10)
                characterBuffer[iter] = (char)('0' + i);
            else if (i < 36)
                characterBuffer[iter] = (char)('A' + i - 10);
            else if (i < 62)
                characterBuffer[iter] = (char)('a' + i - 36);
            else
            {
                characterBuffer[iter] = punctuations[i - 62];
                count++;
            }
        }

        if (count >= numberOfNonAlphanumericCharacters)
            return new string(characterBuffer);

        int j;
        var rand = new Random();

        for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
        {
            int k;

            do k = rand.Next(0, length);
            while (!char.IsLetterOrDigit(characterBuffer[k]));

            characterBuffer[k] = punctuations[rand.Next(0, punctuations.Length)];
        }

        return new string(characterBuffer);
    }

    public static string GeneratePassword(int length, bool isFirstLetterCapital = false)
    {
        var chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789123456789123456789";
        var charBuffer = new char[length];
        var random = new Random(Guid.NewGuid().GetHashCode());

        for (int i = 0; i < charBuffer.Length; i++)
        {
            if (i == 0 && isFirstLetterCapital)
            {
                var upperChars = chars[26..51];
                charBuffer[i] = upperChars[random.Next(upperChars.Length)];
            }
            else
                charBuffer[i] = chars[random.Next(chars.Length)];
        }

        return new(charBuffer);
    }
}

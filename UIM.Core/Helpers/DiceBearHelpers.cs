using RestSharp;

namespace UIM.Core.Helpers;

public static class DiceBearHelpers
{
    public static async Task<string> GetAvatarAsync(string? sprite = null)
    {
        if (sprite.IsNullOrEmpty())
        {
            var sprites = new string[]
            {
                Sprites.Adventurer,
                Sprites.BigEars,
                Sprites.BigSmile,
                Sprites.Bottts,
                Sprites.Croodles,
                Sprites.Jdenticon,
                Sprites.Micah,
                Sprites.Miniavs,
                Sprites.OpenPeeps,
            };
            int index = new Random().Next(sprites.Length);
            sprite = sprites[index];
        }
        var client = new RestClient(DiceBearUrl(sprite ?? Sprites.Jdenticon));
        var request = new RestRequest();

        var response = await client.ExecuteAsync(request);
        if (response.Content == null)
            throw new HttpException(
                HttpStatusCode.InternalServerError,
                "Unable to get user avatar !!"
            );
        return response.Content;
    }

    private static string DiceBearUrl(string sprite)
    {
        var seed = Guid.NewGuid();
        return $"https://avatars.dicebear.com/api/{sprite}/{seed}.svg";
    }
}

public class Sprites
{
    public const string Adventurer = "adventurer";
    public const string BigEars = "big-ears";
    public const string BigSmile = "big-smile";
    public const string Bottts = "bottts";
    public const string Croodles = "croodles";
    public const string Jdenticon = "jdenticon";
    public const string Micah = "micah";
    public const string Miniavs = "miniavs";
    public const string OpenPeeps = "open-peeps";
}

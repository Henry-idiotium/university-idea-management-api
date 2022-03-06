using UIM.Core.Common;

namespace UIM.Core.Models.Dtos.User
{
    public class UserDetailsResponse : UserDto, IResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
    }
}
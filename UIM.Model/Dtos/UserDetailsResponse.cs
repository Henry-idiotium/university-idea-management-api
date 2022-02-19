namespace UIM.Model.Dtos
{
    public class UserDetailsResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public string DateOfBirth { get; set; }
    }
}
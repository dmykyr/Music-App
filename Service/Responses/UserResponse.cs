namespace MusicApp.Service.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public Guid RoleId { get; set; }

        public string Nickname { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

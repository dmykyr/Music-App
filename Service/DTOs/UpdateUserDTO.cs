using System.ComponentModel.DataAnnotations;

namespace MusicService.DTOs
{
    public class UpdateUserDTO
    {

        [MaxLength(50)]
        public string? Nickname { get; set; }

        [MaxLength(50)]
        public string? Login { get; set; }

        [MaxLength(50)]
        public string? Password { get; set; }
    }
}

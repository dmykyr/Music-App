using System.ComponentModel.DataAnnotations;

namespace MusicService.DTOs
{
    public class CreateSongDTO
    {
        [MaxLength(50)]
        public string Title { get; set; }

        public string Base64Track { get; set; }

        public string Base64Image { get; set; }
    }
}

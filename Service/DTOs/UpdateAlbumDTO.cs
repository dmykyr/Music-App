using System.ComponentModel.DataAnnotations;

namespace MusicService.DTOs
{
    public class UpdateAlbumDTO
    {
        [MaxLength(50)]
        public string? Title { get; set; }

        public string? Base64Image { get; set; }

        public DateTime? PublishingDate { get; set; }
    }
}

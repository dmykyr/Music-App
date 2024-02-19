using System.ComponentModel.DataAnnotations;

namespace MusicService.DTO
{
    public class ArtistDTO
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

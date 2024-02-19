namespace MusicApp.Service.Responses
{
    public class AlbumResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Base64Image { get; set; }

        public DateTime PublishingDate { get; set; }
    }
}

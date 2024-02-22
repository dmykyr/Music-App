using System.Windows.Media;

namespace MusicApp.Views
{
    public class SongItemView
    {
        public ImageSource Source { get; set; }

        public string Title { get; set; }

        public string AlbumTitle { get; set; }

        public DateTime PublishingDate { get; set; }
    }
}

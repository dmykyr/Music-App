using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MusicApp.Service.Responses;
using MusicApp.Service;

namespace MusicApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpServiceClient _client;
        public MainWindow()
        {
            InitializeComponent();
            _client = new HttpServiceClient();
        }

        public static ImageSource ConvertBase64ToImage(string base64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);

            using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
            {
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public class MusicItem
        {
            public ImageSource ImagePath { get; set; }
            public string Name { get; set; }

            public MusicItem(string base64String, string name)
            {
                ImagePath = ConvertBase64ToImage(base64String);
                Name = name;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var favArtists = (await _client.GetAsync<IEnumerable<ArtistResponse>>("artists")).ToList();
            List<MusicItem> musicItems = new List<MusicItem>();
            foreach (var artist in favArtists)
            {
                musicItems.Add(new MusicItem(artist.Base64Image, artist.Name));
            }

            lvFavorite.ItemsSource = musicItems;
        }
    }
}
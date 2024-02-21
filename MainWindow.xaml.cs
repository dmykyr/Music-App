using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MusicApp.Service.Responses;
using MusicApp.Service;
using MusicApp.Views;
using System.Windows.Controls;

namespace MusicApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly float scaleCoef = 2.4F;
        private string _addedToFavorite = "✓";
        private string _notAddedToFavorite = "+";
        private string _playMusic = "▶";
        private string _stopMusic = "⏸";



        private HttpServiceClient _client;

        private ImageSource _currentSongImage;
        public MainWindow()
        {
            InitializeComponent();
            _client = new HttpServiceClient();

            _currentSongImage = new BitmapImage();
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

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var favArtists = (await _client.GetAsync<IEnumerable<ArtistResponse>>("artists")).ToList();
        //    List<MusicItem> musicItems = new List<MusicItem>();
        //    foreach (var artist in favArtists)
        //    {
        //        musicItems.Add(new MusicItem(artist.Base64Image, artist.Name));
        //    }

        //    lvFavorite.ItemsSource = musicItems;
        //}

        private async void tbHome_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            searchPage.Visibility = Visibility.Hidden;
            allItemsPage.Visibility = Visibility.Visible;
            albumPage.Visibility = Visibility.Hidden;

            try
            {
                await ShowAllDbAlbums();
            }
            catch (Exception ex) { }
        }

        private void tbSearch_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            searchPage.Visibility = Visibility.Visible;
            allItemsPage.Visibility = Visibility.Hidden;
            albumPage.Visibility = Visibility.Hidden;
        }

        private async Task ShowAllDbAlbums()
        {
            IEnumerable<AlbumResponse> dbAlbums = (await _client.GetAsync<IEnumerable<AlbumResponse>>("albums"))
                ?? throw new Exception();

            AllItemsView allItemsView = new AllItemsView() { ItemsCategory = "Albums"};
            List<GridItemView> gridItems = dbAlbums.Select(album => new GridItemView
            {
                Title = album.Title,
                Source = ConvertBase64ToImage(album.Base64Image)
            }).ToList();

            List<UIElement> items = ConvertToGridItems(gridItems);
            AddItemsToGrid(allItemsGrid, items);
            tb.DataContext = allItemsView;
        }

        public void AddItemsToGrid(Grid grid, List<UIElement> items)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            int rowCount = items.Count / 4 + 1;
            for(int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < items.Count; i++)
            {
                Grid.SetRow(items[i], i / grid.ColumnDefinitions.Count);
                Grid.SetColumn(items[i], i % grid.ColumnDefinitions.Count);

                grid.Children.Add(items[i]);
            }
        }

        private async Task ShowSearchedItems(string searchingEntity)
        {
            List<GridItemView> gridItems = new List<GridItemView>();
            switch (searchingEntity)
            {
                case "Album":  
                    {
                        IEnumerable<AlbumResponse> dbAlbums = (await _client
                                    .GetAsync<IEnumerable<AlbumResponse>>($"albums?searchName={tbSearchPattern.Text}"))
                           ?? throw new Exception();

                        gridItems.AddRange(dbAlbums.Select(album => new GridItemView
                        {
                            Title = album.Title,
                            Source = ConvertBase64ToImage(album.Base64Image)
                        }).ToList());
                        break;
                    }
                case "Artist":
                    {
                        IEnumerable<ArtistResponse> dbArtists = (await _client
                                    .GetAsync<IEnumerable<ArtistResponse>>($"artists?searchName={tbSearchPattern.Text}"))
                           ?? throw new Exception();

                        gridItems.AddRange(dbArtists.Select(artist => new GridItemView
                        {
                            Title = artist.Name,
                            Source = ConvertBase64ToImage(artist.Base64Image)
                        }).ToList());
                        break;
                    }
                case "Song":
                    {
                        IEnumerable<SongResponse> dbSongs = (await _client
                                    .GetAsync<IEnumerable<SongResponse>>($"songs?searchName={tbSearchPattern.Text}"))
                           ?? throw new Exception();

                        gridItems.AddRange(dbSongs.Select(song => new GridItemView
                        {
                            Title = song.Title,
                            Source = ConvertBase64ToImage(Convert.ToBase64String(song.Image))
                        }).ToList());
                        break;
                    }
                case "All":
                    {
                        IEnumerable<SongResponse> dbSongs = (await _client
                                    .GetAsync<IEnumerable<SongResponse>>($"songs?searchName={tbSearchPattern.Text}"))
                           ?? throw new Exception();
                        IEnumerable<ArtistResponse> dbArtists = (await _client
                                    .GetAsync<IEnumerable<ArtistResponse>>($"artists?searchName={tbSearchPattern.Text}"))
                           ?? throw new Exception();
                        IEnumerable<AlbumResponse> dbAlbums = (await _client
                                   .GetAsync<IEnumerable<AlbumResponse>>($"albums?searchName={tbSearchPattern.Text}"))
                          ?? throw new Exception();

                        gridItems.AddRange(dbSongs.Select(song => new GridItemView
                        {
                            Title = song.Title,
                            Source = ConvertBase64ToImage(Convert.ToBase64String(song.Image))
                        }).ToList());

                        gridItems.AddRange(dbArtists.Select(artist => new GridItemView
                        {
                            Title = artist.Name,
                            Source = ConvertBase64ToImage(artist.Base64Image)
                        }).ToList());

                        gridItems.AddRange(dbAlbums.Select(album => new GridItemView
                        {
                            Title = album.Title,
                            Source = ConvertBase64ToImage(album.Base64Image)
                        }).ToList());
                        break;
                    }
            }

            List<UIElement> items = ConvertToGridItems(gridItems);
            AddItemsToGrid(foundResults, items);
        }
        private List<UIElement> ConvertToGridItems (List<GridItemView> items)
        {
            List<UIElement> gridItems = new List<UIElement>();
            foreach (var item in items)
            {
                TextBlock textBlock = new TextBlock()
                {
                    Text = item.Title,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                Image image = new Image()
                {
                    Source = item.Source,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10),
                    Stretch = Stretch.Fill,
                };

                StackPanel stackPanel = new StackPanel()
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1D1D1D")),
                    Margin = new Thickness(20, 10, 20, 10)
                };

                stackPanel.Children.Add(image);
                stackPanel.Children.Add(textBlock);
                gridItems.Add(stackPanel);
            }

            return gridItems;
        }

        private async void btnSearchClick(object sender, RoutedEventArgs e)
        {
            string btnName = (sender as Button).Name;

            string searchingEntity = btnName.Substring(9);
            await ShowSearchedItems(searchingEntity);
        }
    }
}
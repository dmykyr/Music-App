using System.Windows.Input;
using System.Windows.Media;

namespace MusicApp.Views
{
    public class GridItemView
    {
        public string Title { get; set; }
        public ImageSource Source { get; set; }
        public MouseButtonEventHandler? ClickEvent {  get; set; }
        public object? ResponseClass { get; set; }
    }
}

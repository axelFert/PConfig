using PConfig.Model;
using System.Windows.Controls;

namespace PConfig.View.TreeItem
{
    internal class PlaceTreeItem : TreeViewItem
    {
        public Place Place { get; set; }

        public PlaceTreeItem(Place place)
        {
            Place = place;
            Header = place.name;
        }
    }
}
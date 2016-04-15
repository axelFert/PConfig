using PConfig.Model;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PConfig.View.TreeItem
{
    internal class TotemTreeItem : TreeViewItem
    {
        public Totem Totem { get; set; }
        public List<PlaceTreeItem> PlaceChildren { get; set; }

        public TotemTreeItem(Totem tot, List<Place> place)
        {
            this.Totem = tot;
            Header = tot.name;
            PlaceChildren = new List<PlaceTreeItem>();
            //place.Sort((pl1, pl2) => pl1.ID_mac.CompareTo(pl2.ID_mac));
            place.ForEach(pl => PlaceChildren.Add(new PlaceTreeItem(pl)));
            PlaceChildren.ForEach(pl => Items.Add(pl));
        }
    }
}
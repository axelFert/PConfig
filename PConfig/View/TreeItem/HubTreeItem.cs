using PConfig.Model;
using System.Windows.Controls;

namespace PConfig.View.TreeItem
{
    internal class HubTreeItem : TreeViewItem
    {
        public Hub Hub { get; set; }

        public HubTreeItem(Hub hub)
        {
            Hub = hub;
            Header = Hub.Nom;
        }
    }
}
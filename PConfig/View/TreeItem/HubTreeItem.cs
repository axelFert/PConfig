using PConfig.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
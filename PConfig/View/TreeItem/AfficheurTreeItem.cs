using PConfig.Model.DAO;
using System;
using System.Windows.Controls;

namespace PConfig.View.TreeItem
{
    internal class AfficheurTreeItem : TreeViewItem
    {
        public Compteur Compteur { get; set; }

        public AfficheurTreeItem(string nom, Compteur Cpt)
        {
            Compteur = Cpt;
            Header = nom + " - " + Cpt.Nom;
            Cpt.PlaceComptees.ForEach(pl => Items.Add(new PlaceTreeItem(pl)));
        }
    }
}
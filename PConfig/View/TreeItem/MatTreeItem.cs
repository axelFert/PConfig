using PConfig.Model;
using System.Windows.Controls;

namespace PConfig.View.TreeItem
{
    public class MatTreeItem : TreeViewItem
    {
        private Mat Mat { get; set; }

        public MatTreeItem(Mat mat)
        {
            Header = mat.Nom;
            Mat = mat;
            foreach (string cpt in Mat.Afficheurs.Keys)
            {
                AfficheurTreeItem aff = new AfficheurTreeItem(cpt, Mat.Afficheurs[cpt]);
                Items.Add(aff);
            }
        }
    }
}
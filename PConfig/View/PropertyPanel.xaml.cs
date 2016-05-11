using PConfig.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour PropertyPanel.xaml
    /// </summary>
    public partial class PropertyPanel : UserControl
    {
        public ILegendObject ObjetLegende { get; set; }

        public PropertyPanel()
        {
            InitializeComponent();
        }

        public void updateAffichage()
        {
            PropertiesStack.Children.Clear();
            if (ObjetLegende == null) return;
            NomObjet.Content = ObjetLegende.getType() + " : " + ObjetLegende.getNom();

            foreach (Propriete prop in ObjetLegende.GetInfo())
            {
                LabelValue lbl = new LabelValue(prop.Nom, prop.Valeur, false);
                PropertiesStack.Children.Add(lbl);
            }
        }
    }
}
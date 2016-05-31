using PConfig.Model;
using System.Windows.Controls;

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
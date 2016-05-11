using PConfig.View.Utils;
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
    /// Logique d'interaction pour ConfigurationApplication.xaml
    /// </summary>
    public partial class ConfigurationTab : UserControl
    {
        public ConfigurationTab()
        {
            InitializeComponent();
        }

        // The delegate procedure we are assigning to our object
        public delegate void ChangeColorHandler();

        public event EventHandler OnChangeColor;

        private void UpdateTotemColor(object sender, RoutedEventArgs e)
        {
            SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_TOTEM).CouleurBordure = (Color)CouleurPickerTotem.SelectedColor;
            if (OnChangeColor != null)
            {
                OnChangeColor(this, new EventArgs());
            }
        }

        private void UpdatePlaceColor(object sender, RoutedEventArgs e)
        {
            SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_PLACE).CouleurBordure = (Color)CouleurPickerPlace.SelectedColor;
            if (OnChangeColor != null)
            {
                OnChangeColor(this, new EventArgs());
            }
        }
    }
}
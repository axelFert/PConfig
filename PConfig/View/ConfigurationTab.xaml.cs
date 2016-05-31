using PConfig.View.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            if ((Color)CouleurPickerTotem.SelectedColor == null) return;
            SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_TOTEM).CouleurBordure = (Color)CouleurPickerTotem.SelectedColor;
            if (OnChangeColor != null)
            {
                OnChangeColor(this, new EventArgs());
            }
        }

        private void UpdatePlaceColor(object sender, RoutedEventArgs e)
        {
            if ((Color)CouleurPickerPlace.SelectedColor == null) return;
            SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_PLACE).CouleurBordure = (Color)CouleurPickerPlace.SelectedColor;
            if (OnChangeColor != null)
            {
                OnChangeColor(this, new EventArgs());
            }
        }

        private void UpdateMatColor(object sender, RoutedEventArgs e)
        {
            if ((Color)CouleurPickerMat.SelectedColor == null) return;
            SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_MAT).CouleurBordure = (Color)CouleurPickerMat.SelectedColor;
            if (OnChangeColor != null)
            {
                OnChangeColor(this, new EventArgs());
            }
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour Legende.xaml
    /// </summary>
    public partial class Legende : UserControl
    {
        // The delegate procedure we are assigning to our object
        public delegate void ChangeColorHandler(Color color);

        public event ChangeColorHandler OnChangeColor;

        public delegate void ChangeBorderHandler(Color color);

        public event ChangeBorderHandler OnChangeBorder;

        public Legende()
        {
            InitializeComponent();
        }

        private void UpdateBorderColor(object sender, RoutedEventArgs e)
        {
            if (OnChangeBorder != null)
            {
                if (ClrPckerBordure.SelectedColor.HasValue)
                {
                    OnChangeBorder((Color)ClrPckerBordure.SelectedColor);
                }
            }
        }

        private void UpdateFillColor(object sender, RoutedEventArgs e)
        {
            if (OnChangeColor != null)
            {
                if (ClrPcker.SelectedColor.HasValue)
                {
                    OnChangeColor((Color)ClrPcker.SelectedColor);
                }
            }
        }
    }
}
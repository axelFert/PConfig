using Microsoft.Win32;
using PConfig.View.Utils;
using System.Windows;
using System.Windows.Controls;

namespace PConfig.View.Dessin
{
    /// <summary>
    /// Logique d'interaction pour OngletDessin.xaml
    /// </summary>
    public partial class OngletDessin : UserControl
    {
        private PlanDessin plan;

        public OngletDessin()
        {
            InitializeComponent();
            border.Child = (plan = new PlanDessin());
            initUI();
        }

        private void ChargerPlan(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Tous type (*.*)|*.*|Png (*.png)|*.png|jpeg (*.jpeg)|*.jpeg|jpg (*.jpg)|*.jpg";
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                plan = new PlanDessin(openFileDialog.FileName);
                border.Child = plan;
            }
            initUI();
        }

        private void ClearDessin(object sender, RoutedEventArgs e)
        {
            plan.ClearDessin();
        }

        private void initUI()
        {
            ComboTypeDessin.Items.Clear();
            ComboTypeDessin.Items.Add(new TypeDessin("Place", TYPE_DESSIN.PLACE));
            ComboTypeDessin.Items.Add(new TypeDessin("Totem", TYPE_DESSIN.TOTEM));
            ComboTypeDessin.Items.Add(new TypeDessin("Mat", TYPE_DESSIN.MAT));
            ComboTypeDessin.Items.Add(new TypeDessin("Libre", TYPE_DESSIN.LIBRE));
            ComboTypeDessin.SelectedIndex = 0;
        }

        private void TypeDessinChange(object sender, RoutedEventArgs e)
        {
            if (ComboTypeDessin.SelectedItem != null)
                plan.TypeDessin = (ComboTypeDessin.SelectedItem as TypeDessin).Mode;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            plan.ChangerClic();
        }
    }
}
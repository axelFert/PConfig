using PConfig.View.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour ConfigurationApplication.xaml
    /// </summary>
    public partial class ConfigurationTab : UserControl, INotifyPropertyChanged
    {
        private int diametreTotem;

        public int DiametreTotem
        {
            get { return diametreTotem; }
            set { diametreTotem = value; SmgUtilsIHM.DIAMETRE_TOTEM = diametreTotem; RaisePropertyChanged("DiametreTotem"); }
        }

        private int coteMat;

        public int CoteMat
        {
            get { return coteMat; }
            set { coteMat = value; SmgUtilsIHM.COTE_MAT = coteMat; RaisePropertyChanged("CoteMat"); }
        }

        private int taillePolice;

        public int TaillePolice
        {
            get { return taillePolice; }
            set { taillePolice = value; SmgUtilsIHM.TAILLE_POLICE = taillePolice; RaisePropertyChanged("TaillePolice"); }
        }

        private bool tailleAuto;

        public bool TailleAuto
        {
            get { return tailleAuto; }
            set { tailleAuto = value; SmgUtilsIHM.TAILLE_POLICE_AUTO = tailleAuto; RaisePropertyChanged("TailleAuto"); }
        }

        public ConfigurationTab()
        {
            this.DataContext = this;

            tailleAuto = SmgUtilsIHM.TAILLE_POLICE_AUTO;
            taillePolice = SmgUtilsIHM.TAILLE_POLICE;
            coteMat = SmgUtilsIHM.COTE_MAT;
            diametreTotem = SmgUtilsIHM.DIAMETRE_TOTEM;
            InitializeComponent();
        }

        // The delegate procedure we are assigning to our object
        public delegate void ChangeColorHandler();

        public event EventHandler OnChangeColor;

        public event PropertyChangedEventHandler PropertyChanged;

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

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
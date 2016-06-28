using System.ComponentModel;
using System.Windows;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour PopupNombrePlace.xaml
    /// </summary>
    public partial class PopupNombrePlace : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int nbPlaceHauteur;

        public int NbPlaceHauteur
        {
            get { return nbPlaceHauteur; }
            set { nbPlaceHauteur = value; RaisePropertyChanged("NbPlaceHauteur"); }
        }

        private int nbPlaceLargeur;

        public int NbPlaceLargeur
        {
            get { return nbPlaceLargeur; }
            set { nbPlaceLargeur = value; RaisePropertyChanged("NbPlaceLargeur"); }
        }

        private int numeroPlace;

        public int NumeroPlace
        {
            get { return numeroPlace; }
            set { numeroPlace = value; RaisePropertyChanged("NumeroPlace"); }
        }

        public bool IsCancelled = false;

        public PopupNombrePlace()
        {
            this.DataContext = this;
            InitializeComponent();
            NbPlaceHauteur = 1;
            NbPlaceLargeur = 1;
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void ButtonOk(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonCancel(object sender, RoutedEventArgs e)
        {
            this.IsCancelled = true;
            this.Close();
        }
    }
}
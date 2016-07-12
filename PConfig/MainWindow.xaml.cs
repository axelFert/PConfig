using PConfig.Conf;
using PConfig.Tools;
using PConfig.View;
using PConfig.View.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace PConfig
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// LOGGER
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ConfigurationSite myConf { get; set; }

        public MainWindow()
        {
            log.Info("lancement de l'application");
            InitializeComponent();
            this.Title = "pConfig " + Properties.Settings.Default.version;
            InitGlobalVariable();
            InitConf();
        }

        private void ClickOpenFile(object sender, RoutedEventArgs e)
        {
            //selection du site a charger
            PopupConfiguration popup = new PopupConfiguration();
            popup.ShowDialog();
            ConfigurationSite conf = popup.selectedConf;

            if (conf != null)
            {
                this.Parking.chargerConf(conf);
            }
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            log.Info("Fermeture de l'application");
            Close();
        }

        #region misc

        private void InitConf()
        {
            Configuration Conf = XmlParser.GetConfig("ConfSites.xml");
            SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_MAT).CouleurBordure = (Color)ColorConverter.ConvertFromString(Conf.CouleurMat);
            SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_TOTEM).CouleurBordure = (Color)ColorConverter.ConvertFromString(Conf.CouleurTotem);
            SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_PLACE).CouleurBordure = (Color)ColorConverter.ConvertFromString(Conf.CouleurPlace);

            SmgUtilsIHM.COTE_MAT = Conf.TailleMat;
            SmgUtilsIHM.DIAMETRE_TOTEM = Conf.TailleTotem;
        }

        private void InitGlobalVariable()
        {
            SmgUtilsIHM.LIST_COULEUR_ETAT = new Dictionary<ETAT_OBJET_PLAN, ColorState>();
            foreach (ETAT_OBJET_PLAN values in EnumUtil.GetValues<ETAT_OBJET_PLAN>())
            {
                ColorState couleur = null;
                switch (values)
                {
                    case ETAT_OBJET_PLAN.CONNEXION_RADIO:
                        //couleur = new ColorState(values, Color.FromArgb(155, 0, 255, 0), Colors.Black);
                        couleur = new ColorState(values, Color.FromArgb(155, 0, 0, 255), Colors.Red);
                        break;

                    case ETAT_OBJET_PLAN.COMPTAGE_RADIO:
                        couleur = new ColorState(values, Color.FromArgb(155, 0, 0, 255), Colors.Red);
                        break;

                    case ETAT_OBJET_PLAN.COMPTAGE_MULTIPANEL:
                        couleur = new ColorState(values, Color.FromArgb(155, 255, 0, 0), Colors.Red);
                        break;

                    case ETAT_OBJET_PLAN.ELEMENT_LIE:
                        couleur = new ColorState(values, Color.FromArgb(155, 0, 255, 0), Colors.Green);
                        break;

                    case ETAT_OBJET_PLAN.NONE_PLACE:
                    case ETAT_OBJET_PLAN.NONE_TOTEM:
                    case ETAT_OBJET_PLAN.NONE_MAT:
                    default:
                        couleur = new ColorState(values, Color.FromArgb(0, 0, 255, 0), Colors.White);
                        break;
                }

                SmgUtilsIHM.LIST_COULEUR_ETAT.Add(couleur.Etat, couleur);
            }
            SmgUtilsIHM.COULEUR_PLACE_SELECTION = new ColorState(Color.FromArgb(155, 0, 255, 0), Colors.Green);
        }

        #endregion misc
    }
}
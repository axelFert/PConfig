using PConfig.Conf;
using PConfig.Tools;
using PConfig.Tools.Mysql;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour PopupConfiguration.xaml
    /// </summary>
    public partial class PopupConfiguration : Window
    {
        public ConfigurationSite selectedConf { get; set; }

        public PopupConfiguration()
        {
            InitializeComponent();
            init();
            loadConf();
        }

        public void loadConf()
        {
            List<ConfigurationSite> lstConf = XmlParser.GetConfig("ConfSites.xml").LstSite;
            ConfigurationSite localConf = new ConfigurationSite();
            localConf.NomSite = "Local";
            localConf.HostName = "localhost";
            localConf.Port = "3306";
            localConf.Login = "root";
            localConf.Password = "";
            //comboConf.Items.Add(localConf);
            foreach (ConfigurationSite conf in lstConf)
            {
                comboConf.Items.Add(conf);
            }
            comboConf.SelectedItem = localConf;
        }

        private void choixConf(object sender, SelectionChangedEventArgs e)
        {
            ConfigurationSite conf = (e.AddedItems[0] as ConfigurationSite);
            TxtHost.Text = conf.HostName;
            TxtPort.Text = conf.Port;
            TxtLogin.Text = conf.Login;
            TxtPassword.Text = conf.Password;

            LstPlan.Children.Clear();
            foreach (var planInfo in conf.ListePlan)
            {
                cbNiveau cb = new cbNiveau(planInfo.Zone, planInfo.Nom, planInfo.Path);
                LstPlan.Children.Add(cb);
            }
            MasterData.IsChecked = true;
            BtnChargerConf.IsEnabled = true;
            BtnChargerConf.ToolTip = "";
        }

        private void BtnChargerConf_Click(object sender, RoutedEventArgs e)
        {
            selectedConf = new ConfigurationSite();
            selectedConf.HostName = TxtHost.Text;
            selectedConf.Login = TxtLogin.Text;
            selectedConf.Password = TxtPassword.Text;
            selectedConf.Port = TxtPort.Text;

            MySqlTools.init(selectedConf.HostName, selectedConf.Port, selectedConf.Login, selectedConf.Password, "db_smg_masterdata");
            if (!MySqlTools.getConnection().CanConnect())
            {
                MessageBox.Show("Impossible de se connecter à la base", "Erreur de connexion");
                return;
            }

            foreach (var cb in LstPlan.Children)
            {
                cbNiveau niveau = cb as cbNiveau;
                if (niveau != null)
                {
                    if (niveau.IsChecked != false)
                    {
                        selectedConf.ListePlan.Add(new PlanInfo(niveau.IdZone, niveau.Nom, niveau.Path));
                    }
                }
            }
            if (MasterData.IsChecked == true)
            {
                selectedConf.isOnMasterData = true;
            }
            if (Run.IsChecked == true)
            {
                selectedConf.isOnMasterData = false;
            }
            this.Close();
        }

        public void init()
        {
            BtnChargerConf.IsEnabled = false;
            BtnChargerConf.ToolTip = "Choisir un site avant de charger sa conf";
        }
    }
}
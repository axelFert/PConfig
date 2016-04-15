using PConfig.Conf;
using PConfig.Tools;
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
        public Configuration selectedConf { get; set; }

        public PopupConfiguration()
        {
            InitializeComponent();
            init();
            loadConf();
        }

        public void loadConf()
        {
            List<Configuration> lstConf = XmlParser.GetConfig("ConfSites.xml");
            Configuration localConf = new Configuration();
            localConf.NomSite = "Local";
            localConf.HostName = "localhost";
            localConf.Port = "3306";
            localConf.Login = "root";
            localConf.Password = "";
            comboConf.Items.Add(localConf);
            foreach (Configuration conf in lstConf)
            {
                comboConf.Items.Add(conf);
            }
            comboConf.SelectedItem = localConf;
        }

        private void choixConf(object sender, SelectionChangedEventArgs e)
        {
            Configuration conf = (e.AddedItems[0] as Configuration);
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
            selectedConf = new Configuration();
            selectedConf.HostName = TxtHost.Text;
            selectedConf.Login = TxtLogin.Text;
            selectedConf.Password = TxtPassword.Text;
            selectedConf.Port = TxtPort.Text;
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
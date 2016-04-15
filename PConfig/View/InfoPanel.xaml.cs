using PConfig.Model;
using PConfig.Model.DAO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour InfoPanel.xaml
    /// </summary>
    public partial class InfoPanel : UserControl
    {
        public int NbObject { get; set; }

        public TotemDAO TotemDao { get; set; }
        public PlaceDAO PlaceDao { get; set; }

        private LabelValue nom;
        private LabelValue pan;
        private LabelValue mac;
        private LabelValue zone;

        // The delegate procedure we are assigning to our object
        public delegate void UpdateHandler(string oldObjName, SmgObj newObj);

        public event UpdateHandler OnUpdateEvent;

        public InfoPanel()
        {
            InitializeComponent();
            NbObject = 0;
        }

        public InfoPanel(SmgObj smgObj)
        {
            InitializeComponent();
            addObj(smgObj);
        }

        public InfoPanel(List<SmgObj> lstSmgObj)
        {
            InitializeComponent();
            foreach (SmgObj smgObj in lstSmgObj)
            {
                addObj(smgObj);
            }
        }

        public void addObj(SmgObj smgObj)
        {
            if (!SelectedPlace.Items.Contains(smgObj))
            {
                SelectedPlace.Items.Add(smgObj);
                NbObject++;
            }
        }

        public void removeObj(SmgObj smgObj)
        {
            if (SelectedPlace.Items.Contains(smgObj))
            {
                SelectedPlace.Items.Remove(smgObj);
                NbObject--;
            }
        }

        private void SelectionObj(object sender, SelectionChangedEventArgs e)
        {
            SmgObj smgObj = ((sender as ComboBox).SelectedItem as SmgObj);
            if (smgObj == null)
            {
                proprieteImp.Content = null;
                return;
            }
            StackPanel prop = new StackPanel();
            nom = new LabelValue("nom", smgObj.name, true);
            pan = new LabelValue("Pan", smgObj.ID_pan.ToString(), false);
            mac = new LabelValue("Mac", smgObj.ID_mac.ToString(), false);
            zone = new LabelValue("Zone", smgObj.ID_zone.ToString(), false);

            prop.Children.Add(nom);
            prop.Children.Add(pan);
            prop.Children.Add(mac);
            prop.Children.Add(zone);

            proprieteImp.Content = prop;
        }

        public void clearObjet()
        {
            proprieteImp.Content = null;
            SelectedPlace.Items.Clear();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SmgObj smgObj = (SelectedPlace.SelectedItem as SmgObj);
            string oldName = smgObj.name;
            if (smgObj != null)
            {
                SmgObj newSmg = smgObj;
                newSmg.name = nom.getValueProp;
                if (save(newSmg))
                {
                    SelectedPlace.Items.Remove(smgObj);
                    SelectedPlace.Items.Add(newSmg);
                    SelectedPlace.SelectedItem = newSmg;

                    if (OnUpdateEvent != null)
                    {
                        OnUpdateEvent(oldName, newSmg);
                    }
                }
            }
        }

        private bool save(SmgObj smgObj)
        {
            Place place;
            Totem totem;

            bool result = false;
            if ((totem = (smgObj as Totem)) != null)
            {
                TotemDao = TotemDAO.getInstance();
                result = TotemDao.Update(totem);
            }
            else if ((place = (smgObj as Place)) != null)
            {
                PlaceDao = PlaceDAO.getInstance();
                result = PlaceDao.Update(place);
            }
            return result;
        }
    }
}
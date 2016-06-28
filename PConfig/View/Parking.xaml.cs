using PConfig.Conf;
using PConfig.Model;
using PConfig.Model.DAO;
using PConfig.Tools;
using PConfig.Tools.Mysql;
using PConfig.View.Dessin;
using PConfig.View.ObjetPlan;
using PConfig.View.TreeItem;
using PConfig.View.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour Parking.xaml
    /// </summary>
    public partial class Parking : UserControl
    {
        private List<IAbstractNiveau> LstNiveaux;

        // les listes des objets

        private List<Place> LstPlace;
        private List<Totem> LstTotem;
        private List<string> LstCategoriePlace;
        private List<SmgObj> LstAllObject = new List<SmgObj>();
        private List<Compteur> LstCompteur;
        private List<Mat> LstMat;
        private List<Multipanel> LstMultiPanel;
        private List<Hub> lstHub;

        // classes DAO de recupération des objets

        private TotemDAO TotemDao;
        private PlaceDAO PlaceDao;
        private MultipanelDAO MultipanelDao;
        private CompteurDAO CompteurDao;
        private MatDAO MatDao;
        private HubDAO HubDao;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(NiveauGlobal));

        public Parking()
        {
            InitializeComponent();
            initComboboxAffichage();

            LstPlace = new List<Place>();
            LstTotem = new List<Totem>();
            LstNiveaux = new List<IAbstractNiveau>();
        }

        /// <summary>
        /// recuperation de tout les objet
        /// </summary>
        private void getAllObject()
        {
            clearParking();
            PlaceDao = PlaceDAO.getInstance();
            TotemDao = TotemDAO.getInstance();
            MatDao = MatDAO.Instance;
            MultipanelDao = MultipanelDAO.getInstance();
            CompteurDao = CompteurDAO.getInstance();
            HubDao = HubDAO.Instance;

            LstPlace = PlaceDao.getAll();
            LstTotem = TotemDao.getAll();
            LstMat = MatDao.getAll();
            LstCategoriePlace = PlaceDao.GetAllCategory();
            LstCompteur = CompteurDao.getAll();
            lstHub = HubDao.getAll();

            LstAllObject.AddRange(LstPlace);
            LstAllObject.AddRange(LstTotem);
            LstAllObject.AddRange(LstMat);
            LstMultiPanel = MultipanelDao.getAll();
            LstPlace = AssociationPlaceMultipanel(LstPlace, LstMultiPanel);
            AssocierPlaceTotem();
        }

        private List<Place> AssociationPlaceMultipanel(List<Place> lstPlace, List<Multipanel> lstMulti)
        {
            foreach (Place place in lstPlace)
            {
                foreach (Multipanel multi in lstMulti)
                {
                    if (place.IdTotemRadio.Equals(multi.PanMacCounter))
                    {
                        if ((multi.NewMask[place.Numero] == '1'))
                            place.LstTotemDispalyer.Add(multi.ID_panel_displayer, multi.PanMacDispalyer);
                    }
                }
            }
            return lstPlace;
        }

        private void AssocierPlaceTotem()
        {
            foreach (Totem tot in LstTotem)
            {
                foreach (Place pl in LstPlace)
                {
                    if (tot.IdTotemRadio == pl.IdTotemRadio)
                    {
                        tot.PlaceRadio.Add(pl);
                    }
                    if (pl.LstTotemDispalyer.ContainsKey(tot.ID_panels) || (tot.IdTotemRadio == pl.IdTotemRadio && pl.ID_panels != 0))
                    {
                        tot.PlaceAffiche.Add(pl);
                    }
                }
            }
        }

        private void clearParking()
        {
            LstPlace.Clear();
            LstTotem.Clear();
            LstAllObject.Clear();
        }

        public async void chargerConf(Configuration conf)
        {
            foreach (var planInfo in conf.ListePlan)
            {
                if (!File.Exists(planInfo.Path))
                {
                    MessageBox.Show("Erreur dans le fichier de configuration pour le fichier : " + planInfo.Path, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            pbStatus.Visibility = System.Windows.Visibility.Visible;
            string db = (conf.isOnMasterData ? "db_smg_masterdata" : "db_smg_run");
            MySqlTools.init(conf.HostName, conf.Port, conf.Login, conf.Password, db);

            // recupération des info de niveau
            List<Level> lstLvl = LevelDAO.getInstance().getAll();

            //recup place
            await Task.Run(() =>
            {
                getAllObject();
            });
            CreationCompteur();
            populateTreeObj();

            Niveaux.Items.Clear();

            // onglet de recap
            TabItem tabGlobalBis = new TabItem();
            tabGlobalBis.Header = "Site Complet";
            NiveauGlobal NivGlobal = new NiveauGlobal(conf.ListePlan, LstAllObject, LstPlace, LstTotem);
            NivGlobal.SelectionEventHandler += selectionPlan;
            tabGlobalBis.Content = NivGlobal;
            Niveaux.Items.Add(tabGlobalBis);
            LstNiveaux.Add(NivGlobal);

            // Création d'un onglet par plan a afficher
            foreach (var planInfo in conf.ListePlan)
            {
                TabItem tab = new TabItem();
                tab.Header = planInfo.Nom;
                Niveau niv = new Niveau(planInfo.Path, planInfo.Nom, planInfo.Zone, LstAllObject.Where(obj => obj.ID_zone.Equals(planInfo.Zone)).ToList());
                niv.SelectionEventHandler += selectionPlan;
                tab.Content = niv;
                Niveaux.Items.Add(tab);
                LstNiveaux.Add(niv);
            }
            TabItem confTab = new TabItem();
            confTab.Header = "Configuration";
            ConfigurationTab cnf = new ConfigurationTab();
            cnf.OnChangeColor += ColorChanged;
            confTab.Content = cnf;
            Niveaux.Items.Add(confTab);

            TabItem dessin = new TabItem();
            dessin.Header = "dessin !!!!";
            OngletDessin dess = new OngletDessin();
            dessin.Content = dess;
            Niveaux.Items.Add(dessin);

            pbStatus.Visibility = System.Windows.Visibility.Hidden;
        }

        #region IHM

        private void updateAffichage(MODE_AFFICHAGE_OBJET newMode)
        {
            LstNiveaux.ForEach(niv => niv.UpdateAffichage(newMode));
        }

        private void updateAffichageTotem(MODE_AFFICHAGE_OBJET newMode)
        {
            LstNiveaux.ForEach(niv => niv.UpdateAffichageTotem(newMode));
        }

        private void changeAffichagePlace(object sender, SelectionChangedEventArgs e)
        {
            ModeAffichage mode = ((sender as ComboBox).SelectedItem) as ModeAffichage;

            updateAffichage(mode.Mode);
        }

        private void changeAffichageTotem(object sender, SelectionChangedEventArgs e)
        {
            ModeAffichage mode = ((sender as ComboBox).SelectedItem) as ModeAffichage;

            updateAffichageTotem(mode.Mode);
        }

        private void initComboboxAffichage()
        {
            foreach (MODE_AFFICHAGE_OBJET value in EnumUtil.GetValues<MODE_AFFICHAGE_OBJET>())
            {
                ModeAffichage mode = null;
                switch (value)
                {
                    case MODE_AFFICHAGE_OBJET.ID:
                        mode = new ModeAffichage("ID", value);
                        CbxAffichagePlace.Items.Add(mode);
                        CbxAffichageTotem.Items.Add(mode);
                        //CbxAffichageTotem.SelectedItem = mode;
                        break;

                    case MODE_AFFICHAGE_OBJET.NOM:
                        mode = new ModeAffichage("Nom", value);
                        CbxAffichagePlace.Items.Add(mode);
                        CbxAffichageTotem.Items.Add(mode);
                        break;

                    case MODE_AFFICHAGE_OBJET.PAN_MAC:
                        mode = new ModeAffichage("PAN / MAC ", value);
                        CbxAffichagePlace.Items.Add(mode);
                        CbxAffichageTotem.Items.Add(mode);
                        break;

                    case MODE_AFFICHAGE_OBJET.TYPE:
                        mode = new ModeAffichage("Categorie", value);
                        CbxAffichagePlace.Items.Add(mode);
                        CbxAffichageTotem.Items.Add(mode);
                        break;

                    default:
                        break;
                }
            }
            //CbxAffichagePlace.SelectedIndex = 1;
            //CbxAffichageTotem.SelectedIndex = 1;
        }

        public void CreationCompteur()
        {
            foreach (Compteur cpt in LstCompteur)
            {
                cpt.PlaceComptees = LstPlace.Where(
                    plc => cpt.LstPanMac.Contains(plc.ID_pan + "/" + plc.ID_mac)
                ).ToList();
            }

            foreach (Mat mat in LstMat)
            {
                foreach (Tuple<string, int> tpl in mat.AfficheursId)
                {
                    foreach (Compteur cp in LstCompteur)
                    {
                        if (tpl.Item2 == cp.Id)
                        {
                            mat.Afficheurs.Add(tpl.Item1, cp);
                        }
                    }
                }
            }
        }

        public void ColorChanged(object sender, EventArgs e)
        {
            redrawAllPlace();
        }

        public void redrawAllPlace()
        {
            LstNiveaux.ForEach(niv => niv.UpdateColor());
        }

        #endregion IHM

        private void populateTreeObj()
        {
            ParkingObject.Items.Clear();

            if (lstHub.Count > 0)
            {
                TreeViewItem HubHead = new TreeViewItem();
                HubHead.Header = "Hub";
                ParkingObject.Items.Add(HubHead);
                foreach (Hub hub in lstHub)
                {
                    HubTreeItem tree = new HubTreeItem(hub);
                    HubHead.Items.Add(tree);
                }
            }

            if (LstTotem.Count > 0)
            {
                // totem radio
                TreeViewItem totemHead = new TreeViewItem();
                totemHead.Header = "Totem";
                ParkingObject.Items.Add(totemHead);
                foreach (Totem tot in LstTotem)
                {
                    TotemTreeItem tree = new TotemTreeItem(tot, LstPlace.Where(pl => pl.IdTotemRadio == tot.IdTotemRadio).ToList());
                    totemHead.Items.Add(tree);
                }
            }

            if (LstMat.Count > 0)
            {
                TreeViewItem PanelHead = new TreeViewItem();
                PanelHead.Header = "Mat";
                ParkingObject.Items.Add(PanelHead);
                foreach (Mat mat in LstMat)
                {
                    MatTreeItem tree = new MatTreeItem(mat);
                    PanelHead.Items.Add(tree);
                }
            }

            if (LstCategoriePlace.Count > 0)
            {
                TreeViewItem catHead = new TreeViewItem();
                catHead.Header = "Type de place";
                ParkingObject.Items.Add(catHead);
                foreach (string str in LstCategoriePlace)
                {
                    TreeViewItem cat = new TreeViewItem();
                    cat.Header = str;
                    catHead.Items.Add(cat);
                    LstPlace.Where(pl => pl.category.Equals(str)).ToList().ForEach(pl => cat.Items.Add(new PlaceTreeItem(pl)));
                }
            }
        }

        private void UpdateView(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selection = (TreeViewItem)((TreeView)sender).SelectedItem;
            if (selection == null)
                return;

            if (selection as AfficheurTreeItem != null)
            {
                LstNiveaux.ForEach(niv => niv.updateViewCompteur((selection as AfficheurTreeItem).Compteur));
                InformationPanel.ObjetLegende = (selection as AfficheurTreeItem).Compteur;
                InformationPanel.updateAffichage();
            }
            else if (selection as PlaceTreeItem != null)
            {
                Place pl = (selection as PlaceTreeItem).Place;
                LstNiveaux.ForEach(niv => niv.SelectionSmgObj(pl.ID_pan, pl.ID_mac));
                InformationPanel.ObjetLegende = pl;
                InformationPanel.updateAffichage();
            }
            else if (selection as TotemTreeItem != null)
            {
                Totem tot = (selection as TotemTreeItem).Totem;
                LstNiveaux.ForEach(niv => niv.SelectionSmgObj(tot.ID_pan, tot.ID_mac));
                InformationPanel.ObjetLegende = tot;
                InformationPanel.updateAffichage();
            }
            else if (selection as MatTreeItem != null)
            {
                Mat mat = (selection as MatTreeItem).Mat;
                LstNiveaux.ForEach(niv => niv.SelectionSmgObj(mat.ID_pan, mat.ID_mac));
                InformationPanel.ObjetLegende = mat;
                InformationPanel.updateAffichage();
            }
            else if (selection as HubTreeItem != null)
            {
                Hub hub = (selection as HubTreeItem).Hub;
                LstNiveaux.ForEach(niv => niv.SelectionSmgObjByHub(hub.Numero));
                InformationPanel.ObjetLegende = hub;
                InformationPanel.updateAffichage();
            }
            else if (LstCategoriePlace.Contains(selection.Header.ToString()))
            {
                // selection de toutes les places de ce type
                LstNiveaux.ForEach(niv => niv.SelectionTypePlace(selection.Header.ToString()));
                InformationPanel.ObjetLegende = null;
                InformationPanel.updateAffichage();
            }
        }

        private void BoutonNegatif_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ColorState colors = null;
            ColorState colorsPlace = null;
            if (SmgUtilsIHM.IS_NEGATIF)
            {
                colorsPlace = new ColorState(ETAT_OBJET_PLAN.NONE_PLACE, Color.FromArgb(0, 0, 255, 0), Colors.Black);
                colors = new ColorState(ETAT_OBJET_PLAN.NONE_TOTEM, Color.FromArgb(0, 0, 255, 0), Colors.Black);
            }
            else
            {
                colorsPlace = new ColorState(ETAT_OBJET_PLAN.NONE_PLACE, Color.FromArgb(0, 0, 255, 0), Colors.White);
                colors = new ColorState(ETAT_OBJET_PLAN.NONE_PLACE, Color.FromArgb(0, 0, 255, 0), Colors.Black);
            }

            SmgUtilsIHM.IS_NEGATIF = !SmgUtilsIHM.IS_NEGATIF;

            SmgUtilsIHM.LIST_COULEUR_ETAT[ETAT_OBJET_PLAN.NONE_PLACE] = colorsPlace;
            //SmgUtilsIHM.LIST_COULEUR_ETAT.Add(colorsPlace.Etat, colorsPlace);

            SmgUtilsIHM.LIST_COULEUR_ETAT[ETAT_OBJET_PLAN.NONE_TOTEM] = colors;
            //SmgUtilsIHM.LIST_COULEUR_ETAT.Add(colors.Etat, colors);

            redrawAllPlace();
        }

        private void Radio_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            SmgUtilsIHM.IS_RADIO_LINK = true;
        }

        private void Comptage_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            SmgUtilsIHM.IS_RADIO_LINK = false;
        }

        private void SelectionObjectTree(object sender, EventArgs e)
        {
            //return;
            if ((sender as SmgObjView) != null)
            {
                int pan = (sender as SmgObjView).Pan;
                int mac = (sender as SmgObjView).Mac;

                foreach (TreeViewItem Superobj in ParkingObject.Items)
                {
                    foreach (var obj in Superobj.Items)
                    {
                        // on parcours tout les item du tree
                        TreeViewItem selection = (TreeViewItem)obj;
                        if (selection as TotemTreeItem != null) // cas d'un totem
                        {
                            Totem tot = (selection as TotemTreeItem).Totem;
                            if (tot.ID_pan == pan && tot.ID_mac == (mac & SmgUtil.MASQUE_TOTEM_RADIO_MAC))
                            {
                                if (mac == tot.ID_mac)
                                {
                                    selection.IsSelected = true;
                                    return;
                                }
                                else
                                {
                                    selection.IsExpanded = true;
                                    foreach (var obj2 in selection.Items)
                                    {
                                        TreeViewItem place = (TreeViewItem)obj2;
                                        if (place as PlaceTreeItem != null)
                                        {
                                            Place pl = (place as PlaceTreeItem).Place;
                                            if (pl.ID_mac == mac)
                                            {
                                                place.IsSelected = true;
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (selection as PlaceTreeItem != null)
                        {
                            Place pl = (selection as PlaceTreeItem).Place;
                            if (pl.ID_mac == mac && pl.ID_pan == pan)
                            {
                                selection.IsSelected = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void selectionPlan(object sender, EventArgs e)
        {
            if ((sender as SmgObjView) == null)
                return;
            int pan = (sender as SmgObjView).Pan;
            int mac = (sender as SmgObjView).Mac;

            InformationPanel.ObjetLegende = LstAllObject.Find(obj => obj.ID_pan == pan && obj.ID_mac == mac) as ILegendObject;
            InformationPanel.updateAffichage();
        }

        private void UpdatePolice(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            SmgUtilsIHM.TAILLE_POLICE = taillePolice.Value.HasValue ? taillePolice.Value.Value : SmgUtilsIHM.TAILLE_POLICE;
            redrawAllPlace();
        }
    }
}
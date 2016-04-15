using PConfig.Conf;
using PConfig.Model;
using PConfig.Model.DAO;
using PConfig.Tools;
using PConfig.Tools.Mysql;
using PConfig.View.TreeItem;
using PConfig.View.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Place> LstPlace { get; set; }
        public List<Totem> LstTotem { get; set; }
        public List<string> LstCategoriePlace { get; set; }
        private List<SmgObj> LstAllObject = new List<SmgObj>();
        private List<Compteur> LstCompteur { get; set; }
        private List<Mat> LstMat { get; set; }

        private List<Multipanel> LstMultiPanel { get; set; }

        private TotemDAO TotemDao;
        private PlaceDAO PlaceDao;
        private MultipanelDAO MultipanelDao;
        private CompteurDAO CompteurDao;
        private MatDAO MatDao;

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
            MultipanelDao = MultipanelDAO.getInstance();
            LstPlace = PlaceDao.getAll();
            LstTotem = TotemDao.getAll();
            LstCategoriePlace = PlaceDao.GetAllCategory();

            LstAllObject.AddRange(LstPlace);
            LstAllObject.AddRange(LstTotem);
            LstMultiPanel = MultipanelDao.getAll();
            LstPlace = AssociationPlaceMultipanel(LstPlace, LstMultiPanel);
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
                tot.
            }
        }

        private void clearParking()
        {
            LstPlace.Clear();
            LstTotem.Clear();
            LstAllObject.Clear();
        }

        public void chargerConf(Configuration conf)
        {
            string db = (conf.isOnMasterData ? "db_smg_masterdata" : "db_smg_run");
            MySqlTools.init(conf.HostName, conf.Port, conf.Login, conf.Password, db);
            //on clear les niveaux déjà présents

            // recupération des info de niveau
            List<Level> lstLvl = LevelDAO.getInstance().getAll();

            //recup place
            getAllObject();
            CreationCompteur();
            populateTreeObj();

            Niveaux.Items.Clear();

            // onglet de recap
            TabItem tabGlobalBis = new TabItem();
            tabGlobalBis.Header = "Site Complet";
            NiveauGlobal NivGlobal = new NiveauGlobal(conf.ListePlan, LstAllObject, LstPlace, LstTotem);
            tabGlobalBis.Content = NivGlobal;
            Niveaux.Items.Add(tabGlobalBis);
            LstNiveaux.Add(NivGlobal);

            // Création d'un onglet par plan a afficher
            foreach (var planInfo in conf.ListePlan)
            {
                TabItem tab = new TabItem();
                tab.Header = planInfo.Nom;
                Niveau niv = new Niveau(planInfo.Path, planInfo.Nom, planInfo.Zone, LstAllObject.Where(obj => obj.ID_zone.Equals(planInfo.Zone)).ToList());
                tab.Content = niv;
                Niveaux.Items.Add(tab);
                LstNiveaux.Add(niv);
            }
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
        }

        public void CreationCompteur()
        {
            MatDao = MatDAO.Instance;
            LstMat = MatDao.getAll();
            CompteurDao = CompteurDAO.getInstance();
            LstCompteur = CompteurDao.getAll();
            foreach (Compteur cpt in LstCompteur)
            {
                cpt.PlaceComptees = LstPlace.Where(
                    plc => cpt.LstPanMac.Contains(plc.ID_pan + "/" + plc.ID_mac)
                ).ToList();
            }

            foreach (Mat mat in LstMat)
            {
                mat.Afficheurs = new Dictionary<string, Compteur>();
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

        #endregion IHM

        private void populateTreeObj()
        {
            ParkingObject.Items.Clear();
            if (LstTotem.Count > 0)
            {
                // totem radio
                TreeViewItem totemHead = new TreeViewItem();
                totemHead.Header = "Totem";
                ParkingObject.Items.Add(totemHead);
                //LstTotem.Sort((emp1, emp2) => emp1.IdTotemRadio.CompareTo(emp2.IdTotemRadio));
                foreach (Totem tot in LstTotem)
                {
                    TotemTreeItem tree = new TotemTreeItem(tot, LstPlace.Where(pl => pl.IdTotemRadio == tot.IdTotemRadio).ToList());
                    totemHead.Items.Add(tree);
                }

                ////totem multi panel
                //TreeViewItem MultiPanelHead = new TreeViewItem();
                //MultiPanelHead.Header = "MultiPanel";
                //ParkingObject.Items.Add(MultiPanelHead);
                ////LstTotem.Sort((emp1, emp2) => emp1.IdTotemRadio.CompareTo(emp2.IdTotemRadio));
                //foreach (Totem tot in LstTotem)
                //{
                //    List<Place> lstplace = LstPlace.Where(pl => (pl.IdTotemRadio == tot.IdTotemRadio && pl.ID_panels != 0) || pl.LstTotemDispalyer.ContainsValue(tot.ID_panels)).ToList();

                //    TotemTreeItem tree = new TotemTreeItem(tot, lstplace);
                //    MultiPanelHead.Items.Add(tree);
                //}
            }
            else
            {
                TreeViewItem PlaceHead = new TreeViewItem();
                PlaceHead.Header = "Place";
                ParkingObject.Items.Add(PlaceHead);
                LstPlace.ForEach(pl => PlaceHead.Items.Add(new PlaceTreeItem(pl)));
            }

            if (LstMat.Count > 0)
            {
                TreeViewItem PanelHead = new TreeViewItem();
                PanelHead.Header = "Panel";
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
            else if (LstCategoriePlace.Contains(selection.Header.ToString()))
            {
                // selection de toutes les places de ce type
                LstNiveaux.ForEach(niv => niv.SelectionTypePlace(selection.Header.ToString()));
            }
        }

        private void BoutonNegatif_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ColorState colors = null;
            if (SmgUtilsIHM.IS_NEGATIF)
                colors = new ColorState(ETAT_OBJET_PLAN.NONE, Color.FromArgb(0, 0, 255, 0), Colors.Black);
            else
                colors = new ColorState(ETAT_OBJET_PLAN.NONE, Color.FromArgb(0, 0, 255, 0), Colors.White);

            SmgUtilsIHM.IS_NEGATIF = !SmgUtilsIHM.IS_NEGATIF;

            SmgUtilsIHM.LIST_COULEUR_ETAT.Remove(ETAT_OBJET_PLAN.NONE);
            SmgUtilsIHM.LIST_COULEUR_ETAT.Add(colors.Etat, colors);

            LstNiveaux.ForEach(niv => niv.UpdateColor());
        }

        private void Radio_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            SmgUtilsIHM.IS_RADIO_LINK = true;
        }

        private void Comptage_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            SmgUtilsIHM.IS_RADIO_LINK = false;
        }
    }
}
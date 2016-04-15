using PConfig.Model;
using PConfig.Model.DAO;
using PConfig.View.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour Niveau.xaml - NE PAS UTILISER DEPRECATED
    /// </summary>
    public partial class MultiNiveau : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<Plan> AllPlan { get; set; }
        public List<Place> LstPlace { get; set; }
        public List<Totem> LstTotem { get; set; }
        private List<SmgObj> LstAllObject = new List<SmgObj>();
        private List<Compteur> LstCompteur { get; set; }

        private List<Multipanel> LstMultiPanel { get; set; }

        private TotemDAO TotemDao;
        private PlaceDAO PlaceDao;
        private MultipanelDAO MultipanelDao;
        private CompteurDAO CompteurDao;

        private InfoPanel infoPanel;

        private Legende Legende { get; set; }

        /// <summary>
        /// Creation d'un niveau de parking
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="name"></param>
        /// <param name="zone"></param>
        public MultiNiveau(List<Tuple<string, string, int>> niveaux)
        {
            InitializeComponent();

            AllPlan = new List<Plan>();
            LstPlace = new List<Place>();
            LstTotem = new List<Totem>();
            LstCompteur = new List<Compteur>();
            double totalWidth = 0;

            foreach (Tuple<string, string, int> niv in niveaux)
            {
                Plan plan = new Plan(niv.Item1);
                plan.IdZone = niv.Item3;
                // ajout d'un event pour la remonter d'info lors du click sur une place
                plan.InfoEventHandler += InfoSelectionPlace;
                AllPlan.Add(plan);
                myCanvas.Children.Add(plan);
                totalWidth += plan.Width;
            }
            border.Width = 2000;
            border.Height = 50000;
            infoPanel = new InfoPanel();
            InfoExpander.Content = infoPanel;
            InfoExpander.Visibility = Visibility.Hidden;
            infoPanel.OnUpdateEvent += UpdateInterface;

            InitUi();
        }

        private void InitUi()
        {
            ColorState colors = SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.CONNEXION_RADIO);
            radioCheck.Background = new SolidColorBrush(colors.CouleurRemplissage);
            colors = SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.COMPTAGE_RADIO);
            ComptageCheck.Background = new SolidColorBrush(colors.CouleurRemplissage);
        }

        #region Dessin

        /// <summary>
        /// Event lancer lors du click sur le dessin de toutes les places
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawAllPlace(object sender, RoutedEventArgs e)
        {
            DrawAllObject();
        }

        private void DrawAllObject()
        {
            clearPlace();
            PlaceDao = PlaceDAO.getInstance();
            TotemDao = TotemDAO.getInstance();
            MultipanelDao = MultipanelDAO.getInstance();
            LstPlace = PlaceDao.getAll();
            LstTotem = TotemDao.getAll();
            foreach (Plan plan in AllPlan)
            {
                foreach (Place place in LstPlace)
                {
                    if (place.ID_zone == plan.IdZone)
                    {
                        plan.dessinerPlace(place);
                    }
                }

                foreach (Totem totem in LstTotem)
                {
                    if (totem.ID_zone == plan.IdZone)
                    {
                        plan.dessinerTotem(totem);
                    }
                }
            }

            LstAllObject.AddRange(LstPlace);
            LstAllObject.AddRange(LstTotem);
            LstMultiPanel = MultipanelDao.getAll();
            LstPlace = AssociationPlaceMultipanel(LstPlace, LstMultiPanel);
        }

        /// <summary>
        /// Clear de toutes les places du plan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPlaceButton(Object sender, RoutedEventArgs e)
        {
            clearPlace();
        }

        private void clearPlace()
        {
            foreach (Plan plan in AllPlan)
            {
                plan.ClearPlan();
            }

            LstPlace.Clear();
            LstTotem.Clear();
            LstAllObject.Clear();
            infoPanel.clearObjet();
        }

        #endregion Dessin

        #region Event

        /// <summary>
        /// fonction de gestion de l'evenement lancé lors du clique sur une place
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoSelectionPlace(object sender, EventArgs e)
        {
            SmgObjView ObjClicked = sender as SmgObjView;
            if (ObjClicked != null)
            {
                // on parcours tous nos objet
                foreach (SmgObj obj in LstAllObject)
                {
                    // on cherhce l'objet qui correspond a l'objet de dessin
                    if (ObjClicked.NameObj.Equals(obj.name))
                    {
                        if (ObjClicked.isSelected) // l'objet est selectionné on l'ajoute au panneau d'informations
                            infoPanel.addObj(obj);
                        else
                            infoPanel.removeObj(obj); // l'objet est deselectionné on le supprime du paneau d'information
                    }
                }

                if (infoPanel.NbObject == 0)
                    InfoExpander.Visibility = Visibility.Hidden;
                else
                    InfoExpander.Visibility = Visibility.Visible;
            }

            foreach (Plan plan in AllPlan)
            {
                plan.UpdatePlan(sender, e);
            }
        }

        private void UpdateInterface(string oldObjName, SmgObj newObj)
        {
            foreach (Plan plan in AllPlan)
            {
                plan.updateObject(oldObjName, newObj);
            }
        }

        private void UpdateBorderColor(Color color)
        {
            foreach (Plan plan in AllPlan)
            {
                plan.UpdateBorderPlace(color);
            }
        }

        private void UpdateFillColor(Color color)
        {
            foreach (Plan plan in AllPlan)
            {
                plan.UpdateColorPlace(color);
            }
        }

        private void VerificationRadioPlan(object sender, RoutedEventArgs e)
        {
            List<int> lstIdTotem = new List<int>();
            foreach (Totem totem in LstTotem)
            {
                lstIdTotem.Add(totem.IdTotemRadio);
            }
            foreach (Plan plan in AllPlan)
                plan.verificationRadio(lstIdTotem);
        }

        private void VerificationComptagePlan(object sender, RoutedEventArgs e)
        {
            List<int> lstIdTotem = new List<int>();
            foreach (Totem totem in LstTotem)
            {
                lstIdTotem.Add(totem.IdTotemRadio);
            }
            foreach (Plan plan in AllPlan)
                plan.verificationComptageRadio(lstIdTotem);
        }

        #endregion Event

        public List<Place> AssociationPlaceMultipanel(List<Place> lstPlace, List<Multipanel> lstMulti)
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
    }
}
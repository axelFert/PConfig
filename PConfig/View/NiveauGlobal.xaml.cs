using PConfig.Conf;
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
    /// Logique d'interaction pour NiveauGlobal.xaml
    /// </summary>
    public partial class NiveauGlobal : UserControl, IAbstractNiveau
    {
        public List<Plan> AllPlan { get; set; }
        public List<Place> LstPlace { get; set; }
        public List<Totem> LstTotem { get; set; }
        private List<SmgObj> LstAllObject = new List<SmgObj>();
        private List<Compteur> LstCompteur { get; set; }

        private List<Multipanel> LstMultiPanel { get; set; }

        public event EventHandler SelectionEventHandler;

        private List<ZoomBorder> lstZoom = new List<ZoomBorder>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(NiveauGlobal));

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="niveau">liste des inforamtion de chaque niveau</param>
        public NiveauGlobal(List<PlanInfo> niveau, List<SmgObj> lstObj, List<Place> lstPlace, List<Totem> lstTotem)
        {
            InitializeComponent();
            LstAllObject = lstObj;
            LstPlace = lstPlace;
            LstTotem = lstTotem;
            AllPlan = new List<Plan>();

            int nblevel = (int)Math.Ceiling(Math.Sqrt(niveau.Count));

            for (int i = 0; i < nblevel; i++)
            {
                // creatio des colonnes
                ColumnDefinition gridCol = new ColumnDefinition();
                GridNiveau.ColumnDefinitions.Add(gridCol);
            }
            int countLvl = 0;
            int countrow = 0;

            while (countLvl != niveau.Count)
            {
                // Create Rows
                RowDefinition gridRow = new RowDefinition();
                GridNiveau.RowDefinitions.Add(gridRow);
                for (int i = 0; i < nblevel; i++)
                {
                    if (countLvl != niveau.Count)
                    {
                        ZoomBorder zm = new ZoomBorder();
                        zm.BorderBrush = Brushes.Black;
                        zm.BorderThickness = new Thickness(0, 0, 1, 1);
                        zm.ClipToBounds = true;

                        PlanInfo niv = niveau[countLvl];
                        Plan plan = new Plan(niv.Path);
                        plan.IdZone = niv.Zone;
                        plan.InfoEventHandler += InfoSelectionPlace;
                        zm.Child = plan;

                        lstZoom.Add(zm);

                        Grid.SetRow(zm, countrow);
                        Grid.SetColumn(zm, i);
                        GridNiveau.Children.Add(zm);
                        AllPlan.Add(plan);
                        countLvl++;
                    }
                }
                countrow++;
            }
            DrawAllObject();
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
            //lstZoom.ForEach(zm => zm.Reset());
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
            AllPlan.ForEach(pln => pln.ClearPlan());

            LstPlace.Clear();
            LstTotem.Clear();
            LstAllObject.Clear();
        }

        public void UpdateAffichage(MODE_AFFICHAGE_OBJET newMode)
        {
            AllPlan.ForEach(pln => pln.UpdateAffichage(newMode));
        }

        public void UpdateAffichageTotem(MODE_AFFICHAGE_OBJET newMode)
        {
            AllPlan.ForEach(pln => pln.UpdateAffichageTotem(newMode));
        }

        #endregion Dessin

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

        /// <summary>
        /// fonction de gestion de l'evenement lancé lors du clique sur une place
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoSelectionPlace(object sender, EventArgs e)
        {
            AllPlan.ForEach(pln => pln.UpdatePlan(sender, e));
            if (SelectionEventHandler != null)
            {
                SelectionEventHandler(sender, new EventArgs());
            }
        }

        public void updateViewCompteur(Compteur cpt)
        {
            AllPlan.ForEach(pl => pl.SelectionCompteur(cpt.LstPanMac));
        }

        public void UpdateColor()
        {
            AllPlan.ForEach(pl => pl.UpdateColor());
        }

        public void SelectionSmgObj(int pan, int mac)
        {
            AllPlan.ForEach(pln => pln.SelectionSmgObj(pan, mac));
        }

        public void SelectionTypePlace(string type)
        {
            AllPlan.ForEach(pln => pln.SelectionTypePlace(type));
        }
    }
}
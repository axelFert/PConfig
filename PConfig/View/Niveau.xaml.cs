using PConfig.Model;
using PConfig.Model.DAO;
using PConfig.View.ObjetPlan;
using PConfig.View.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfig.View
{
    /// <summary>
    /// Logique d'interaction pour Niveau.xaml
    /// </summary>
    public partial class Niveau : UserControl, IAbstractNiveau
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Nom { get; set; }
        public int Zone { get; set; }
        public Plan DrawCanvas { get; set; }
        public List<Place> LstPlace { get; set; }
        public List<Totem> LstTotem { get; set; }
        private List<SmgObj> LstAllObject = new List<SmgObj>();

        private List<Multipanel> LstMultiPanel { get; set; }

        public TotemDAO TotemDao { get; set; }
        public PlaceDAO PlaceDao { get; set; }
        public MultipanelDAO MultipanelDao { get; set; }

        public event EventHandler SelectionEventHandler;

        //private Legende Legende { get; set; }

        /// <summary>
        /// Creation d'un niveau de parking
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="name"></param>
        /// <param name="zone"></param>
        public Niveau(string fileName, string name, int zone, List<SmgObj> lstObj)
        {
            InitializeComponent();
            Nom = name;
            Zone = zone;
            LstAllObject = lstObj;
            LstPlace = new List<Place>();
            LstTotem = new List<Totem>();

            DrawCanvas = new Plan(fileName);
            border.Child = DrawCanvas;

            // ajout d'un event pour la remonter d'info lors du click sur une place
            DrawCanvas.InfoEventHandler += InfoSelectionPlace;

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
            foreach (SmgObj obj in LstAllObject)
            {
                if ((obj as Place) != null)
                {
                    LstPlace.Add(obj as Place);
                }
                else if ((obj as Totem) != null)
                {
                    LstTotem.Add(obj as Totem);
                }
            }

            DrawCanvas.dessinerPlace(LstPlace);
            DrawCanvas.dessinerTotem(LstTotem);
            //border.Reset();
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
            DrawCanvas.ClearPlan();
            LstPlace.Clear();
            LstTotem.Clear();
            LstAllObject.Clear();
        }

        public void UpdateAffichage(MODE_AFFICHAGE_OBJET newMode)
        {
            DrawCanvas.UpdateAffichage(newMode);
        }

        public void UpdateAffichageTotem(MODE_AFFICHAGE_OBJET newMode)
        {
            DrawCanvas.UpdateAffichageTotem(newMode);
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
            if (SelectionEventHandler != null)
            {
                SelectionEventHandler(sender, new EventArgs());
            }
        }

        private void UpdateInterface(string oldObjName, SmgObj newObj)
        {
            DrawCanvas.updateObject(oldObjName, newObj);
        }

        private void UpdateBorderColor(Color color)
        {
            DrawCanvas.UpdateBorderPlace(color);
        }

        private void UpdateFillColor(Color color)
        {
            DrawCanvas.UpdateColorPlace(color);
        }

        #endregion Event

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

        private void VerificationRadioPlan(object sender, RoutedEventArgs e)
        {
            List<int> lstIdTotem = new List<int>();
            foreach (Totem totem in LstTotem)
            {
                lstIdTotem.Add(totem.IdTotemRadio);
            }
            this.DrawCanvas.verificationRadio(lstIdTotem);
        }

        private void VerificationComptagePlan(object sender, RoutedEventArgs e)
        {
            List<int> lstIdTotem = new List<int>();
            foreach (Totem totem in LstTotem)
            {
                lstIdTotem.Add(totem.IdTotemRadio);
            }
            this.DrawCanvas.verificationComptageRadio(lstIdTotem);
        }

        public void updateViewCompteur(Compteur cpt)
        {
            DrawCanvas.SelectionCompteur(cpt.LstPanMac);
        }

        public void SelectionSmgObj(int pan, int mac)
        {
            DrawCanvas.SelectionSmgObj(pan, mac);
        }

        public void UpdateColor()
        {
            DrawCanvas.UpdateColor();
        }

        public void SelectionTypePlace(string type)
        {
            DrawCanvas.SelectionTypePlace(type);
        }
    }
}
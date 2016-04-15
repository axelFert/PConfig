using PConfig.Model;
using PConfig.View.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PConfig.View
{
    /// <summary>
    /// le canvas sur lequel on va placer les objet graphique
    /// </summary>
    public class Plan : Canvas
    {
        public int IdZone { get; set; }

        /// <summary>
        /// LOGGER
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler InfoEventHandler;

        private List<SmgObjView> LstSmgObjPlan;

        /// <summary>
        /// la liste des objet graphiques représentant les places
        /// </summary>

        public Plan(string fichier)
        {
            // clear le calque precédent

            LstSmgObjPlan = new List<SmgObjView>();
            Children.Clear();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(fichier, UriKind.Absolute));
            Width = imageBrush.ImageSource.Width;
            Height = imageBrush.ImageSource.Height;
            Background = imageBrush;
        }

        #region dessin

        /// <summary>
        /// dessinner toutes les places sur le plan
        /// </summary>
        /// <param name="lstPlace">la liste des place a dessiner</param>
        public void dessinerPlace(List<Place> lstPlace)
        {
            foreach (Place place in lstPlace)
            {
                PlaceView placeV = new PlaceView(place);

                placeV.SelectionPlace += new EventHandler(PlaceSelectionEventHandler);
                Children.Add(placeV);
                Children.Add(placeV.text);
                LstSmgObjPlan.Add(placeV);
            }
        }

        /// <summary>
        /// FOnction de dessin d'une seul place
        /// </summary>
        /// <param name="place"></param>
        public void dessinerPlace(Place place)
        {
            PlaceView placeV = new PlaceView(place);
            placeV.SelectionPlace += new EventHandler(PlaceSelectionEventHandler);
            Children.Add(placeV);
            Children.Add(placeV.text);
            LstSmgObjPlan.Add(placeV);
        }

        /// <summary>
        /// Fonction de dessin d'une liste de totem
        /// </summary>
        /// <param name="lstTotem"></param>
        public void dessinerTotem(List<Totem> lstTotem)
        {
            foreach (Totem tot in lstTotem)
            {
                TotemView totem = new TotemView(tot);
                totem.SelectionTotem += new EventHandler(PlaceSelectionEventHandler);
                Children.Add(totem);
                Children.Add(totem.text);
                LstSmgObjPlan.Add(totem);
            }
        }

        /// <summary>
        /// Fonction de dessin d'un unique totem
        /// </summary>
        /// <param name="tot"></param>
        public void dessinerTotem(Totem tot)
        {
            TotemView totem = new TotemView(tot);
            totem.SelectionTotem += new EventHandler(PlaceSelectionEventHandler);
            Children.Add(totem);
            Children.Add(totem.text);
            LstSmgObjPlan.Add(totem);
        }

        /// <summary>
        /// Supprimer tous les objets graphiques du plan
        /// </summary>
        public void ClearPlan()
        {
            Children.Clear();
        }

        /// <summary>
        /// Met a joàur un obet graphique en passant l'ancien nom de l'objet et l'objet par leque on
        /// cva le remplacers
        /// </summary>
        /// <param name="oldObjName">nom del'ancien obj</param>
        /// <param name="newObj">Objet de reamplacement</param>
        public void updateObject(string oldObjName, SmgObj newObj)
        {
            SmgObjView oldChild = null;
            foreach (var child in Children)
            {
                if (child as SmgObjView != null)
                {
                    SmgObjView smgChild = child as SmgObjView;
                    if (smgChild.NameObj.Equals(oldObjName))
                    {
                        oldChild = smgChild;
                    }
                }
            }
            if (oldChild != null)
            {
                Place place;
                Totem totem;
                Children.Remove(oldChild.text);
                Children.Remove(oldChild);
                if ((totem = (newObj as Totem)) != null)
                {
                    dessinerTotem(totem);
                }
                else if ((place = (newObj as Place)) != null)
                {
                    dessinerPlace(place);
                }
            }
        }

        public void UpdateColorPlace(Color color)
        {
            foreach (var item in Children)
            {
                PlaceView place = null;
                if ((place = (item as PlaceView)) != null)
                {
                    place.Fill = new SolidColorBrush(color);
                }
            }
        }

        public void UpdateBorderPlace(Color color)
        {
            foreach (var item in Children)
            {
                PlaceView place = null;
                if ((place = (item as PlaceView)) != null)
                {
                    place.Stroke = new SolidColorBrush(color);
                    place.StrokeThickness = 2;
                }
            }
        }

        public void UpdateAffichage(MODE_AFFICHAGE_OBJET newMode)
        {
            foreach (SmgObjView obj in LstSmgObjPlan)
            {
                if (obj as PlaceView != null)
                    obj.updateAffichageProp(newMode);
            }
        }

        public void UpdateAffichageTotem(MODE_AFFICHAGE_OBJET newMode)
        {
            foreach (SmgObjView obj in LstSmgObjPlan)
            {
                if (obj as TotemView != null)
                    obj.updateAffichageProp(newMode);
            }
        }

        #endregion dessin

        #region Event handle des objet

        private void PlaceSelectionEventHandler(object sender, object EventArgs)
        {
            UpdatePlan(sender, EventArgs);

            //remonter les info de la place cliquer à niveau superieur
            if (InfoEventHandler != null)
            {
                InfoEventHandler(sender, new EventArgs());
            }
        }

        public void UpdatePlan(object sender, object EventArgs)
        {
            var objectClicked = sender as SmgObjView;
            //surligner les place avec le même totem
            foreach (UIElement child in Children)
            {
                var obj = child as SmgObjView; // only one cast
                if (obj != null)
                {
                    //obj.UpdateState(objectClicked.TotemRadio, objectClicked.TotemRadio);
                    obj.UpdateState(sender as SmgObjView, false);
                }
            }
        }

        public void UpdateColor()
        {
            LstSmgObjPlan.ForEach(obj => obj.UpdateColor());
        }

        internal void SelectionCompteur(List<string> LstPanMac)
        {
            foreach (UIElement child in Children)
            {
                PlaceView obj = child as PlaceView; // only one cast
                if (obj != null)
                {
                    if (LstPanMac.Contains(obj.Pan + "/" + obj.Mac))
                    {
                        obj.ObjSelect(true);
                    }
                    else
                    {
                        obj.ObjSelect(false);
                    }
                }
            }
        }

        public void SelectionSmgObj(int pan, int mac)
        {
            List<SmgObjView> objView = LstSmgObjPlan.Where(obj => obj.Pan == pan && obj.Mac == mac).ToList();
            if (objView.Count > 0)
                PlaceSelectionEventHandler(objView[0], new object());
        }

        public void SelectionTypePlace(string type)
        {
            foreach (UIElement child in Children)
            {
                PlaceView obj = child as PlaceView; // only one cast
                if (obj != null)
                {
                    if (obj.Category.Equals(type))
                    {
                        obj.ObjSelect(true);
                    }
                    else
                    {
                        obj.ObjSelect(false);
                    }
                }
            }
        }

        public List<SmgObjView> getSelected()
        {
            List<SmgObjView> lstRetour = new List<SmgObjView>();
            foreach (SmgObjView obj in LstSmgObjPlan)
            {
                if (obj.isSelected)
                    lstRetour.Add(obj);
            }
            return lstRetour;
        }

        #endregion Event handle des objet

        #region deprecated

        public void verificationRadio(List<int> lstRadioId)
        {
            foreach (SmgObjView child in LstSmgObjPlan)
            {
                var obj = child as PlaceView;
                if (obj != null)
                {
                    obj.verifRadio(lstRadioId);
                }
            }
        }

        public void verificationComptageRadio(List<int> lstRadioId)
        {
            foreach (SmgObjView child in LstSmgObjPlan)
            {
                var obj = child as PlaceView;
                if (obj != null)
                {
                    obj.verifComptage(lstRadioId);
                }
            }
        }

        #endregion deprecated
    }
}
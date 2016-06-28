using PConfig.Model;
using PConfig.View.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfig.View.ObjetPlan
{
    public class PlaceView : SmgObjView
    {
        public Double Angle { get; set; }

        public int Id { get; set; }

        public Point Corner { get; set; }

        public bool isHighlighted { get; set; }

        public string Category { get; }

        public Dictionary<int, int> LstTotemDispalyer { get; set; }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return new RectangleGeometry(new Rect(new Size(Width, Height)));
            }
        }

        /// <summary>
        /// Constructeur du rectangle de la place
        /// </summary>
        /// <param name="place"></param>
        public PlaceView(Place place) : base()
        {
            //pan mac
            Pan = place.ID_pan;
            Mac = place.ID_mac;
            TotemRadio = place.IdTotemRadio;

            Category = place.category;
            Id = place.Numero;
            NameObj = place.name;
            IdPanel = place.ID_panels;
            LstTotemDispalyer = place.LstTotemDispalyer;
            text.Content = Category;
            Etat = ETAT_OBJET_PLAN.NONE_PLACE;
            initObjetGraphique(place);
            text.FontSize = (Height / 3) * 72 / 96;
        }

        public PlaceView(int numero) : base()
        {
            Id = numero;
            //initObjetGraphique(top, width, height, rotation);
            text.Content = numero;
            Etat = ETAT_OBJET_PLAN.NONE_PLACE;
        }

        private void initObjetGraphique(Point top, double width, double height, double rotation)
        {
            //propriere graphique
            Corner = top;
            Width = width;
            text.Width = width;
            Height = height;
            text.Height = height;
            Angle = rotation;

            // ugly mais plus facile que de faire du binding de différent objet sur un même canvas
            SetValue(Canvas.LeftProperty, Corner.X);
            SetValue(Canvas.TopProperty, Corner.Y);

            text.SetValue(Canvas.LeftProperty, Corner.X);
            text.SetValue(Canvas.TopProperty, Corner.Y);

            LayoutTransform = new RotateTransform(Angle, 0, height / 2.0);
            text.LayoutTransform = new RotateTransform(Angle + 90, 0, height / 2.0);
            text.FontSize = (Height / 3) * 72 / 96;
            UpdateColor();
        }

        private void initObjetGraphique(Place place)
        {
            //propriere graphique
            Corner = new Point(place.X, place.Y);
            Width = place.Longueur;
            text.Width = place.Longueur;
            Height = place.Hauteur;
            text.Height = place.Hauteur;
            Angle = place.Angle;

            // ugly mais plus facile que de faire du binding de différent objet sur un même canvas
            SetValue(Canvas.LeftProperty, Corner.X);
            SetValue(Canvas.TopProperty, Corner.Y);

            text.SetValue(Canvas.LeftProperty, Corner.X);
            text.SetValue(Canvas.TopProperty, Corner.Y);

            LayoutTransform = new RotateTransform(Angle);
            text.LayoutTransform = new RotateTransform(Angle);
            text.FontSize = (Height / 3) * 72 / 96;
            UpdateColor();
        }

        public override void UpdateState(SmgObjView sender, bool multiView)
        {
            if (!multiView)
            {
                Etat = ETAT_OBJET_PLAN.NONE_PLACE;
                isSelected = false;
            }
            if (sender as TotemView != null)
            {
                if (SmgUtilsIHM.IS_RADIO_LINK)
                {
                    if (sender.TotemRadio.Equals(this.TotemRadio))
                    {
                        Etat = ETAT_OBJET_PLAN.COMPTAGE_RADIO;
                        isSelected = true;
                    }
                }
                else {
                    if (sender.TotemRadio.Equals(this.TotemRadio))
                    {
                        if (IdPanel == 0)
                            Etat = ETAT_OBJET_PLAN.NONE_PLACE;
                        else
                        {
                            Etat = ETAT_OBJET_PLAN.COMPTAGE_MULTIPANEL;
                            isSelected = true;
                        }
                    }
                    else if (LstTotemDispalyer != null && LstTotemDispalyer.ContainsValue(sender.TotemRadio))
                    {
                        Etat = ETAT_OBJET_PLAN.COMPTAGE_MULTIPANEL;
                        isSelected = true;
                    }
                }
            }
            else if (sender as PlaceView != null)
            {
                if (sender == this)
                {
                    Etat = ETAT_OBJET_PLAN.COMPTAGE_RADIO;
                    isSelected = true;
                }
                else
                {
                    if (!SmgUtilsIHM.IS_RADIO_LINK)
                    {
                        if ((sender as PlaceView).Category == Category && Category != "normal")
                        {
                            Etat = ETAT_OBJET_PLAN.ELEMENT_LIE;
                            isSelected = true;
                        }
                    }
                }
            }
            else if ((sender as MatView) != null)
            {
                if (SmgUtilsIHM.IS_RADIO_LINK)
                {
                    if (sender.TotemRadio.Equals(this.TotemRadio))
                    {
                        Etat = ETAT_OBJET_PLAN.COMPTAGE_RADIO;
                        isSelected = true;
                    }
                }
                else {
                    if ((sender as MatView).lstPanMac.Contains(Pan + "/" + Mac))
                    {
                        Etat = ETAT_OBJET_PLAN.COMPTAGE_MULTIPANEL;
                        isSelected = true;
                    }
                }
            }
            UpdateColor();
        }

        public void verifRadio(List<int> lstRadioId)
        {
            Etat = ETAT_OBJET_PLAN.NONE_PLACE;
            if (lstRadioId.Contains(TotemRadio))
            {
                Etat = ETAT_OBJET_PLAN.CONNEXION_RADIO;
            }
            UpdateColor();
        }

        public void verifComptage(List<int> lstRadioId)
        {
            Etat = ETAT_OBJET_PLAN.NONE_PLACE;
            if (lstRadioId.Contains(TotemRadio))
            {
                if (IdPanel == 0)
                    Etat = ETAT_OBJET_PLAN.COMPTAGE_RADIO;
            }
            UpdateColor();
        }

        public override void updateAffichageProp(MODE_AFFICHAGE_OBJET newMode)
        {
            string valeurAffichage = "";
            switch (newMode)
            {
                case MODE_AFFICHAGE_OBJET.ID:
                    valeurAffichage = Id + "";
                    break;

                case MODE_AFFICHAGE_OBJET.NOM:
                    valeurAffichage = NameObj;
                    break;

                case MODE_AFFICHAGE_OBJET.PAN_MAC:
                    valeurAffichage = Pan + "/" + Mac;
                    break;

                case MODE_AFFICHAGE_OBJET.TYPE:
                    valeurAffichage = Category;
                    break;

                default:
                    valeurAffichage = NameObj;
                    break;
            }
            this.text.Content = valeurAffichage;
        }

        public override void ObjSelect(bool select)
        {
            if (select)
            {
                isSelected = true;
                Etat = ETAT_OBJET_PLAN.COMPTAGE_MULTIPANEL;
            }
            else {
                isSelected = false;
                Etat = ETAT_OBJET_PLAN.NONE_PLACE;
            }
            UpdateColor();
        }

        public override void UpdateColor()
        {
            if (SmgUtilsIHM.TAILLE_POLICE_AUTO)
                text.FontSize = (Height / 3) * 72 / 96;
            else
                text.FontSize = SmgUtilsIHM.TAILLE_POLICE;

            ColorState colors = SmgUtilsIHM.getColorEtat(Etat);
            if (colors != null)
            {
                Fill = new SolidColorBrush(colors.CouleurRemplissage);
                Stroke = new SolidColorBrush(colors.CouleurBordure);
                StrokeThickness = SmgUtilsIHM.EPAISSEUR_TRAIT;
                text.Foreground = new SolidColorBrush(colors.CouleurBordure);
                //text.FontSize = SmgUtilsIHM.TAILLE_POLICE;
            }
        }
    }
}
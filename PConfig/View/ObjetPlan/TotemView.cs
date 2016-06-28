using PConfig.Model;
using PConfig.View.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfig.View.ObjetPlan
{
    internal class TotemView : SmgObjView
    {
        public double Diametre { get; set; }
        public Point Centre { get; set; }

        //public event EventHandler SelectionTotem;

        protected override Geometry DefiningGeometry
        {
            get
            {
                return new EllipseGeometry(Centre, Diametre / 2, Diametre / 2);
            }
        }

        public TotemView(Totem totem) : base()
        {
            this.Centre = new Point(totem.XCentre, totem.YCentre);
            Diametre = totem.Diametre;
            NameObj = totem.name;
            this.Pan = totem.ID_pan;
            this.Mac = totem.ID_mac;
            this.TotemRadio = totem.IdTotemRadio;
            IdPanel = totem.ID_panels;
            text.Content = IdPanel;
            Etat = ETAT_OBJET_PLAN.NONE_TOTEM;
            initObjetGraphique();
        }

        public TotemView(Point centre, double diametre, int numero) : base()
        {
            Centre = centre;
            Diametre = diametre;
            IdPanel = numero;
            text.Content = IdPanel;
            Etat = ETAT_OBJET_PLAN.NONE_TOTEM;
            initObjetGraphique();
        }

        private void initObjetGraphique()
        {
            Stroke = new SolidColorBrush(Colors.Yellow);
            Fill = new SolidColorBrush(Color.FromArgb(150, 0, 150, 0));
            StrokeThickness = 2;
            text.SetValue(Canvas.LeftProperty, Centre.X - Diametre / 2);
            text.SetValue(Canvas.TopProperty, Centre.Y - Diametre / 2);

            UpdateColor();
        }

        public override void UpdateState(SmgObjView sender, Boolean multiView = false)
        {
            if (!multiView)
            {
                Etat = ETAT_OBJET_PLAN.NONE_TOTEM;
                isSelected = false;
            }

            //si on a cliquer sur une place
            if ((sender as PlaceView) != null)
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
                    if (sender.TotemRadio.Equals(this.TotemRadio) && sender.IdPanel != 0)
                    {
                        Etat = ETAT_OBJET_PLAN.COMPTAGE_MULTIPANEL;
                        isSelected = true;
                    }
                    else
                    {
                        if ((sender as PlaceView).LstTotemDispalyer.ContainsKey(this.IdPanel))
                        {
                            Etat = ETAT_OBJET_PLAN.COMPTAGE_MULTIPANEL;
                            isSelected = true;
                        }
                    }
                }
            }
            else // sinon
            {
                if (sender == this)
                {
                    Etat = ETAT_OBJET_PLAN.COMPTAGE_RADIO;
                    isSelected = true;
                }
            }
            UpdateColor();
        }

        public override void updateAffichageProp(MODE_AFFICHAGE_OBJET newMode)
        {
            string valeurAffichage = "";
            switch (newMode)
            {
                case MODE_AFFICHAGE_OBJET.ID:
                    valeurAffichage = IdPanel + "";
                    break;

                case MODE_AFFICHAGE_OBJET.NOM:
                    valeurAffichage = NameObj;
                    break;

                case MODE_AFFICHAGE_OBJET.PAN_MAC:
                    valeurAffichage = Pan + " / " + Mac;
                    break;

                case MODE_AFFICHAGE_OBJET.TYPE:
                    valeurAffichage = "Totem";
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
                Etat = ETAT_OBJET_PLAN.NONE_TOTEM;
            }
            UpdateColor();
        }

        public override void UpdateColor()
        {
            ColorState colors = SmgUtilsIHM.getColorEtat(Etat);
            if (colors != null)
            {
                Fill = new SolidColorBrush(colors.CouleurRemplissage);
                Stroke = new SolidColorBrush(colors.CouleurBordure);
                StrokeThickness = SmgUtilsIHM.EPAISSEUR_TRAIT;
                text.Foreground = new SolidColorBrush(colors.CouleurBordure);
            }
        }
    }
}
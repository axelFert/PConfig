using PConfig.Model;
using PConfig.View.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfig.View.ObjetPlan
{
    internal class MatView : SmgObjView
    {
        private Point sommetA, sommetB, sommetC;

        public List<string> lstPanMac
        {
            get; private set;
        }

        private PathGeometry pathGeometry;

        // public event EventHandler SelectionMat;

        protected override Geometry DefiningGeometry
        {
            get
            {
                return pathGeometry;
            }
        }

        public MatView(Mat mat) : base()
        {
            Pan = mat.ID_pan;
            Mac = mat.ID_mac;
            TotemRadio = mat.IdTotemRadio;
            lstPanMac = mat.Afficheurs.Values.ToList().SelectMany(cp => cp.LstPanMac).ToList();

            Etat = ETAT_OBJET_PLAN.NONE_MAT;
            initObjetGraphique(new Point(mat.XCentre, mat.YCentre), mat.TailleCote);
        }

        public MatView(Point centre, double cote, int numero) : base()
        {
            Etat = ETAT_OBJET_PLAN.NONE_MAT;
            IdPanel = numero;
            text.Content = IdPanel;
            initObjetGraphique(centre, cote);
        }

        private void initObjetGraphique(Point centre, double cote)
        {
            this.sommetA = new Point(centre.X, centre.Y - (cote * 2 / 3));
            this.sommetB = new Point(centre.X + (cote / 2), centre.Y + (cote / 3));
            this.sommetC = new Point(centre.X - (cote / 2), centre.Y + (cote / 3));

            pathGeometry = new PathGeometry();

            pathGeometry.FillRule = FillRule.Nonzero;

            PathFigure figure = new PathFigure();
            figure.StartPoint = sommetA;
            figure.IsClosed = true;

            pathGeometry.Figures.Add(figure);

            LineSegment l1 = new LineSegment();
            l1.Point = sommetB;
            LineSegment l2 = new LineSegment();
            l2.Point = sommetC;

            figure.Segments.Add(l1);
            figure.Segments.Add(l2);

            Stroke = new SolidColorBrush(Colors.Yellow);
            Fill = new SolidColorBrush(Color.FromArgb(150, 0, 150, 0));
            StrokeThickness = 2;

            text.SetValue(Canvas.LeftProperty, centre.X - cote / 2);
            text.SetValue(Canvas.TopProperty, centre.Y);

            UpdateColor();
        }

        public override void updateAffichageProp(MODE_AFFICHAGE_OBJET newMode)
        {
            //throw new NotImplementedException();
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
                string panMac = sender.Pan + "/" + Mac;
                if (SmgUtilsIHM.IS_RADIO_LINK)
                {
                    if (sender.TotemRadio == int.Parse(Pan + "" + Mac))
                    {
                        Etat = ETAT_OBJET_PLAN.COMPTAGE_RADIO;
                        isSelected = true;
                    }
                }
                else {
                    if (lstPanMac.Contains(panMac))
                    {
                        Etat = ETAT_OBJET_PLAN.COMPTAGE_MULTIPANEL;
                        isSelected = true;
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
    }
}
using PConfig.View.ObjetPlan;
using PConfig.View.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PConfig.View.Dessin
{
    public class PlanDessin : Canvas
    {
        // point lors des mouvement
        private Point currentPoint = new Point();

        private Point startingPoint = new Point();

        private Line line = null;

        private bool ligneAjoute = false;
        private bool creationLigne = false;

        private double rotation;

        private double heighRect;

        private Rectangle rect = null;

        private DateTime debutclic;

        public TYPE_DESSIN TypeDessin { private get; set; }

        public PlanDessin()
        {
            MouseDown += Canvas_MouseDown;
            MouseMove += Canvas_MouseMove;
            MouseUp += Canvas_MouseUp;
            Height = 1000;
            Width = 2000;

            SolidColorBrush color = new SolidColorBrush(Colors.Black);
            Background = color;

            TypeDessin = TYPE_DESSIN.PLACE;
        }

        public PlanDessin(string fichier)
        {
            // clear le calque
            Children.Clear();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(fichier, UriKind.RelativeOrAbsolute));
            Width = imageBrush.ImageSource.Width;
            Height = imageBrush.ImageSource.Height;
            Background = imageBrush;
            MouseDown += Canvas_MouseDown;
            MouseMove += Canvas_MouseMove;
            MouseUp += Canvas_MouseUp;

            TypeDessin = TYPE_DESSIN.PLACE;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            debutclic = DateTime.Now;
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                switch (TypeDessin)
                {
                    case TYPE_DESSIN.LIBRE:
                        if (!ligneAjoute && !creationLigne)
                        {
                            currentPoint = e.GetPosition(this);
                            startingPoint = e.GetPosition(this);
                            line = new Line();

                            creationLigne = true;

                            line.Stroke = new SolidColorBrush(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_PLACE).CouleurBordure);
                            line.StrokeThickness = 2;
                            line.X1 = currentPoint.X;
                            line.Y1 = currentPoint.Y;
                            Children.Add(line);
                        }
                        else if (ligneAjoute)
                        {
                            currentPoint = e.GetPosition(this);

                            line = null;
                            ligneAjoute = false;
                            creationLigne = false;
                            Children.Remove(line);
                            //selection du site a charger
                            PopupNombrePlace popup = new PopupNombrePlace();
                            popup.ShowDialog();
                            if (!popup.IsCancelled)
                                diviserRectangle(rect, popup.NbPlaceLargeur, popup.NbPlaceHauteur, popup.NumeroPlace);
                            else
                                Children.Remove(rect);
                        }
                        break;

                    case TYPE_DESSIN.MAT:
                        currentPoint = e.GetPosition(this);
                        startingPoint = e.GetPosition(this);
                        DessinerMat(currentPoint);
                        break;

                    case TYPE_DESSIN.TOTEM:
                        currentPoint = e.GetPosition(this);
                        startingPoint = e.GetPosition(this);
                        DessinerTotem(currentPoint);
                        break;

                    case TYPE_DESSIN.PLACE:
                        currentPoint = e.GetPosition(this);
                        startingPoint = e.GetPosition(this);
                        rect = new Rectangle
                        {
                            Stroke = new SolidColorBrush(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_PLACE).CouleurBordure),
                            StrokeThickness = 2
                        };
                        Canvas.SetLeft(rect, currentPoint.X);
                        Canvas.SetTop(rect, currentPoint.X);
                        this.Children.Add(rect);
                        break;
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - debutclic).TotalMilliseconds < 250)
            {
                if (rect != null) { Children.Remove(rect); rect = null; }
                if (line != null) { Children.Remove(line); line = null; }
                return;
            }
            if (e.LeftButton == MouseButtonState.Released && e.ChangedButton == MouseButton.Left)
            {
                switch (TypeDessin)
                {
                    case TYPE_DESSIN.LIBRE:
                        if (creationLigne && !ligneAjoute)
                        {
                            Vector v = currentPoint - startingPoint;
                            heighRect = Math.Sqrt(v.X * v.X + v.Y * v.Y);
                            startingPoint = new Point((line.X2 + line.X1) / 2, (line.Y2 + line.Y1) / 2);
                            rect = new Rectangle
                            {
                                Stroke = new SolidColorBrush(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_PLACE).CouleurBordure),
                                StrokeThickness = 2
                            };
                            rect.Height = heighRect;
                            Children.Add(rect);
                            ligneAjoute = true;
                            creationLigne = false;
                            Children.Remove(line);
                        }

                        break;

                    case TYPE_DESSIN.PLACE:
                        if (rect == null) return;

                        PopupNombrePlace popup = new PopupNombrePlace();
                        popup.ShowDialog();
                        if (popup.IsCancelled)
                        {
                            Children.Remove(rect);
                            break;
                        }

                        int nbHauteur = popup.NbPlaceHauteur;
                        int nbLargeur = popup.NbPlaceLargeur;

                        int hauteurNouvRect = (Int32)rect.ActualHeight / nbHauteur;
                        int LargeurNouvRect = (Int32)rect.ActualWidth / nbLargeur;
                        int nbRectlargeur = (Int32)rect.ActualWidth / LargeurNouvRect;
                        int nbRectHauteru = (Int32)rect.ActualHeight / hauteurNouvRect;

                        int topX = (int)Math.Min(currentPoint.X, startingPoint.X);
                        int topY = (int)Math.Min(currentPoint.Y, startingPoint.Y);

                        for (int i = 0; i < nbRectHauteru; i++)
                        {
                            for (int j = 0; j < nbRectlargeur; j++)
                            {
                                Rectangle Rectangle = new Rectangle
                                {
                                    Stroke = new SolidColorBrush(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_PLACE).CouleurBordure),
                                    StrokeThickness = 1
                                };
                                SetLeft(Rectangle, topX + j * LargeurNouvRect);
                                SetTop(Rectangle, topY + i * hauteurNouvRect);
                                Rectangle.Width = LargeurNouvRect - 1;
                                Rectangle.Height = hauteurNouvRect - 1;
                                Children.Add(Rectangle);
                            }
                        }
                        Children.Remove(rect);
                        rect = null;
                        break;
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (TypeDessin)
            {
                case TYPE_DESSIN.LIBRE:
                    if (creationLigne && !ligneAjoute)
                    {
                        if ((e.LeftButton == MouseButtonState.Pressed && line != null))
                        {
                            line.X2 = e.GetPosition(this).X;
                            line.Y2 = e.GetPosition(this).Y;
                            currentPoint = e.GetPosition(this);
                        }
                    }
                    else if (ligneAjoute)
                    {
                        if (rect != null)
                        {
                            currentPoint = e.GetPosition(this);

                            Vector v = currentPoint - startingPoint;
                            double width = Math.Sqrt(v.X * v.X + v.Y * v.Y);
                            double cos_w = v.X / width;
                            double w_rad = Math.Acos(cos_w);
                            double w2 = w_rad * 180.0 / Math.PI;

                            if (currentPoint.Y < startingPoint.Y)
                                w2 = -w2;

                            rect.Width = width;

                            rect.SetValue(Canvas.LeftProperty, startingPoint.X);
                            rect.SetValue(Canvas.TopProperty, startingPoint.Y - rect.Height / 2.0);

                            rect.RenderTransform = new RotateTransform(w2, 0, rect.Height / 2.0);

                            rotation = w2;
                        }
                    }

                    break;

                case TYPE_DESSIN.PLACE:
                    if (e.LeftButton == MouseButtonState.Released || rect == null)
                        return;
                    currentPoint = e.GetPosition(this);
                    var pos = e.GetPosition(this);

                    var x = Math.Min(pos.X, startingPoint.X);
                    var y = Math.Min(pos.Y, startingPoint.Y);

                    var w = Math.Max(pos.X, startingPoint.X) - x;
                    var h = Math.Max(pos.Y, startingPoint.Y) - y;

                    rect.Width = w;
                    rect.Height = h;

                    Canvas.SetLeft(rect, x);
                    Canvas.SetTop(rect, y);

                    break;
            }
        }

        private void diviserRectangle(Rectangle rect, int nbRectLargeur, int nbRectHauteur, int numero)
        {
            Children.Remove(rect);
            Vector v = currentPoint - startingPoint;
            double width = v.Length;
            width = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            double cos_w = v.X / width;
            double w_rad = Math.Acos(cos_w);
            double w2 = w_rad * 180.0 / Math.PI;

            if (currentPoint.Y < startingPoint.Y)
                w2 = -w2;

            double largeurRect = width / nbRectLargeur;
            double decalageX = largeurRect * v.X / v.Length;
            double decalageY = largeurRect * v.Y / v.Length;

            for (int i = 0; i < nbRectHauteur; i++)
            {
                double newX = startingPoint.X;
                double newY = startingPoint.Y + i * heighRect / nbRectHauteur;
                for (int j = 0; j < nbRectLargeur; j++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        //Stroke = new SolidColorBrush(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_TOTEM).CouleurBordure),
                        Stroke = new SolidColorBrush(Colors.Red),
                        StrokeThickness = 1
                    };

                    rectangle.Width = width / nbRectLargeur;
                    rectangle.Height = heighRect / nbRectHauteur;
                    SetLeft(rectangle, newX);
                    SetTop(rectangle, newY - rectangle.Height / 2.0);
                    rectangle.RenderTransform = new RotateTransform(w2, 0, heighRect / 2.0);

                    PlaceView place = new PlaceView(numero++);
                    place.Width = width / nbRectLargeur;
                    place.Height = heighRect / nbRectHauteur;
                    SetLeft(place, newX);
                    SetTop(place, newY - rectangle.Height / 2.0);
                    place.RenderTransform = new RotateTransform(w2, 0, heighRect / 2.0);

                    SetLeft(place.text, newX);
                    SetTop(place.text, newY - rectangle.Height / 2.0);
                    place.text.RenderTransform = new RotateTransform(w2, 0, heighRect / 2.0);
                    place.UpdateColor();

                    Children.Add(place);
                    Children.Add(place.text);

                    newX = newX + decalageX;
                    newY = newY + decalageY;
                }
            }
        }

        private void DessinerTotem(Point currentPoint)
        {
            Ellipse tot = new Ellipse()
            {
                Stroke = new SolidColorBrush(SmgUtilsIHM.getColorEtat(ETAT_OBJET_PLAN.NONE_TOTEM).CouleurBordure),
                StrokeThickness = 2
            };
            tot.Height = tot.Width = SmgUtilsIHM.DIAMETRE_TOTEM;
            tot.SetValue(Canvas.LeftProperty, currentPoint.X - SmgUtilsIHM.DIAMETRE_TOTEM / 2);
            tot.SetValue(Canvas.TopProperty, currentPoint.Y - SmgUtilsIHM.DIAMETRE_TOTEM / 2);
            //this.Children.Add(tot);

            TotemView totem = new TotemView(currentPoint, SmgUtilsIHM.DIAMETRE_TOTEM, 1);
            this.Children.Add(totem);
            this.Children.Add(totem.text);
        }

        private void DessinerMat(Point currentPoint)
        {
            MatView mat = new MatView(currentPoint, SmgUtilsIHM.COTE_MAT, 1);
            this.Children.Add(mat);
            this.Children.Add(mat.text);
            // this.Children.Add(mat.text);
        }

        public void ClearDessin()
        {
            this.Children.Clear();
        }

        public void ChangerClic()
        {
            MouseDown -= Canvas_MouseDown;
            MouseMove -= Canvas_MouseMove;
            MouseUp -= Canvas_MouseUp;
        }
    }
}
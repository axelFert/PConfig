using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PConfig.View.Dessin
{
    public class PlanDessin : Canvas
    {
        private Point currentPoint = new Point();

        public PlanDessin()
        {
            MouseDown += Canvas_MouseDown;
            MouseMove += Canvas_MouseMove_1;
            Height = 500;
            Width = 600;

            SolidColorBrush color = new SolidColorBrush(Colors.White);
            Background = color;
        }

        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
        }

        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();

                line.Stroke = SystemColors.WindowFrameBrush;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;

                currentPoint = e.GetPosition(this);

                this.Children.Add(line);
            }
        }
    }
}
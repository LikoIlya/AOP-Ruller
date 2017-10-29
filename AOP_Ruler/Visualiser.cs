// Ilya Likhoshva
// AOP_Ruler
// Visualiser.cs
// 23.10.2015
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Drawing.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Panel = System.Windows.Controls.Panel;
using Point = System.Drawing.Point;


namespace AOP_Ruler
{
    /// <summary>
    /// Класс визуализации агентов и среды на графических элементах формы
    /// </summary>
    public class Visualiser
    {
        private List<Point> _agentPoints;
        private Color _pointColor;
        private Grid _drawingArea;

        public Visualiser() : this(new List<Point>(), Color.Black)
        {}
        public Visualiser(List<Point> agentPoints, Color pointColor)
        {
            _agentPoints = agentPoints;
            _pointColor = pointColor;
            _drawingArea = new Grid();
            _drawingArea.HorizontalAlignment = HorizontalAlignment.Left;
            _drawingArea.VerticalAlignment = VerticalAlignment.Top;
            Draw(agentPoints, pointColor, _drawingArea);
        }

        public Grid GetGrid()
        {
            return _drawingArea;
        }

        private void Draw(List<Point> agentPoints, Color pointColor, Grid drawingArea)
        {
            List<Line> Lines = new List<Line>(agentPoints.Count - 1);
            int i = 0;
            foreach (Line line in Lines)
                {
                    line.Stroke =
                        new SolidColorBrush(System.Windows.Media.Color.FromRgb(pointColor.R, pointColor.G, pointColor.B));
                    line.X1 = agentPoints[i].X;
                    line.Y1 = agentPoints[i].Y;
                    line.X2 = agentPoints[++i].X;
                    line.Y2 = agentPoints[i].Y;
                    drawingArea.Children.Add(line);
                    drawingArea.UpdateLayout();
                }
        }

        public void AddGrid_FillPanel(Panel Parent)
        {
            double kP;
            double kCh;
            Grid FillGrid = _drawingArea;
            kP = Parent.Width / Parent.Height;
            kCh = FillGrid.Width / FillGrid.Height;
            double k = kCh >= kP ? (Parent.Width / FillGrid.Width) : (Parent.Height / FillGrid.Height);
            //if (kCh >= kP)
            //    k = Parent.Width / _drawingArea.Width;
            //else
            //    k = Parent.Height / _drawingArea.Height;
            ScaleTransform ST = new ScaleTransform(k,k);
            //FillGrid.RenderTransform = ST;
            foreach (UIElement Child in FillGrid.Children)
                {
                    Child.RenderTransform = ST;
                }
            Parent.Children.Add(FillGrid);
        }
    }
}

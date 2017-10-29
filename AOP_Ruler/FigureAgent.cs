using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AOP_Ruler
{
    public class FigureAgent
    {
        public FigureAgent(Color color, List<PointConfig> points)
        {
            Color = color;
            Points = points;
        }
        public Color Color { get; set; }
        public List<PointConfig> Points { get; set; }

        public void Add(PointConfig pointConfig)
        {
            Points.Add(pointConfig);
        }

        public void AddPoint(Point point)
        {
            int id = Points.Count;
            Points.Add(new PointConfig(id, point, MsgAction.Invoke));
        }
        public void Delete(Point point)
        {
            Points.RemoveAll(x => x.Point.Equals(point));
        }

        public void SetStatus(int id, MsgAction status)
        {
            Points[GetIndex(id)].State = status;
        }
        public void SetStatus(Point point, MsgAction status)
        {
            Points[GetIndex(point)].State = status;
        }

        public int GetIndex(int id)
        {
            return Points.FindIndex(x => x.Id.Equals(id));
        }
        public int GetIndex(Point point)
        {
            return Points.FindIndex(x => x.Point.Equals(point));
        }

        public FigureAgent Clone()
        {
            List<PointConfig> temp = Points.Select(a => new PointConfig(a.Id, new Point(a.Point.X, a.Point.Y), a.State)).ToList();
            return new FigureAgent(Color, temp);
        }

        public override string ToString()
        {
            return Color.ToString() + " {Count=" + Points.Count + "}";
        }
    }
}

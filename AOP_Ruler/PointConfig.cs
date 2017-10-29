using System.Drawing;

namespace AOP_Ruler
{
    public class PointConfig
    {
        public PointConfig(int id, Point point, MsgAction state)
        {
            Id = id;
            Point = point;
            State = state;
        }

        public int Id { get; set; }
        public Point Point { get; set; }
        public MsgAction State { get; set; }

        public PointConfig Clone()
        {
            return new PointConfig(Id, new Point(Point.X, Point.Y), State);
        }
    }
}

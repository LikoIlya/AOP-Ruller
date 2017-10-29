using System;
using System.Collections.Generic;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace AOP_Ruler
{
    public class VectorPoint : IComparable<VectorPoint>, IComparer<VectorPoint>
    {
        private Point firstPoint;
        private Point secondPoint;
        
        public VectorPoint() : this(0, 0) { }
        public VectorPoint(int x, int y) : this(0, 0, x, y) { }

        public VectorPoint(int x1, int y1, int x2, int y2)
        {
            this.firstPoint = new Point(x1, y1);
            this.secondPoint = new Point(x2, y2);
        }

        public VectorPoint(Point point) : this(new Point(0, 0), point) { }
        public VectorPoint(Point firstPoint, Point secondPoint)
        {
            this.firstPoint = firstPoint;
            this.secondPoint = secondPoint;
        }
        public VectorPoint(Size sz)
        {
            this.firstPoint = new Point(0, 0);
            this.secondPoint = new Point(sz);
        }

        public VectorPoint(int dw)
        {
            this.firstPoint = new Point(0, 0);
            this.secondPoint = new Point(dw);
        }
        public int X1
        {
            get { return firstPoint.X; }
            set { firstPoint.X = value; }
        }

        public int Y1
        {
            get { return firstPoint.Y; }
            set { firstPoint.Y = value; }
        }
        public int X2
        {
            get { return secondPoint.X; }
            set { secondPoint.X = value; }
        }

        public int Y2
        {
            get { return secondPoint.Y; }
            set { secondPoint.Y = value; }
        }

        public Point FirstPoint
        {
            get { return firstPoint; }
            set { firstPoint = value; }
        }
        public Point SecondPoint
        {
            get { return secondPoint; }
            set { secondPoint = value; }
        }
        public bool IsEmpty => (firstPoint.IsEmpty && secondPoint.IsEmpty);

        public int Compare(VectorPoint x, VectorPoint y)
        {
            int result = x.X1.CompareTo(y.X1);
            if (result == 0) result = x.Y1.CompareTo(y.Y1);
            else if (result == 0) result = x.X2.CompareTo(y.X2);
            else if (result == 0) result = x.Y2.CompareTo(y.Y2);
            return result;
        }

        public int CompareTo(VectorPoint other)
        {
            int result = X1.CompareTo(other.X1);
            if (result == 0) result = Y1.CompareTo(other.Y1);
            else if (result == 0) result = X2.CompareTo(other.X2);
            else if (result == 0) result = Y2.CompareTo(other.Y2);
            return result;
        }

        public override int GetHashCode()
        {
            return firstPoint.GetHashCode() / 2 + secondPoint.GetHashCode() / 2;
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is VectorPoint)
            {
                VectorPoint temp = (VectorPoint)obj;
                result = (firstPoint.Equals(temp.FirstPoint) && secondPoint.Equals(temp.SecondPoint));
            }
            return result;
        }

        public override string ToString()
        {
            return firstPoint.ToString() + " -> " + secondPoint.ToString();
        }
    }
}

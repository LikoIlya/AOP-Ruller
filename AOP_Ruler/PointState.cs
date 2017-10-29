using System;
using System.Drawing;

namespace AOP_Ruler
{
    public class PointState : ICloneable, IComparable<PointState>
    {
        private readonly int _id;            // Номер точки по порядку в конфигурации фигуры. Нумерация начинается с 0.
        private readonly Point _point;       // Точка (X, Y)
        private MsgAction _state;  // Состояние точки

        public PointState(int id, Point point, MsgAction state)
        {
            _id = id;
            _point = point;
            _state = state;
        }
        public int Id => _id;

        public Point Point => _point;

        public MsgAction State
        {
            get { return _state; }
            set { _state = value; }
        }

        public int CompareTo(PointState other)
        {
            int result = _id.CompareTo(other.Id);
            if (result == 0)
            {
                result = _point.X.CompareTo(other.Point.X);
                if (result == 0) result = _point.Y.CompareTo(other.Point.Y);
            }
            return result;
        }

        public override string ToString()
        {
            return _id.ToString() + ", " + _point.ToString() + " - " + _state.ToString();
        }

        public override bool Equals(object obj)
        {
            PointState tempObj = (PointState) (obj);
            return _id.Equals(tempObj.Id) && _point.Equals(tempObj.Point);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode() * 37 + _point.GetHashCode();
        }

        public object Clone()
        {
            return new PointState(_id, _point, _state);
        }
    }
}

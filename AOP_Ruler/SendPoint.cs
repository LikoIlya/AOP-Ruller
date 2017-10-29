using System;
using System.Drawing;

namespace AOP_Ruler
{
    class SendPoint : ICloneable
    {
        #region Поля класса SendPoint
        private readonly int _id;       // Номер точки по порядку точки в конфигурации фигуры
        private readonly Point _point;  // Точка конфигурации
        #endregion
        #region Конструкторы класса SendPoint
        public SendPoint(int id, Point point)
        {
            if (id > 0) _id = id;
            else _id = 0;
            if (point != null) _point = point;
            else _point = new Point();
        }
        #endregion
        #region Свойства класса SendPoint
        public int ID => _id;           // Номер точки по порядку точки в конфигурации фигуры
        public Point Point => _point;   // Точка конфигурации
        #endregion
        #region Методы класса SendPoint

        public override string ToString()
        {
            return _id + ", " + _point;
        }
        public override bool Equals(object obj)
        {
            SendPoint tempObj = (SendPoint)obj;
            return (_point.Equals(tempObj.Point)) && (_id.Equals(tempObj.ID));
        }

        public override int GetHashCode()
        {
            return _point.GetHashCode();
        }

        public object Clone()
        {
            return new SendPoint(_id, new Point(_point.X, _point.Y));
        }

        #endregion

    }
}

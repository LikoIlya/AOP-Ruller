using System.Drawing;

namespace AOP_Ruler
{
    /// <summary>
    /// Класс, описывающий цель агента
    /// </summary>
    public class Purpose
    {
        #region Поля класса Purpose

        private int _importance; // Важность данной цели для агента. Диапазон (0..100)
        private int _worship;    // Соответствие данной цели к идеалам вероисповедания или убеждениям агента. Диапазон (0..100)
        private object _entity;  // Цель, которую хочет достичь агент

        #endregion

        #region Конструкторы класса Purpose
        public Purpose() : this(50) { }
        public Purpose(int importance) : this(importance, 50) { }
        public Purpose(int importance, int worship) : this(importance, worship, new Point(0,0)) { }
        public Purpose(int importance, int worship, object entity)
        {
            Importance = importance;
            Worship = worship;
            Entity = entity;
        }
        #endregion

        #region Свойства класса Purpose
        public int Importance
        {
            get { return _importance; }
            set { _importance = value > 0 ? value : 0; }
        }

        public int Worship
        {
            get { return _worship; }
            set { _worship = value > 0 ? value : 0; }
        }

        public object Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
        #endregion

        #region Методы класса Purpose

        public override bool Equals(object obj)
        {
            bool result = true;
            if (obj is Purpose)
            {
                Purpose tempObj = (Purpose) obj;
                result &= _importance.Equals(tempObj.Importance);
                result &= _worship.Equals(tempObj.Worship);
                if ((_entity is Point) && (tempObj.Entity is Point))
                {
                    Point _point = (Point) _entity;
                    Point point = (Point) tempObj.Entity;
                    result &= _point.Equals(point);
                }
                else
                {
                    result &= _entity.Equals(tempObj.Entity);
                }
            }
            else
            {
                result = base.Equals(obj);
            }
            return result;
        }
        #endregion
    }
}

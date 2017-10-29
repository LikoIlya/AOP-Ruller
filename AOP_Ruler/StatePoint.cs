namespace AOP_Ruler
{
    public class StatePoint
    {
        #region Поля класса Field
        private readonly int _id;   // Номер по порядку точки в конфигурации фигуры
        private MsgAction _state;   // Состояние точки
        #endregion
        #region Конструкторы класса Field
        public StatePoint(int id) : this(id, MsgAction.Approve) { }
        public StatePoint(int id, MsgAction state)
        {
            if (id > 0) _id = id;
            else _id = 0;
            State = state;
        }
        #endregion
        #region Свойства класса Field
        public int ID => _id;
        public MsgAction State
        {
            get { return _state; }
            set { _state = value; }
        }
        #endregion
        #region Методы класса Field

        public override string ToString()
        {
            return _id + ", " + _state;
        }
        public override bool Equals(object obj)
        {
            StatePoint tempObj = (StatePoint) obj;
            return _id.Equals(tempObj.ID);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        #endregion

    }
}

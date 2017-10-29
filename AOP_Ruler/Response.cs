namespace AOP_Ruler
{
    /// <summary>
    /// Класс описывающие скорость реакции на различные сообщения
    /// </summary>
    public class Response
    {
        #region Поля класса Response
        private TypeMessege _typeMessage;
        private int _timeResponse;
        #endregion
        #region Конструкторы класса Response
        public Response() : this(TypeMessege.GetPoint, 0) { }
        public Response(TypeMessege type, int time)
        {
            _typeMessage  = type;
            if (time >= 0) _timeResponse = time;
            else _timeResponse = 0;
        }
        #endregion

        public TypeMessege TypeMessage
        {
            get { return _typeMessage; }
            set { _typeMessage = value; }
        }

        public int TimeResponse
        {
            get { return _timeResponse; }
            set { if (value >= 0) _timeResponse = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;

namespace AOP_Ruler
{
    /// <summary>
    /// Перечислитель, определяющий типы сообщений
    /// </summary>
    public enum TypeMessege
    {
        InitEnvironment,    // Создание окружающей среды закончено, можно агентам начинать внедрятся в среду
        GetPoint,           // Запрос на получение точки координат
        DeletePoint,        // Запрос на удаление точки
        DeleteAllPoint,     // Удалить все точки и линии, созданные агентом
        NewAgent            // Запрос на подсоединение нового агента к данному окружению/среде обитания
    }
    /// <summary>
    /// Тип запрашиваемых действий или ответов на них
    /// </summary>
    public enum MsgAction
    {
        Invoke,     // Запросить разрешение на какое-либо действие в среде (запрос агента)
        Approve,    // Подпвердить запрошенное действие (ответ окружающей среды)
        Deny,       // Отказ в запрошенном действии (ответ окружающей среды)
        Commit,     // Подтверждение агентом ранее запрошенных намерений на какое-либо действие в случае получения Approve от окружающей среды
        Rollback    // Отказ агентом от ранее запрошенных намерений на какое-либо действие в случае получения Approve от окружающей среды
    }
    public class EnvironmentMessage
    {
        #region Поля класса SpaceMessage

        private readonly int _id;           // ID сообщения
        private readonly int _idOwner;      // ID отправителя (владельца) сообщения. Окружающая среда имеет ID = 0;
        private TypeMessege _typeMessege;   // Тип сообщения 
        private MsgAction _action;          // Тип запроса или ответа на определенный тип действий
        private object _entity;             // Сущность, которая передается в сообщении для дальнейшей обработки данного события
                                            // Это может быть точка координат для типа сообщений TypeMessege.GetPoint или null
                                            // если это сообщение TypeMessege.NewAgent

        #endregion

        #region Конструкторы класса EnvironmentMessage
        public EnvironmentMessage() : this(0) { }
        public EnvironmentMessage(int id) : this(id, 0) { }
        public EnvironmentMessage(int id, int owner) : this(id, owner, TypeMessege.InitEnvironment, MsgAction.Commit, null) { }
        public EnvironmentMessage(int id, int owner, TypeMessege typeMessege, MsgAction action, object entity)
        {
            _id = id;
            _idOwner = owner;
            _typeMessege = typeMessege;
            _action = action;
            _entity = entity;
        }

        #endregion

        #region Свойства класса EnvironmentMessage

        public int ID => _id;

        public int Owner => _idOwner;

        public TypeMessege Message
        {
            get { return _typeMessege; }
            set { _typeMessege = value; }
        }

        public MsgAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public object Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
        #endregion

        #region Методы класса EnvironmentMessage

        public override bool Equals(object obj)
        {
            bool result = false;
            EnvironmentMessage tempObj = (EnvironmentMessage)obj;
            if (obj != null)
            {
                if (this._id == tempObj.ID)
                {
                    if (this.Owner == tempObj.Owner)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool AproveForAgent(EnvironmentMessage oldMessage)
        {
            return (Equals(oldMessage) && (this.Action == MsgAction.Approve));
        }
        public override string ToString()
        {
            return _id.ToString() + ", " + 
                   _idOwner.ToString();
        }

        public override int GetHashCode()
        {
            return (_id.ToString() + _idOwner.ToString()).GetHashCode();
        }

        public EnvironmentMessage Clone()
        {
            Object obj = null;
            if (_entity == null) _entity = new SendPoint(0, new Point());
            else if (_entity is Point)
            {
                Point temp = (Point) _entity;
                obj = new Point(temp.X, temp.Y);
            }
            else if (_entity is SendPoint)
            {
                SendPoint temp = (SendPoint) _entity;
                obj = new SendPoint(temp.ID, new Point(temp.Point.X, temp.Point.Y));
            }
            else if (_entity is AgentConfig)
            {
                AgentConfig temp = (AgentConfig) _entity;
                Purpose tempPurpose = new Purpose(temp.Purpose.Importance, temp.Purpose.Worship, 
                    new Point(((Point)(temp.Purpose.Entity)).X,((Point)(temp.Purpose.Entity)).Y));
                SortedList<TypeMessege, int> tempSortedList = new SortedList<TypeMessege, int>();
                List<List<Point>> listConfig = new List<List<Point>>();
                if (temp.ListConfig != null)
                {
                    List<Point> tempConfig;
                    foreach (List<Point> config in temp.ListConfig)
                    {
                        tempConfig = new List<Point>();
                        foreach (Point point in tempConfig)
                        {
                            tempConfig.Add(point);
                        }
                        listConfig.Add(tempConfig);
                    }
                }
                if (temp.ResponceTime != null)
                {
                    foreach (var i in temp.ResponceTime)
                    {
                        tempSortedList.Add(i.Key, i.Value);
                    }
                }
                Point tempPoint = new Point(temp.StartPoint.X, temp.StartPoint.Y);
                obj = new AgentConfig(temp.Name, temp.Color, temp.SenseOfPurpose, 
                                      tempPurpose, temp.Worship, temp.Temper, 
                                      temp.LifeCircle, temp.Attempt, tempSortedList, 
                                      tempPoint, temp.Length, listConfig);
            }
            else if (_entity is EnvironmentConfig)
            {
                EnvironmentConfig temp = (EnvironmentConfig) _entity;
                Point tempPoint = new Point(temp.OffsetXY.X, temp.OffsetXY.Y);
                SortedList<TypeMessege, int> tempSortedList = new SortedList<TypeMessege, int>();
                if (tempSortedList != null)
                {
                    foreach (var i in temp.Response)
                    {
                        tempSortedList.Add(i.Key, i.Value);
                    }
                }
                obj = new EnvironmentConfig(temp.Height, temp.Width, tempPoint, tempSortedList);
            }
            return new EnvironmentMessage(_id, _idOwner, _typeMessege, _action, obj);
        }

        public int CompareTo(EnvironmentMessage obj)
        {
            int result = _id.CompareTo(obj.ID);
            if (result == 0) result = _idOwner.CompareTo(obj.Owner);
            return result;
        }
        #endregion
    }
}

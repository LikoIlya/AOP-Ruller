using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace AOP_Ruler
{
    public class Environment
    {
        #region Поля класса Environment

        private int _height;    // Высота пространства (Height Space)
        private int _width;     // Длина пространства (Width Space)
        private Point _offsetXY;    // Смещение начала координат
        private SortedList<TypeMessege, int> _response;  // Скорость реакции окружающей среды на различные виды событий
        public delegate void DelEnvironmentMessage(EnvironmentMessage msgEnvironment);  // Делегат, необходимый для создания события
        public static event DelEnvironmentMessage EnvironmentMessage;  // События, генерируемые Environment. 
        private SortedList<int, FigureAgent> _space; // Хранилище всех конфигураций всех агентов
        private List<Thread> _environmenThreads;
        public delegate void DelEnvironmentNewPoint(SortedList<int, FigureAgent> changedPoint);  // Делегат, необходимый для создания события
        public static event DelEnvironmentNewPoint ChangedPoints;  // События, генерируемые Environment. 
        private bool _active;
        private bool _running;
        #endregion

        #region Конструкторы класса Environment

        public Environment()
        {
            _space = new SortedList<int, FigureAgent>();
            _environmenThreads = new List<Thread>();
            _offsetXY = new Point(0, 0);
            _active = true;
            _running = false;
            InitResponce();
            Agent.AgentMessage += ReceiveMessage;
        }

        #endregion

        #region Свойства класса Environment

        public int Height
        {
            get { return _height; } 
            set { if (value > 0) _height = value; }
        }

        public int Width
        {
            get { return _width; }
            set { if (value > 0) _width = value; }
        }

        public Point OffsetXY
        {
            get { return _offsetXY; }
            set { if ((value.X > 0) && (value.Y > 0)) _offsetXY = value; }
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                if (value != _active)
                {
                    if (value)
                    {
                        Agent.AgentMessage += ReceiveMessage;
                    }
                    else
                    {
                        Agent.AgentMessage -= ReceiveMessage;
                    }
                    _active = value;
                }
            }
        }


        #endregion

        #region Методы класса Environment

        private void InitResponce()
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                _response = new SortedList<TypeMessege, int>();
                _response.Add(TypeMessege.GetPoint, 100);
                _response.Add(TypeMessege.NewAgent, 100);
                _response.Add(TypeMessege.DeletePoint, 150);
                _response.Add(TypeMessege.DeleteAllPoint, 200);
                _response.Add(TypeMessege.InitEnvironment, 100);
                _running = tempRunning;
            }
        }
        public void InitResponce(int getPoint,int newAgent,int deletePoint,int deleteAllPoint, int initEnvironment)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                _response = new SortedList<TypeMessege, int>();
                _response.Add(TypeMessege.GetPoint, getPoint);
                _response.Add(TypeMessege.NewAgent, newAgent);
                _response.Add(TypeMessege.DeletePoint, deletePoint);
                _response.Add(TypeMessege.DeleteAllPoint, deleteAllPoint);
                _response.Add(TypeMessege.InitEnvironment, initEnvironment);
                _running = tempRunning;
            }
        }

        public SortedList<TypeMessege, int> Respose
        {
            get { return _response; }
            set { _response = value; }
        } 
        private void SendMessage(EnvironmentMessage message)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                lock (this)
                {
                    if (message != null)
                    {
                        if ((message.Message == TypeMessege.DeletePoint) ||
                            (message.Message == TypeMessege.DeleteAllPoint) ||
                            (message.Message == TypeMessege.GetPoint))
                        {
                            if (_space != null)
                            {
                                ChangedPoints?.Invoke(_space);
                            }
                        }
                        EnvironmentMessage?.Invoke(message);
                    }
                }
                _running = tempRunning;
            }
        }

        private void SendMessageEnvironmentMessage(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                EnvironmentMessage message = new EnvironmentMessage();
                if (message is EnvironmentMessage)
                    message = ((EnvironmentMessage) msg).Clone();
                EnvironmentMessage?.Invoke(message);
                _running = tempRunning;
            }
        }

        private void SendMessageChangePoint(object space)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                SortedList<int, FigureAgent> eventMessage = new SortedList<int, FigureAgent>();
                if (space is SortedList<int, FigureAgent>)
                    eventMessage = (SortedList<int, FigureAgent>) space;
                ChangedPoints?.Invoke(eventMessage);
                _running = tempRunning;
            }
        }

        /// <summary>
        /// Обработчик событий, поступивших от агента. Метод, который слушает сообщения Agent. Он должен быть подписан на события AgentMessage.
        /// </summary>
        /// <param name="message"></param>
        public void ReceiveMessage(EnvironmentMessage message)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                //int i;
                switch (message.Message)
                {
                    case TypeMessege.GetPoint:
                        switch (message.Action)
                        {
                            case MsgAction.Invoke: // Запускаем поток реакции на сообщение MsgAction.Invoke
                                //lock (this)
                                //{
                                    //_environmenThreads.Add(new Thread(new ParameterizedThreadStart(AddPoint)));
                                    //i = _environmenThreads.Count - 1;
                                    //_environmenThreads[i].IsBackground = true;
                                    //_environmenThreads[i].Start(message);
                                    AddPoint(message);
                                //}
                                break;
                            case MsgAction.Commit: // Запускаем поток реакции на сообщение MsgAction.Commit
                                //lock (this)
                                //{
                                    //_environmenThreads.Add(new Thread(new ParameterizedThreadStart(CommitPoint)));
                                    //i = _environmenThreads.Count - 1;
                                    //_environmenThreads[i].IsBackground = true;
                                    //_environmenThreads[i].Start(message);
                                    CommitPoint(message);
                                //}
                                break;
                            case MsgAction.Rollback: // Запускаем поток реакции на сообщение MsgAction.Rollback
                                //lock (this)
                                //{
                                    //_environmenThreads.Add(new Thread(new ParameterizedThreadStart(RollBackPoint)));
                                    //i = _environmenThreads.Count - 1;
                                    //_environmenThreads[i].IsBackground = true;
                                    //_environmenThreads[i].Start(message);
                                    RollBackPoint(message);
                                //}
                                break;
                        }
                        break;
                    case TypeMessege.DeletePoint:
                        switch (message.Action)
                        {
                            case MsgAction.Invoke: // Запускаем поток реакции на сообщение MsgAction.Invoke
                                //lock (this)
                                //{
                                    //_environmenThreads.Add(new Thread(new ParameterizedThreadStart(DeletePoint)));
                                    //i = _environmenThreads.Count - 1;
                                    //_environmenThreads[i].IsBackground = true;
                                    //_environmenThreads[i].Start(message);
                                    DeletePoint(message);
                                //}
                                break;
                            case MsgAction.Commit: // Запускаем поток реакции на сообщение MsgAction.Commit
                                lock (this)
                                {
                                    //_environmenThreads.Add(new Thread(new ParameterizedThreadStart(CommitDeletePoint)));
                                    //i = _environmenThreads.Count - 1;
                                    //_environmenThreads[i].IsBackground = true;
                                    //_environmenThreads[i].Start(message);
                                    CommitDeletePoint(message);
                                }
                                break;
                            case MsgAction.Rollback: // Запускаем поток реакции на сообщение MsgAction.Rollback
                                //lock (this)
                                //{
                                    //_environmenThreads.Add(new Thread(new ParameterizedThreadStart(RollBackDeletePoint)));
                                    //i = _environmenThreads.Count - 1;
                                    //_environmenThreads[i].IsBackground = true;
                                    //_environmenThreads[i].Start(message);
                                    RollBackDeletePoint(message);
                                //}
                                break;
                        }
                        break;
                    case TypeMessege.DeleteAllPoint: // Запускаем поток реакции на сообщение MsgAction.DeleteAllPoint
                        //lock (this)
                        //{
                            //_environmenThreads.Add(new Thread(new ParameterizedThreadStart(DeleteAllPoint)));
                            //i = _environmenThreads.Count - 1;
                            //_environmenThreads[i].IsBackground = true;
                            //_environmenThreads[i].Start(message);
                            DeleteAllPoint(message);
                        //}
                        break;
                    case TypeMessege.NewAgent: // Запускаем поток реакции на сообщение MsgAction.NewAgent
                        //lock (this)
                        //{
                            //_environmenThreads.Add(new Thread(new ParameterizedThreadStart(AddAgent)));
                            //i = _environmenThreads.Count - 1;
                            //_environmenThreads[i].IsBackground = true;
                            //_environmenThreads[i].Start(message);
                            AddAgent(message);
                        //}
                        break;
                }
                _running = tempRunning;
            }
        }

        /// <summary>
        /// Метод поиска точек в хранилище конфигураций
        /// </summary>
        /// <returns></returns>
        private bool SearchAgent(int id)
        {
            bool result = false;
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_space != null)
                {
                    lock (this)
                    {
                        result = _space.ContainsKey(id);
                    }
                }
                _running = tempRunning;
            }
            return result;
        }
        private bool SearchPoint(Point point)
        {
            bool result = false;
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_space != null)
                {
                    lock (this)
                    {
                        //result = _space.Values.Any(pls => pls.Points.Any(ps => ps.Equals(point)));
                        foreach (FigureAgent listPoints in from listPoints in _space.Values
                            from pointsConfig in
                                listPoints.Points.Where(pointsConfig => point.Equals(pointsConfig.Point))
                            select listPoints)
                        {
                            result = true;
                        }
                    }
                }
                _running = tempRunning;
            }
            return result;
        }

        private int IndexPoint(EnvironmentMessage msg)
        {
            int i = -1;
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg.Entity is SendPoint))
                {
                    if (_space != null)
                    {
                        lock (this)
                        {
                            i = _space[msg.Owner].Points.FindIndex(x => x.Point.Equals(((SendPoint) (msg.Entity)).Point));
                        }
                    }
                }
                _running = tempRunning;
            }
            return i;
        }
        private bool SearchPoint(int agent, int id, Point point)
        {
            bool result = false;
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_space != null)
                {
                    lock (this)
                    {
                        if ((_space.Count > 0) && _space.ContainsKey(agent))
                        {
                            int indexPoint = _space[agent].Points.FindIndex(state => state.Point.Equals(point));
                            result = ((indexPoint >= 0) &&
                                      (_space[agent].Points[indexPoint].State == MsgAction.Invoke) &&
                                      (_space[agent].Points[indexPoint].Id == id));
                        }
                    }
                }
                _running = tempRunning;
            }
            return result;
        }

        private void AddAgent(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg is EnvironmentMessage))
                {
                    EnvironmentMessage message = (EnvironmentMessage) (msg);
                    Thread.Sleep(_response[TypeMessege.NewAgent]);
                    if (_space != null)
                    {
                        lock (this)
                        {
                            if (!_space.ContainsKey(message.Owner))
                            {
                                try
                                {
                                    _space.Add(message.Owner,
                                        new FigureAgent(((AgentConfig) (message.Entity)).Color, new List<PointConfig>()));
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    EnvironmentConfig environmentConfig = new EnvironmentConfig(_height, _width, _offsetXY, _response);
                    message.Entity = environmentConfig;
                    message.Action = MsgAction.Approve;
                    SendMessage(message);
                }
                _running = tempRunning;
            }
        }

        private void AddPoint(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg is EnvironmentMessage))
                {
                    EnvironmentMessage message = (EnvironmentMessage) (msg);
                    Thread.Sleep(_response[TypeMessege.GetPoint]);
                    Color color = Color.Red;
                    if (message.Entity is AgentConfig)
                    {
                        color = ((AgentConfig) (message.Entity)).Color;
                    }
                    if (!SearchPoint((((SendPoint) (message.Entity)).Point)))
                    {
                        if (!SearchAgent(message.Owner))
                        {
                            if (_space != null)
                            {
                                lock (this)
                                {
                                    _space.Add(message.Owner, new FigureAgent(color, new List<PointConfig>()));
                                }
                            }
                        }
                        if (_space != null)
                        {
                            lock (this)
                            {
                                if ((_space.Count > 0) && (_space[message.Owner].Points.Count > 0))
                                {
                                    Point lastPoint = _space[message.Owner].Points.Last().Point;
                                    Point aditionPoint = ((SendPoint) (message.Entity)).Point;
                                    int length = Math.Abs(lastPoint.X - aditionPoint.X) +
                                                 Math.Abs(lastPoint.Y - aditionPoint.Y);
                                    if (length > 1)
                                    {
                                        _space[message.Owner].Points.RemoveAll(state =>
                                            state.Point.Equals(lastPoint));
                                    }
                                }
                                _space[message.Owner].Points.Add(new PointConfig(((SendPoint) (message.Entity)).ID,
                                    new Point(((SendPoint) (message.Entity)).Point.X,
                                        ((SendPoint) (message.Entity)).Point.Y), message.Action));
                            }
                        }
                        message.Action = MsgAction.Approve;
                    }
                    else
                    {
                        message.Action = MsgAction.Deny;
                    }
                    SendMessage(message);
                }
                _running = tempRunning;
            }
        }
        private void DeletePoint(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg is EnvironmentMessage))
                {
                    EnvironmentMessage message = (EnvironmentMessage) (msg);
                    Thread.Sleep(_response[TypeMessege.DeletePoint]);
                    if (_space != null)
                    {
                        lock (this)
                        {
                            if ((_space.ContainsKey(message.Owner)) && (_space[message.Owner].Points.RemoveAll(
                                state => state.Point.Equals(((SendPoint) (message.Entity)).Point)) > 0))
                                message.Action = MsgAction.Approve;
                            else message.Action = MsgAction.Deny;
                        }
                    }
                    SendMessage(message);
                }
                _running = tempRunning;
            }
        }

        private void CommitPoint(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg is EnvironmentMessage))
                {
                    EnvironmentMessage message = (EnvironmentMessage) (msg);
                    Thread.Sleep(_response[TypeMessege.GetPoint]);
                    if (message.Entity is SendPoint)
                    {
                        if (SearchPoint(message.Owner, ((SendPoint) (message.Entity)).ID,
                            ((SendPoint) (message.Entity)).Point))
                        {
                            int indexPoint = IndexPoint(message);
                            if (_space != null)
                            {
                                lock (this)
                                {
                                    _space[message.Owner].Points[indexPoint].State = MsgAction.Commit;
                                }
                            }
                            message.Action = MsgAction.Approve;
                        }
                        else
                        {
                            message.Action = MsgAction.Deny;
                        }
                        SendMessage(message);
                    }
                }
                _running = tempRunning;
            }
        }

        private void CommitDeletePoint(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg is EnvironmentMessage))
                {
                    EnvironmentMessage message = (EnvironmentMessage) (msg);
                    Thread.Sleep(_response[TypeMessege.DeletePoint]);
                    message.Action = MsgAction.Approve;
                    SendMessage(message);
                }
                _running = tempRunning;
            }
        }

        private void RollBackPoint(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg is EnvironmentMessage))
                {
                    EnvironmentMessage message = (EnvironmentMessage) (msg);
                    Thread.Sleep(_response[TypeMessege.GetPoint]);
                    if (_space != null)
                    {
                        lock (this)
                        {
                            if (
                                _space[message.Owner].Points.RemoveAll(
                                    state => state.Point.Equals(((Point) (message.Entity)))) > 0)
                                message.Action = MsgAction.Approve;
                            else message.Action = MsgAction.Deny;
                        }
                    }
                    SendMessage(message);
                }
                _running = tempRunning;
            }
        }

        private void RollBackDeletePoint(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg is EnvironmentMessage))
                {
                    EnvironmentMessage message = (EnvironmentMessage) (msg);
                    Thread.Sleep(_response[TypeMessege.GetPoint]);
                    if (_space != null)
                    {
                        lock (this)
                        {
                            _space[message.Owner].Points.RemoveAll(
                                state => state.Point.Equals(((Point) (message.Entity))));
                        }
                    }
                    message.Action = MsgAction.Approve;
                    SendMessage(message);
                }
                _running = tempRunning;
            }
        }

        private void DeleteAllPoint(object msg)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if ((msg != null) && (msg is EnvironmentMessage))
                {
                    EnvironmentMessage message = (EnvironmentMessage) (msg);
                    Thread.Sleep(_response[TypeMessege.DeleteAllPoint]);
                    if (_space != null)
                    {
                        lock (this)
                        {
                            if (_space.Remove(message.Owner))
                                message.Action = MsgAction.Approve;
                            else message.Action = MsgAction.Deny;
                        }
                        SendMessage(message);
                    }
                }
                _running = tempRunning;
            }
        }

        public SortedList<int, FigureAgent> Show()
        {
            return _space;
        }

        public void Clear()
        {
            _height = 0;
            _width = 0;
            _offsetXY = new Point();
            _response.Clear();
            _response = null;
            _space?.Clear();
            _space = null;
            for (int i = 0; i < _environmenThreads.Count; i++)
            {
                _environmenThreads[i].Abort();
                _environmenThreads[i] = null;
            }
            _environmenThreads.Clear();
            _environmenThreads = null;
        }
        public void StopEnvironmentAndClearAll()
        {
            Environment.EnvironmentMessage -= ReceiveMessage;
            _active = false;
            //while (_running) ;
            Clear();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace AOP_Ruler
{
    public enum Temper
    {
        Флегматик,
        Холерик,
        Сангвінік,
        Меланхолік
    }
    /// <summary>
    /// Класс, описывающий основное поведение агента
    /// </summary>
    public class Agent
    {
        #region Поля класса Agent
        #region Константы класса Agent
        #region Константы влияющие на время выполнения действий агентом
        /// <summary>
        /// Коеффициент влияния целеустремленности на время выполнения одного шага действия агента.
        /// </summary>
        private static float _ktSenseOfPurpose = 0.002f;
        /// <summary>
        /// Коеффициент влияния важности данной цели для агента на время выполнения одного шага действия агента.
        /// </summary>
        private static float _ktPurposeImportance = 0.0015f;
        /// <summary>
        /// Коеффициент влияния соответствия данной цели к идеалам вероисповедания или убеждениям агента на время выполнения одного шага действия агента.
        /// </summary>
        private static float _ktPurposeWorship = 0.002f;
        /// <summary>
        /// Коеффициент влияния вероисповедания на время выполнения одного шага действия агента.
        /// </summary>
        private static float _ktWorship = 0.0025f;
        /// <summary>
        /// Коеффициент влияния жизненного цикла на время выполнения одного шага действия агента.
        /// </summary>
        private static float _ktLifeCircle = 0.0001f;
        #endregion
        #region Константы влияющие на количество попыток, сделанные агентом для достижения цели
        /// <summary>
        /// Коеффициент влияния целеустремленности на количество попыток к достижению цели агентом.
        /// </summary>
        private static float _kaSenseOfPurpose = 0.01f;
        /// <summary>
        /// Коеффициент влияния важности данной цели для агента на количество попыток к достижению цели агентом.
        /// </summary>
        private static float _kaPurposeImportance = 0.015f;
        /// <summary>
        /// Коеффициент влияния соответствия данной цели к идеалам вероисповедания или убеждениям агента на количество попыток к достижению цели агентом.
        /// </summary>
        private static float _kaPurposeWorship = 0.02f;
        /// <summary>
        /// Коеффициент влияния вероисповедания на количество попыток к достижению цели агентом.
        /// </summary>
        private static float _kaWorship = 0.025f;
        /// <summary>
        /// Коеффициент влияния жизненного цикла на количество попыток к достижению цели агентом.
        /// </summary>
        private static float _kaLifeCircle = 0.001f;
        #endregion
        #endregion

        private string _name;           // Имя агента
        private Color _color;           // Цвет, которым будет отображатся агент
        private int _senseOfPurpose;    // Целеустремленность. Диапазон (0..100). Измеряется в процентах
        private Purpose _purpose;       // Цель, см. класс Purpose
        private int _worship;           // Вероисповедание. Диапазон (-100..100). -100 - агент антагонист любым вероучениям. 100 - полностью верующий агент.
        private Temper _temper;         // Характер. Значение по умолчанию Temper.Сангвінік
        private int _lifeCircle;     // Время жизни агента. Если _lifeCircle = 0 - бессмертен. Время задается в милисекундах. Значение по умолчанию 0 - бессмертен.
        private static int _averageTimePause = 1000; // Начальное время задержки каждого следующего шага для всех агентов. 
        private int _timePause;  // Расчетное время задержки каждого шага для каждого агента в отдельности. Зависит от множества предыдущих параметров.
        private SortedList<TypeMessege, int> responceTime; // Время выполнения каждого действия в отдельности
        private static int _startСountAttempt = 10;      // Начальное количество попыток по достижению цели.
        private int _countAttempt;      // Количество попыток по достижению цели.
        private int _id;                // ID агента.
        private int _timeToStart;       // Время до рождения агента  
        private bool _endOfLife;        // Флаг, который свидетельствует необходимо ли начинать процессы по сворачиванию всех процессов жизнедеятельности агента
        private Engine _engine;         // Объект, который выполняет действий по достижению цели
        private DateTime _startTime;     // Время рождения агента
        private Point _startPoint;      // Начальная точки, точка в которой рождается агент
        public delegate void DelAgentMessage(EnvironmentMessage message); // Делегат для создания событий агента
        public static event DelAgentMessage AgentMessage;  // Событие агента, которое посылает сообщения агента
        private List<PointState> _space; // Хранилище всех конфигураций агента
        private int _idMessage;         // id сообщения
        private EnvironmentMessage _oldMessage;     // Старое сообщение
        private EnvironmentMessage _newMessage;     // Новое сообщение
        private Environment _env;       // Ссылка на объект среды окружения 
        private bool _registred;        // Переменная, которая хранит в себе состояние агента был ли он зарегистрирован в Environment
        private bool _denyEnvironment;  // Переменная, которая хранит в себе значение был ли агент принят Environment
        private bool _active;           // Показывает активный ли агент.
        private int _length;            // Длина агента (линейки)
        private List<List<Point>> listConfig = new List<List<Point>>();
        private Queue<EnvironmentMessage> queue = new Queue<EnvironmentMessage>();
        private bool _running = false;
        //public delegate void ReceiveMsg(EnvironmentMessage b);
        //public static event ReceiveMsg AgentMessage;


        #endregion

        #region Конструкторы класса Agent
        public Agent() : this("No Name") { }
        public Agent(string name) : this(name, Color.Red) { }
        public Agent(string name, Color color) : this(name, color, new Point(0, 0)) { }
        public Agent(string name, Color color, Point startPoint) : this(name, color, startPoint, 50) { }
        public Agent(string name, Color color, Point startPoint, int senseOfPurpose) : this(name, color, startPoint, senseOfPurpose, new Purpose()) { }
        public Agent(string name, Color color, Point startPoint, int senseOfPurpose, Purpose purpose) : this(name, color, startPoint, senseOfPurpose, purpose, 20) { }
        public Agent(string name, Color color, Point startPoint, int senseOfPurpose, Purpose purpose, int worship) : this(name, color, startPoint, senseOfPurpose, purpose, worship, Temper.Сангвінік) { }
        public Agent(string name, Color color, Point startPoint, int senseOfPurpose, Purpose purpose, int worship, Temper temper) : this(name, color, startPoint, senseOfPurpose, purpose, worship, temper, 0) { }
        public Agent(string name, Color color, Point startPoint, int senseOfPurpose, Purpose purpose, int worship, Temper temper, int lifeCircle) : this(name, color, startPoint, senseOfPurpose, purpose, worship, temper, 0, new RealEngine()) { }
        public Agent(string name, Color color, Point startPoint, int senseOfPurpose, Purpose purpose, int worship, Temper temper, int lifeCircle, Engine engine) : this(name, color, startPoint, senseOfPurpose, purpose, worship, temper, 0, new RealEngine(), 0) { }
        public Agent(string name, Color color, Point startPoint, int senseOfPurpose, Purpose purpose, int worship, Temper temper, int lifeCircle, Engine engine, int timeToStart)
        {
            _active = true;
            _name = name;
            _color = color;
            _startPoint = startPoint;
            InitTimeStep();
            _senseOfPurpose = senseOfPurpose;
            _purpose = purpose;
            _worship = worship;
            _temper = temper;
            LifeCircle = lifeCircle;
            _timeToStart = timeToStart;
            Engine = engine;
            EndOfLife = false;
            _startTime = DateTime.Now;
            _idMessage = 0;
            _oldMessage = null;
            _newMessage = null;
            _registred = false;
            _space= new List<PointState>();
            Length = 10;
            CalculateResponceTime();
            InitCountAttempt();
            Environment.EnvironmentMessage += ReceiveMessage;
        }
        #endregion

        #region Свойства класса Agent

        public static float KTrySenseOfPurpose
        {
            get { return _kaSenseOfPurpose; }
            set { _kaSenseOfPurpose = value; }
        }
        public static float KTryPurposeImportance
        {
            get { return _kaPurposeImportance; }
            set { _kaPurposeImportance = value; }
        }
        public static float KTryPurposeWorship
        {
            get { return _kaPurposeWorship; }
            set { _kaPurposeWorship = value; }
        }
        public static float KTryWorship
        {
            get { return _kaWorship; }
            set { _kaWorship = value; }
        }
        public static float KTryLifeCircle
        {
            get { return _kaLifeCircle; }
            set { _kaLifeCircle = value; }
        }

        public static float KStepSenseOfPurpose
        {
            get { return _ktSenseOfPurpose; }
            set { _ktSenseOfPurpose = value; }
        }
        public static float KStepPurposeImportance
        {
            get { return _ktPurposeImportance; }
            set { _ktPurposeImportance = value; }
        }
        public static float KStepPurposeWorship
        {
            get { return _ktPurposeWorship; }
            set { _ktPurposeWorship = value; }
        }
        public static float KStepWorship
        {
            get { return _ktWorship; }
            set { _ktWorship = value; }
        }
        public static float KStepLifeCircle
        {
            get { return _ktLifeCircle; }
            set { _ktLifeCircle = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public int SenseOfPurpose
        {
            get { return _senseOfPurpose; }
            set
            {
                if (_senseOfPurpose != value)
                {
                    _senseOfPurpose = value > 0 ? value : 0;
                    CalculateResponceTime();
                    InitCountAttempt();
                }
            }
        }
        public Purpose Purpose
        {
            get { return _purpose; }
            set
            {
                if (!_purpose.Equals(value))
                {
                    _purpose = value;
                    CalculateResponceTime();
                    InitCountAttempt();
                }
            }
        }
        public int PurposeWorship
        {
            get { return _purpose.Worship; }
            set
            {
                if (!_purpose.Worship.Equals(value))
                {
                    _purpose.Worship = value;
                    CalculateResponceTime();
                    InitCountAttempt();
                }
            }
        }
        public int PurposeImportance
        {
            get { return _purpose.Importance; }
            set
            {
                if (!_purpose.Importance.Equals(value))
                {
                    _purpose.Importance = value;
                    CalculateResponceTime();
                    InitCountAttempt();
                }
            }
        }
        public Temper Temper
        {
            get { return _temper; }
            set
            {
                if (_temper != value)
                {
                    _temper = value;
                    CalculateResponceTime();
                    InitCountAttempt();
                }
            }
        }
        public int Worship
        {
            get { return _worship; }
            set
            {
                if (_worship != value)
                {
                    if (value > 100) _worship = 100;
                    else if (value < -100) _worship = -100;
                    else _worship = value;
                    CalculateResponceTime();
                    InitCountAttempt();
                }
            }
        }
        public int LifeCircle
        {
            get { return _lifeCircle; }
            set
            {
                if (_lifeCircle != value)
                {
                    _lifeCircle = (value < 0) ? 0 : value;
                    CalculateResponceTime();
                    InitCountAttempt();
                }
            }
        }

        public int TimeToStart
        {
            get { return _timeToStart; }
            set { _timeToStart = value; }
        }
        public bool EndOfLife
        {
            get { return _endOfLife; }
            set { _endOfLife = value; }
        }
        public Engine Engine
        {
            get { return _engine; }
            set { _engine = value; }
        }
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public Point StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        public List<List<Point>> ListConfig
        {
            get { return listConfig; }
            set { listConfig = value; }
        }
        public static int StartCountAttempt
        {
            get { return _startСountAttempt; }
            set { _startСountAttempt = value; }
        }
        public int CountAttempt
        {
            get { return _countAttempt; }
            //set { _countAttempt = value; }
        }
        public SortedList<TypeMessege, int> ResponceTime => responceTime;

        public bool Active
        {
            get { return _active; }
            set
            {
                if (value != _active)
                {
                    if (value)
                    {
                        Environment.EnvironmentMessage += ReceiveMessage;
                    }
                    else
                    {
                        Environment.EnvironmentMessage -= ReceiveMessage;
                    }
                    _active = value;
                    _denyEnvironment = value;
                }
            }
        }

        public bool Running => _running;

        #endregion

        #region Методы класса Agent
        #region Методы определяющие влияние различных факторов на поведение агента
        private int FactorTimerTemper(int timeStep)
        {
            float i;
            switch (_temper)
            {
                case Temper.Холерик:
                    i = 2f;
                    break;
                case Temper.Сангвінік:
                    i = 1f;
                    break;
                case Temper.Флегматик:
                    i = 0.7f;
                    break;
                case Temper.Меланхолік:
                    i = 0.35f;
                    break;
                default:
                    i = 1f;
                    break;
            }
            return (int) (timeStep/i);
        }

        public static int AverageTimePause
        {
            get { return _averageTimePause; }
            set
            {
                if (_averageTimePause != value)
                {
                    if (value > 0) _averageTimePause = value;
                    else _averageTimePause = 0;
                }
            }
        }

        public int TimePause => _timePause;
        public int ResponseTimeInitEnvironment => responceTime[TypeMessege.InitEnvironment];
        public int ResponseTimeNewAgent => responceTime[TypeMessege.NewAgent];
        public int ResponseTimeGetPoint => responceTime[TypeMessege.GetPoint];
        public int ResponseTimeDeletePoint => responceTime[TypeMessege.DeletePoint];
        public int ResponseTimeDeleteAllPoint => responceTime[TypeMessege.DeleteAllPoint];
        private int FactorTimeSenseOfPurpose(int timeStep)
        {
            return (int)(timeStep + timeStep * _senseOfPurpose * _ktSenseOfPurpose);
        }
        private int FactorTimePurpose(int timeStep)
        {
            timeStep = (int)(timeStep / 2 + timeStep * _purpose.Importance * _ktPurposeImportance);
            timeStep = (int)(timeStep / 2 + timeStep * _purpose.Worship * _ktPurposeWorship);
            return timeStep;
        }
        private int FactorTimeWorship(int timeStep)
        {
            return (int)(timeStep / 2 + timeStep * _worship * _ktWorship);
        }
        private int FactorTimeLifeCircle(int timeStep)
        {
            return (_lifeCircle == 0) ? (timeStep * 2) : (int)(timeStep / 2 + timeStep * _lifeCircle * _ktLifeCircle);
        }

        private void CalculateResponceTime()
        {
            _timePause = _averageTimePause;
            _timePause = FactorTimeWorship(_timePause);         // Определяем влияние веры
            _timePause = FactorTimerTemper(_timePause);         // Определяем влияние характера
            _timePause = FactorTimeSenseOfPurpose(_timePause);  // Определяем влияние целеустремленности
            _timePause = FactorTimePurpose(_timePause);         // Определяем влияние цели и таких ее характеристик как соответствие поставленной цели с вероисповеданием или важность самой цели.
            _timePause = FactorTimeLifeCircle(_timePause);      // Определяем влияние длительности жизненного цикла на скорость выполнения всех операций агентом
            responceTime[TypeMessege.InitEnvironment] = (int)(_timePause * 0.1f);
            responceTime[TypeMessege.NewAgent]        = (int)(_timePause * 0.1f);
            responceTime[TypeMessege.GetPoint]        = (int)(_timePause * 1.1f);
            responceTime[TypeMessege.DeletePoint]     = (int)(_timePause * 0.5f);
            responceTime[TypeMessege.DeleteAllPoint]  = (int)(_timePause * 1.8f);
        }
        private void InitTimeStep()
        {
            responceTime = new SortedList<TypeMessege, int>();
            responceTime.Add(TypeMessege.InitEnvironment, 100);
            responceTime.Add(TypeMessege.NewAgent,        100);
            responceTime.Add(TypeMessege.GetPoint,        100);
            responceTime.Add(TypeMessege.DeletePoint,     100);
            responceTime.Add(TypeMessege.DeleteAllPoint,  100);
        }
        private float FactorAttemptTemper(float attempt)
        {
            float i;
            switch (_temper)
            {
                case Temper.Холерик:
                    i = 0.5f;
                    break;
                case Temper.Сангвінік:
                    i = 1.2f;
                    break;
                case Temper.Флегматик:
                    i = 2f;
                    break;
                case Temper.Меланхолік:
                    i = 0.85f;
                    break;
                default:
                    i = 1f;
                    break;
            }
            return attempt * i;
        }
        private float FactorAttemptSenseOfPurpose(float attempt)
        {
            return attempt * 0.8f + attempt * _senseOfPurpose * _ktSenseOfPurpose;
        }
        private float FactorAttemptPurpose(float attempt)
        {
            attempt = attempt * 0.8f + attempt * _purpose.Importance * _ktPurposeImportance;
            attempt = attempt * 0.8f + attempt * _purpose.Worship * _ktPurposeWorship;
            return attempt;
        }
        private float FactorAttemptWorship(float attempt)
        {
            return attempt * 0.8f + attempt * _worship * _ktWorship;
        }
        private float FactorAttemptLifeCircle(float attempt)
        {
            return (int)(attempt * 0.8f + attempt * _lifeCircle * _ktLifeCircle);
        }
        private void InitCountAttempt()
        {
            if (_active)
            {
                //_countAttempt = 3;
                float tempAtemp = _startСountAttempt;
                tempAtemp = FactorAttemptWorship(tempAtemp);        // Определяем влияние веры
                tempAtemp = FactorAttemptTemper(tempAtemp);         // Определяем влияние характера
                tempAtemp = FactorAttemptSenseOfPurpose(tempAtemp); // Определяем влияние целеустремленности
                tempAtemp = FactorAttemptPurpose(tempAtemp);        // Определяем влияние цели и таких ее 
                                                                    // характеристик как соответствие поставленной 
                                                                    // цели с вероисповеданием или важность самой цели.
                _countAttempt =                                     // Определяем влияние длительности жизненного цикла
                    (int) Math.Round(                               //  на скорость выполнения всех операций агентом.
                        FactorAttemptLifeCircle(tempAtemp));
                if (_countAttempt < 1) _countAttempt = 1;
            }

        }
        #endregion
        #region Методы организующие логику работы класса Agent
        private void SendMessage(EnvironmentMessage message)
        {
            // Если на событие AgentMessage кто-нибудь подписан - создаем событие, т.е. посылаем сообщение
            // для нашего Environment-а
            if (_active && (message != null) && (AgentMessage != null))
            {
                AgentMessage(message.Clone());
            }
        }

        public void ReceiveMessage(EnvironmentMessage message)
        {
            if (_active && message.Equals(_newMessage))
            {
                _oldMessage = _newMessage;
                _newMessage = message;
            }
        }
        public void Start()
        {
            if (_active)
            {
                _running = true;
                // Вариант запуска отдельного потока
                //Thread mainThread = new Thread(new ParameterizedThreadStart(ThreadStart));
                //mainThread.IsBackground = true;
                //mainThread.Start(mainThread.ManagedThreadId);
                // Вариант запуска без потока
                ThreadStart(Thread.CurrentThread.ManagedThreadId);
            }
        }

        public void ThreadStart(object id)
        {
            if (!(id is int)) _id = Thread.CurrentThread.ManagedThreadId;
            else _id = (int) id;
            Thread.Sleep(_timeToStart);
            _denyEnvironment = true;
            _startTime = DateTime.Now;
            DateTime endDateTime = DateTime.Now.AddMilliseconds(_lifeCircle);
            InitCircleMessage();
            while (((_lifeCircle == 0) || ((_lifeCircle != 0) && (endDateTime > DateTime.Now))) && _denyEnvironment)
            {
                if (_active)
                {
                    if (StartDialog())
                    {
                        // Запрос у Environment каких либо действий
                        if (_registred) InvokePoint(); // Запрос новой точки
                        else RegistredAgent(); // Запрос регистрации агента
                    }
                    else if (FirstResponce())
                    {
                        switch (_newMessage.Message)
                        {
                            case TypeMessege.GetPoint:
                                NextPointDialog();
                                break;
                            case TypeMessege.NewAgent:
                                NextAgentDialog();
                                break;
                            case TypeMessege.DeletePoint:
                                NextDeletePointDialog();
                                break;
                            default:
                                InitCircleMessage();
                                break;
                        }
                    }
                    else if (SecondResponce())
                    {
                        switch (_newMessage.Message)
                        {
                            case TypeMessege.GetPoint:
                                EndPointDialog();
                                break;
                            case TypeMessege.DeletePoint:
                                EndPointDeleteDialog();
                                break;
                            case TypeMessege.NewAgent:
                                EndAgentDialog();
                                break;
                        }
                    }
                }
                else
                {
                    _denyEnvironment = false;
                }
                //else InitCircleMessage();
            }
            // Процессы окончания деятельности Агента
            if (_active) EndOfLifeAgent(_id);
            _running = false;
        }

        private bool StartDialog()
        {
            if (_active)
            {
                if (_newMessage == null)
                {
                    _oldMessage = null;
                    return true;
                }
            }
            return false;
        }

        private bool FirstResponce()
        {
            if (_active)
            {
                if (_newMessage == null)
                {
                    _oldMessage = null;
                }
                if ((_oldMessage != null) && (_newMessage != null))
                    return ((_oldMessage.Action == MsgAction.Invoke) &&
                            ((_newMessage.Action == MsgAction.Approve) ||
                             (_newMessage.Action == MsgAction.Deny)));
            }
            return false;
        }

        private bool SecondResponce()
        {
            if (_active && (_oldMessage != null) && (_newMessage != null))
                return (((_oldMessage.Action == MsgAction.Commit) || 
                         (_oldMessage.Action == MsgAction.Rollback)) &&
                        ((_newMessage.Action == MsgAction.Approve) ||
                         (_newMessage.Action == MsgAction.Deny)));
            return false;
        }
        private void InitCircleMessage()
        {
            if (_active)
            {
                _oldMessage = null;
                _newMessage = null;
                _idMessage++;
            }
        }
        private void RegistredAgent()
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.NewAgent]);
                _registred = true;
                _newMessage = new EnvironmentMessage(_idMessage, _id, TypeMessege.NewAgent,
                    MsgAction.Invoke,
                    new AgentConfig(_name, _color, _senseOfPurpose, _purpose, _worship,
                        _temper, _lifeCircle, _countAttempt,
                        null, _startPoint, _length, listConfig));
                _engine.NextStep(ref _newMessage);
                SendMessage(_newMessage);
            }
        }

        private void InvokePoint()
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.GetPoint]);
                _newMessage = new EnvironmentMessage(_idMessage, _id,
                    TypeMessege.GetPoint,
                    MsgAction.Invoke, null);
                _engine.NextStep(ref _newMessage);
                SendMessage(_newMessage);
            }
        }

        private void NextAgentDialog()
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.NewAgent]);
                switch (_newMessage.Action)
                {
                    case MsgAction.Approve:
                        _denyEnvironment = true;
                        _engine.NextStep(ref _newMessage);
                        if ((_newMessage != null) &&
                            !((_newMessage.Action != MsgAction.Commit) &&
                              (_newMessage.Action != MsgAction.Rollback)))
                        {
                            _newMessage.Action = MsgAction.Commit;
                        }
                        SendMessage(_newMessage);
                        break;
                    case MsgAction.Deny:
                        _engine.NextStep(ref _newMessage);
                        _denyEnvironment = false;
                        break;
                }
            }
        }

        private void NextPointDialog()
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.GetPoint]);
                switch (_newMessage.Action)
                {
                    case MsgAction.Approve:
                        int index = _space.Count;
                        _space.Add(new PointState(index, ((SendPoint) (_newMessage.Entity)).Point, MsgAction.Invoke));
                        _engine.NextStep(ref _newMessage);
                        if ((_newMessage != null) &&
                            !((_newMessage.Action != MsgAction.Commit) &&
                              (_newMessage.Action != MsgAction.Rollback)))
                        {
                            _newMessage.Action = MsgAction.Commit;
                        }
                        SendMessage(_newMessage);
                        break;
                    case MsgAction.Deny:
                        _engine.NextStep(ref _newMessage);
                        InitCircleMessage();
                        break;
                }
            }
        }
        private void NextDeletePointDialog()
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.DeletePoint]);
                switch (_newMessage.Action)
                {
                    case MsgAction.Approve:
                        _space.RemoveAll(state => state.Point.Equals(((SendPoint) (_newMessage.Entity)).Point));
                        _engine.NextStep(ref _newMessage);
                        if ((_newMessage != null) &&
                            !((_newMessage.Action != MsgAction.Commit) &&
                              (_newMessage.Action != MsgAction.Rollback)))
                        {
                            _newMessage.Action = MsgAction.Commit;
                        }
                        SendMessage(_newMessage);
                        break;
                    case MsgAction.Deny:
                        _engine.NextStep(ref _newMessage);
                        InitCircleMessage();
                        break;
                }
            }
        }

        private void EndAgentDialog()
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.NewAgent]);
                switch (_oldMessage.Action)
                {
                    case MsgAction.Commit:
                        switch (_newMessage.Action)
                        {
                            case MsgAction.Approve:
                                _denyEnvironment = true;
                                break;
                            case MsgAction.Deny:
                                _denyEnvironment = false;
                                break;
                        }
                        break;
                    case MsgAction.Rollback:
                        switch (_newMessage.Action)
                        {
                            case MsgAction.Approve:
                                _denyEnvironment = false;
                                break;
                            case MsgAction.Deny:
                                _denyEnvironment = true;
                                break;
                        }
                        break;
                }
                _engine.NextStep(ref _newMessage);
                InitCircleMessage();
            }
        }
        private void EndPointDialog()
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.GetPoint]);
                if (_oldMessage.Action == MsgAction.Commit)
                {
                    if (_newMessage.Action == MsgAction.Approve)
                        _space[IndexPoint(_newMessage)].State = MsgAction.Commit;
                    else if (_newMessage.Action == MsgAction.Deny)
                        _space.RemoveAll(state => state.Point.Equals(((SendPoint) (_newMessage.Entity)).Point));
                }
                else if (_oldMessage.Action == MsgAction.Rollback)
                {
                    if (_newMessage.Action == MsgAction.Approve)
                        _space.RemoveAll(state => state.Point.Equals(((SendPoint) (_newMessage.Entity)).Point));
                    else if (_newMessage.Action == MsgAction.Deny)
                        _space.RemoveAll(state => state.Point.Equals(((SendPoint) (_newMessage.Entity)).Point));
                }
                _engine.NextStep(ref _newMessage);
                InitCircleMessage();
            }
        }

        private void EndPointDeleteDialog()
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.DeletePoint]);
                if (_oldMessage.Action == MsgAction.Commit)
                {
                    if (_newMessage.Action == MsgAction.Approve)
                        _space.RemoveAll(state => state.Point.Equals(((SendPoint) (_newMessage.Entity)).Point));
                }
                else if (_oldMessage.Action == MsgAction.Rollback)
                {
                    if (_newMessage.Action == MsgAction.Deny)
                        _space.RemoveAll(state => state.Point.Equals(((SendPoint) (_newMessage.Entity)).Point));
                }
                _engine.NextStep(ref _newMessage);
                InitCircleMessage();
            }
        }

        private int IndexPoint(EnvironmentMessage msg)
        {
            return (_active) ? _space.FindIndex(state => state.Point.Equals(((SendPoint)(msg.Entity)).Point)) : -1;
        }

        private void EndOfLifeAgent(object idThread)
        {
            if (_active)
            {
                Thread.Sleep(responceTime[TypeMessege.DeleteAllPoint]);
                InitCircleMessage();
                _id = (int) idThread;
                // Первое собитие (сообщение) о том, что анегт прекращает свое существование, а это значит, 
                // что все точки и соединение между ними, т.е.полученная фигура должна быть вытерта из хранилища
                // вреды окружения.
                EnvironmentMessage message =
                    new EnvironmentMessage(_id, _idMessage,
                        TypeMessege.DeleteAllPoint,
                        MsgAction.Invoke, null);
                SendMessage(message);
                //Invoke(_ReceiveMsg, message);
                while (!message.AproveForAgent(_oldMessage))
                {
                }
                _endOfLife = true;
            }
        }
        public List<PointState> Show()
        {
            return _space;
        }

        public void Clear()
        {
            _space.Clear();
            _name = "";
            _color = new Color();
            _senseOfPurpose = 0;
            _purpose = null;
            _worship = 0;
            _temper = Temper.Сангвінік;
            _lifeCircle = 0;
            _timePause = 0;
            responceTime.Clear();
            responceTime = null;
            _countAttempt = 0;
            _id = 0;          
            _endOfLife = true;  
            _engine = null;   
            _startTime = DateTime.Now;
            _startPoint = new Point();  
            _space.Clear();
            _space = null; 
            _idMessage = 0;         
            _oldMessage = null; 
            _newMessage = null; 
            _env = null;       
            _registred = false;       
            _denyEnvironment = true; 
            _length = 0;           
            listConfig.Clear();
            listConfig = null;
            queue.Clear();
            queue = null;
            GC.Collect();
        }

        public void StopAgentAndClearAll()
        {
            Environment.EnvironmentMessage -= ReceiveMessage;
            _active = false;
        }

        #endregion
        #region Стандартные переопределенныме методы

        public Agent Clone()
        {
            return new Agent(_name, _color, _startPoint, _senseOfPurpose, _purpose, 
                             _worship, _temper, _lifeCircle, _engine, _timeToStart) {Length = _length};
        }
        #endregion
        #endregion
    }
}

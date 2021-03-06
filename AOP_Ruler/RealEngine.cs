﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;

namespace AOP_Ruler
{
    public class RealEngine : Engine
    {
        private Random rndTurn;
        private AgentConfig agentConfig; // Конфигурация агента
        private EnvironmentConfig environmentConfig;
        private List<PointState> _space; // Хранилище конфигурации агентов фигуры, которую строит агент
        private List<Point> _cancel;     // Хранилище точек, по которым был получен отказ от Environment
        private List<VectorPoint> _stepDown = new List<VectorPoint>();   // Хранилище точек тупикового развития конфигурации
        private EnvironmentMessage _oldMessage;     // Предыдущее сообщение
        private EnvironmentMessage _newMessage;     // Текущее сообщение
        private bool _nextFigure;                   // Флаг, который указывает осуществляется ли перестройка Engine на другую фигуру
        private int currentPositionOfConfig;        // Текущая точка в текущей конфигурации 
        private int currentConfig;                  // Текущий номер конфигурации
        private bool _endWork;
        public RealEngine()
        {
            rndTurn = new Random(3);
            _oldMessage = null;
            _newMessage = null;
            _space = new List<PointState>();
            _cancel = new List<Point>();
            _stepDown = new List<VectorPoint>();
            _nextFigure = false;
            currentConfig = 0;
            currentPositionOfConfig = 0;
            _endWork = false;
        }
        public override void NextStep(ref EnvironmentMessage message)
        {
            if ((_oldMessage != null) && (_oldMessage.ID != message.ID)) _oldMessage = null;
            else _oldMessage = _newMessage;
            _newMessage = message;
            if (message.Entity is AgentConfig)
            {
                agentConfig = (AgentConfig)message.Entity;
                _nextFigure = false;
            }
            else if (message.Entity is EnvironmentConfig)
            {
                environmentConfig = (EnvironmentConfig)_newMessage.Entity;
                _newMessage.Entity = agentConfig;
                if (_oldMessage != null)
                {
                    if (_newMessage.Message == TypeMessege.NewAgent)
                    {
                        if ((_newMessage.Action == MsgAction.Approve) &&
                            (_oldMessage.Action == MsgAction.Invoke))
                        {
                            _newMessage.Action = MsgAction.Commit;
                        }
                        else if ((_newMessage.Action == MsgAction.Approve) &&
                                 (_oldMessage.Action == MsgAction.Commit))
                        {
                            _newMessage = null;
                            _oldMessage = null;
                        }
                    }
                }
            }
            else if ((message.Entity is SendPoint) || (message.Entity == null))
            {
                switch (_newMessage.Message)
                {
                    case TypeMessege.GetPoint:
                        NextAction();
                        break;
                    case TypeMessege.DeletePoint:
                        CancelPoint();
                        break;
                }
            }
            message = _newMessage;
        }
        private void NextAction()
        {
            int i = 0;
            switch (_newMessage.Action)
            {
                case MsgAction.Invoke:
                    NextPoint();
                    break;
                case MsgAction.Approve:
                    switch (_oldMessage.Action)
                    {
                        case MsgAction.Invoke:
                            int index = _space.Count;
                            _space.Add(new PointState(index,
                                                      new Point(((SendPoint)(_newMessage.Entity)).Point.X,
                                                                ((SendPoint)(_newMessage.Entity)).Point.Y),
                                                      _oldMessage.Action));
                            _newMessage.Action = MsgAction.Commit;
                            break;
                        case MsgAction.Commit:
                            i = IndexPoint(((SendPoint) (_newMessage.Entity)).Point);
                            if (i >= 0) _space[IndexPoint(((SendPoint)(_newMessage.Entity)).Point)].State = MsgAction.Commit;
                            _newMessage = null;
                            break;
                    }
                    break;
                case MsgAction.Deny:
                    if (_oldMessage.Action == MsgAction.Commit)
                        _space.RemoveAll(state => state.Point.Equals(((SendPoint)(_newMessage.Entity)).Point));
                    _cancel.Add(((SendPoint)(_newMessage.Entity)).Point);
                    _newMessage = null;
                    break;
            }
        }

        private void CancelPoint()
        {
            switch (_newMessage.Action)
            {
                case MsgAction.Approve:
                    switch (_oldMessage.Action)
                    {
                        case MsgAction.Invoke:
                            _space.RemoveAll(state => state.Point.Equals(((SendPoint)(_newMessage.Entity)).Point));
                            _newMessage.Action = MsgAction.Commit;
                            break;
                        case MsgAction.Commit:
                            _newMessage = null;
                            break;
                    }
                    break;
                case MsgAction.Deny:
                    _space.RemoveAll(state => state.Point.Equals(((SendPoint)(_newMessage.Entity)).Point));
                    _newMessage = null;
                    break;
            }
        }

        private bool OutOfEnvironment(Point point)  // Определяет не выходит ли найденная точка за пределы размеров Environment
        {
            return ((point.X < 0) || (point.Y < 0) || (point.X > environmentConfig.Width) || (point.Y > environmentConfig.Height));
        }

        private int IndexPoint(Point point) // Осуществляет поиск точки в уже построенной фигуре
        {                                   // Возвращает -1, если элемент не найден
            return _space.FindIndex(state => state.Point.Equals(point));
        }

        private bool ApproveEngine(Point point) // Обобщенный метод, который разрешает или запрещает добавить 
        {                                       // в конфигурации найденную точку.
            bool result = !OutOfEnvironment(point);  // Находится ли данная точка в пределах координат Envirinment?
            if (result) result = (IndexPoint(point) < 0); // Не пересекайт ли новая точка нашу фигуру?
            if (result && (_cancel != null) && (_cancel.Count > 0)) result = (!_cancel.Contains(point)); // Данная точке не попадает в список уже занятых точек на Environment
            if (result) result = (!SearchStepDownPoint(point)); // Данная точка не попадает в список тупиковых точек?
            if (result) result = ((_space.Count < agentConfig.Length) || (agentConfig.Length == 0));
            return result;
        }

        private bool SearchStepDownPoint(Point point)
        {
            return ((_stepDown != null) && (_stepDown.Count > 0)) && (_stepDown.Contains(new VectorPoint(_space.Last().Point, point)));
        }

        private bool PostActionApprove(Point point)
        {
            if (!ApproveEngine(point)) return false;
            _newMessage.Action = MsgAction.Invoke;
            _newMessage.Message = TypeMessege.GetPoint;
            _newMessage.Entity = new SendPoint(_space.Count, point);
            return true;

        }
        private void NextPoint()
        {
            lock (this)
            {
                try
                    {
                        if ((agentConfig.Attempt > 0) &&
                            !((_space.Count > 0) &&
                              (_space.Last().Point.Equals(((Point) (agentConfig.Purpose.Entity))))))


                            {
                                Point point = new Point();
                                if (_nextFigure)
                                    {
                                        point = agentConfig.ListConfig[currentConfig + 1][_space.Count - 1];
                                        if (
                                            _space.Last()
                                                .Point.Equals(
                                                    agentConfig.ListConfig[currentConfig + 1][_space.Count - 1]))
                                            {
                                                _nextFigure = false;
                                                agentConfig.Attempt--;
                                                currentConfig++;
                                                if (currentConfig == agentConfig.ListConfig.Count)
                                                    {
                                                        agentConfig.Attempt = 0;
                                                        currentConfig = agentConfig.ListConfig.Count - 1;
                                                    }
                                                _oldMessage = null;
                                                _newMessage = null;
                                            }
                                        else
                                            {
                                                AddStepDownPoint();
                                                DeletePoint();
                                            }
                                    }
                                else
                                    {
                                        if (_space.Count < (agentConfig.ListConfig[currentConfig].Count))
                                            {
                                                point = agentConfig.ListConfig[currentConfig][_space.Count];
                                                if (!PostActionApprove(point))
                                                    {
                                                        _nextFigure = true;
                                                        if ((currentConfig == agentConfig.ListConfig.Count - 1) ||
                                                            (agentConfig.Attempt == 1))
                                                            {
                                                                agentConfig.Attempt = 0;
                                                                currentConfig = agentConfig.ListConfig.Count - 1;
                                                                _oldMessage = null;
                                                                _newMessage = null;
                                                            }
                                                        else
                                                            {
                                                                AddStepDownPoint();
                                                                if (_space.Count > 1)
                                                                    {
                                                                        DeletePoint();
                                                                    }
                                                                else
                                                                    {
                                                                        _stepDown.Clear();
                                                                    }
                                                            }
                                                    }
                                            }
                                        else
                                            {
                                                _nextFigure = true;
                                                if ((currentConfig == agentConfig.ListConfig.Count - 1) ||
                                                    (agentConfig.Attempt == 1))
                                                    {
                                                        agentConfig.Attempt = 0;
                                                        currentConfig = agentConfig.ListConfig.Count - 1;
                                                        _oldMessage = null;
                                                        _newMessage = null;
                                                    }
                                                else
                                                    {
                                                        AddStepDownPoint();
                                                        if (_space.Count > 1)
                                                            {
                                                                DeletePoint();
                                                            }
                                                        else
                                                            {
                                                                _stepDown.Clear();
                                                            }
                                                    }
                                            }
                                    }
                            }

                        else
                            {
                                _oldMessage = null;
                                _newMessage = null;
                                if (!_endWork)
                                    {
                                        _endWork = true;
                                        DoEndWork($"{agentConfig.Name} завершив свою побудову.");
                                    }
                            }
                    }
                catch (Exception ee)
                    {}
                if ((_newMessage != null) && (_newMessage.Entity == null))
                {
                    _oldMessage = null;
                    _newMessage = null;
                }
                
            }
        }

        private bool AllVariant(List<int> a)
        {
            return a.Contains(0) && a.Contains(1) && a.Contains(2) && a.Contains(3);
        }

        private void AddStepDownPoint()
        {
            Point firstPoint = agentConfig.StartPoint;
            Point secondPoint = agentConfig.StartPoint;
            if (_space.Count > 0)
            {
                firstPoint = _space[(_space.Count == 1) ? 0 : (_space.Count - 2)].Point;
                secondPoint = _space.Last().Point;
            }
            VectorPoint currentVector = new VectorPoint(firstPoint, secondPoint);
            if (!_stepDown.Contains(currentVector))  // Если в списке векторов с тупиковыми точками не занесен вектор  
            {                                        // с концом текущей точки, то мы создаем новый вектор с началом  
                _stepDown.Add(currentVector);        // в предыдущей и концом в текущей точках и заносим его в наш 
            }                                        // списой векторов с тупиковыми точками.
        }

        private void DeletePoint()
        {
            lock (this)
            {
                if ((_stepDown != null) && (_stepDown.Count > 0))
                {
                    _stepDown.RemoveAll(x => (x.FirstPoint.Equals((_space.Count > 0) ? _space.Last().Point : agentConfig.StartPoint)));
                }
                Point firstPoint = agentConfig.StartPoint;
                Point secondPoint = agentConfig.StartPoint;
                if (_space.Count > 0)
                {
                    firstPoint = _space[(_space.Count == 1) ? 0 : (_space.Count - 2)].Point;
                    secondPoint = _space.Last().Point;
                }
                VectorPoint currentVector = new VectorPoint(firstPoint, secondPoint);
                if (!_stepDown.Contains(currentVector))
                {
                    _stepDown.Add(currentVector);
                }
                if ((_space != null) && (_space.Count > 0))
                {
                    _newMessage.Entity = new SendPoint(_space.Count, (_space.Count > 0) ? _space.Last().Point : agentConfig.StartPoint);
                    _newMessage.Action = MsgAction.Invoke;
                    _newMessage.Message = TypeMessege.DeletePoint;
                    _space.RemoveAll(x => (x.Point.Equals(_space.Last().Point)));
                }
            }
        }
        private Point Up()
        {
            return _space.Count == 0 ? agentConfig.StartPoint : new Point(_space.Last().Point.X, _space.Last().Point.Y + 1);
        }
        private Point Down()
        {
            return _space.Count == 0 ? agentConfig.StartPoint : new Point(_space.Last().Point.X, _space.Last().Point.Y - 1);
        }
        private Point Left()
        {
            return _space.Count == 0 ? agentConfig.StartPoint : new Point(_space.Last().Point.X - 1, _space.Last().Point.Y);
        }
        private Point Right()
        {
            return _space.Count == 0 ? agentConfig.StartPoint : new Point(_space.Last().Point.X + 1, _space.Last().Point.Y);
        }
        public List<PointState> Show()
        {
            return _space;
        }
    }
}

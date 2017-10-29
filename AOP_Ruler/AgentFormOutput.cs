// Ilya Likhoshva
// AOP_Ruller
// AgentFormOutput.cs
// 25.10.2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace AOP_Ruler
{
    public class AgentFormOutput
    {
        private SortedList<int, FigureAgent> _agents;
        private PictureBox _drawingArea;
        private Graphics g;
        private Bitmap BMP;
        private Size _envSize;
        private int _kof;
        private bool _active;
        private bool _running;

        public AgentFormOutput(PictureBox PB, Size envSize) : this(new SortedList<int, FigureAgent>(), PB, envSize) { }
        public AgentFormOutput(SortedList<int, FigureAgent> agentPoints, PictureBox PB, Size envSize)
        {
            _agents = agentPoints;
            _envSize = envSize;
            _drawingArea = PB;
            _active = true;
            _running = false;
            Environment.ChangedPoints += RefreshOnChangePoints;
            InitEnvSize();
            InitKof();
            Draw();
        }

        public void AddAgentPoints(List<Point> agentPoints, Color pointColor)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                List<PointConfig> tempPointConfigs = new List<PointConfig>(agentPoints.Count);
                tempPointConfigs.AddRange(agentPoints.Select((t, i) => new PointConfig(i, t, MsgAction.Commit)));
                _agents.Add(_agents.Count, new FigureAgent(pointColor, tempPointConfigs));
                Refresh();
                _running = tempRunning;
            }
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    if (value)
                    {
                        Environment.ChangedPoints += RefreshOnChangePoints;
                    }
                    else
                    {
                        Environment.ChangedPoints -= RefreshOnChangePoints;
                    }
                }
            }
        }

        public Size Size
        {
            get { return new Size(_envSize.Width - 2, _envSize.Height - 2); }
            set
            {
                if ((value.Height != (_envSize.Height - 2)) || (value.Width != (_envSize.Width - 2)))
                {
                    _envSize = value;
                    InitEnvSize();
                    InitKof();
                    Refresh();
                }
            }
        }
        public void SetAgents(SortedList<int, FigureAgent> agentPoints)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                _agents = agentPoints;
                Refresh();
                _running = tempRunning;
            }
        }

        public void Remove(int agentNumber)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                _agents.Remove(agentNumber);
                Refresh();
                _running = tempRunning;
            }
        }
        public void Clear()
        {
            _agents?.Clear();
            _agents = null;
            _drawingArea = null;
            g = null;
            BMP = null;
            _envSize = Size.Empty;
            _kof = 0;
        }

        public void Refresh()
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                try
                {
                    Draw();
                }
                catch (ObjectDisposedException e)
                {
                    _active = false;
                }
                _running = tempRunning;
            }
        }

        private int СalculatePen(bool bold = true)
        {
            int result = 1;
            int height = 0;
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_drawingArea.InvokeRequired)
                {
                    Action action = () => height = _drawingArea.Height;
                    _drawingArea.Invoke(action);
                }
                else
                {
                    height = _drawingArea.Height;
                }
                if (height >= 800)
                    result = bold ? 7 : 4;
                else if (height >= 450 && _drawingArea.Height < 800)
                    result = bold ? 5 : 3;
                else if (height >= 200 && _drawingArea.Height < 450)
                    result = bold ? 4 : 2;
                else if (height > 140 && _drawingArea.Height < 200)
                    result = bold ? 3 : 2;
                else if (height > 95 && _drawingArea.Height <= 140)
                    result = bold ? 2 : 1;
                _running = tempRunning;
            }
            return result;
        }
        private void Draw()
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_drawingArea.InvokeRequired)
                {
                    Action action = () => BMP = null;
                    _drawingArea.Invoke(action);
                }
                else
                {
                    BMP = null;
                }
                Draw_XY();
                int penBold = СalculatePen();
                int pen = СalculatePen(false);
                List<Point> boldPoints = new List<Point>();
                List<Point> points = new List<Point>();
                Point point = new Point(0, 0);
                foreach (FigureAgent agent in _agents.Values)
                {
                    foreach (PointConfig pointConfig in agent.Points)
                    {
                        if (agent.Points.Count > 1)
                        {
                            if (pointConfig.State == MsgAction.Invoke)
                            {
                                points.Add(point);
                                points.Add(pointConfig.Point);
                            }
                            else
                            {
                                point = pointConfig.Point;
                                boldPoints.Add(pointConfig.Point);
                            }
                        }
                    }
                    if (boldPoints.Count > 1)
                    {
                        DrawLines(new Pen(agent.Color, penBold), Decode_Point(boldPoints));
                    }
                    if (points.Count > 1)
                    {
                        DrawLines(new Pen(agent.Color, pen), Decode_Point(points));
                    }
                    boldPoints.Clear();
                    points.Clear();
                }
                RefreshDrawingArea();
                _running = tempRunning;
            }
        }

        private void RefreshDrawingArea()
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_drawingArea.InvokeRequired)
                {
                    Action action = () =>
                    {
                        _drawingArea.SizeMode = PictureBoxSizeMode.CenterImage;
                        _drawingArea.Image = BMP;
                    };
                    _drawingArea.Invoke(action);
                }
                else
                {
                    _drawingArea.SizeMode = PictureBoxSizeMode.CenterImage;
                    _drawingArea.Image = BMP;
                }
                _running = tempRunning;
            }

        }
        private void DrawLines(Pen pen, Point[] points)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_drawingArea.InvokeRequired)
                {
                    Action action = () => g.DrawLines(pen, points);
                    _drawingArea.Invoke(action);
                }
                else
                {
                    g.DrawLines(pen, points);
                }
                _running = tempRunning;
            }
        }
        private void DrawLine(Pen pen, Point firstPoint, Point secondPoint)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_drawingArea.InvokeRequired)
                {
                    Action action = () => g.DrawLine(pen, firstPoint, secondPoint);
                    _drawingArea.Invoke(action);
                }
                else
                {
                    g.DrawLine(pen, firstPoint, secondPoint);
                }
                _running = tempRunning;
            }
        }
        public void Resize(PictureBox drawingArea)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_drawingArea != null)
                {
                    if (_drawingArea.InvokeRequired)
                    {
                        Action action = () => _drawingArea = drawingArea;
                        _drawingArea.Invoke(action);
                    }
                    else
                    {
                        _drawingArea = drawingArea;
                    }
                }
                else
                {
                    if (drawingArea.InvokeRequired)
                    {
                        Action action = () => _drawingArea = drawingArea;
                        drawingArea.Invoke(action);
                    }
                    else
                    {
                        _drawingArea = drawingArea;
                    }

                }
                InitKof();
                Refresh();
                _running = tempRunning;
            }
        }

        private void InitEnvSize()
        {
            _envSize = new Size(_envSize.Width + 2, _envSize.Height + 2);
        }
        private void InitKof()
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_drawingArea.InvokeRequired)
                {
                    Action action = () =>
                    {
                        _kof = (int) ((_drawingArea.Width/_envSize.Width) >=
                                      (_drawingArea.Height/_envSize.Height)
                            ? _drawingArea.Height/_envSize.Height
                            : _drawingArea.Width/_envSize.Width);
                    };
                    _drawingArea.Invoke(action);
                }
                else
                {
                    _kof = (int) ((_drawingArea.Width/_envSize.Width) >= (_drawingArea.Height/_envSize.Height)
                        ? _drawingArea.Height/_envSize.Height
                        : _drawingArea.Width/_envSize.Width);
                }
                _running = tempRunning;
            }
        }

        private void InitBMP()
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                if (_drawingArea.InvokeRequired)
                {
                    Action action = () =>
                    {
                        if (BMP == null)
                        {
                            BMP = new Bitmap(_envSize.Width*_kof + 2, _envSize.Height*_kof + 2);
                            g = Graphics.FromImage(BMP); //_drawingArea
                        }
                    };
                    _drawingArea.Invoke(action);
                }
                else
                {
                    if (BMP == null)
                    {
                        BMP = new Bitmap(_envSize.Width*_kof + 2, _envSize.Height*_kof + 2);
                        g = Graphics.FromImage(BMP); //_drawingArea
                    }
                }
                _running = tempRunning;
            }
        }
        public void Draw_XY()
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                InitBMP();
                int koord = -1;
                for (int i = 0; i <= _envSize.Width*_kof; i += _kof)
                    {
                        DrawLine(new Pen(Color.DarkGray), new Point(i, 0), new Point(i, _envSize.Height*_kof));
                        g.DrawString(koord > -1 ? (koord).ToString() : "X", new Font(FontFamily.GenericSansSerif, 10),
                            new SolidBrush(Color.DarkGray), new Point(i + 4, _kof + 4));
                        koord++;
                    }
                koord = -1;
                for (int i = 0; i <= _envSize.Height*_kof; i += _kof)
                    {
                        DrawLine(new Pen(Color.DarkGray), new Point(0, i), new Point(_envSize.Width*_kof, i));
                        g.DrawString(koord > -1 ? (koord).ToString() : "Y", new Font(FontFamily.GenericSansSerif, 10),
                            new SolidBrush(Color.DarkGray), new Point(_kof + 4, i + 4));
                        koord++;
                }
                RefreshDrawingArea();
                _running = tempRunning;
                string d = " wjdks ";
                //g.DrawString(d,);
            }
        }

        private Point[] Decode_Point(List<Point> points)
        {   // Функция преобразования массива точек мнимой
            // системы координат в массив точек с  
            // относительными координатами 
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                List<Point> tmp = new List<Point>(points.Count);
                tmp.AddRange(points.Select(t => new Point((t.X + 1)*_kof, (t.Y + 1)*_kof)));
                _running = tempRunning;
                return tmp.ToArray();
            }
            return null;
        }

        public void RefreshOnChangePoints(SortedList<int, FigureAgent> changedPoint)
        {
            if (_active)
            {
                bool tempRunning = _running;
                _running = true;
                _agents = changedPoint;
                Refresh();
                _running = tempRunning;
            }
        }
        public void StopAgentFromOutputAndClearAll()
        {
            Environment.ChangedPoints -= RefreshOnChangePoints;
            _active = false;
            //while (_running) ;
            Clear();
        }

    }
}
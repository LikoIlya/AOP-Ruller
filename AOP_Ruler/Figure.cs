using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AOP_Ruler
{
    public class Figure
    {
        private List<Point> _config; // Конфигурация фигуры без учета начального смещения 
        private List<Point> _configOffset;
        private Point _endPoint; // Конечная точка фигуры        
        private Point _offset = new Point(0, 0);
        public Figure() : this(new List<Point>())
        {}

        public Figure(List<Point> config)
        {
            _config = config;
            _configOffset = new List<Point>(_config);
        }

        public Point Offset
        {
            get { return _offset; }
            set
            {
                if (_offset != value)
                {
                    Point tempPoint;
                    _offset = value;
                    if (_config != null)
                        _configOffset = new List<Point>();
                        foreach (var point in _config)
                        {
                            tempPoint = new Point(point.X, point.Y);
                            tempPoint.Offset(value);
                            _configOffset.Add(tempPoint);
                        }
                }
            }
        } // Начальная точка

        public int MaxX => _configOffset.Aggregate(0, (current, p) => (current > p.X) ? current : p.X);
        public int MaxY => _configOffset.Aggregate(0, (current, p) => (current > p.Y) ? current : p.Y);
        public int MinX => _configOffset.Aggregate(int.MaxValue, (current, p) => (current < p.X) ? current : p.X);
        public int MinY => _configOffset.Aggregate(int.MaxValue, (current, p) => (current < p.Y) ? current : p.Y);
        public Point Purpose { get; set; } // Цель

        public int Count => _config.Count;

        private int DifferenceX
            => Math.Abs(Purpose.X - EndPoint.X);

        private int DifferenceY
            => Math.Abs(Purpose.Y - EndPoint.Y);

        public int DifferenceXY
            => Math.Abs(Purpose.X - EndPoint.X) + Math.Abs(Purpose.Y - EndPoint.Y);

        public void AddPoint(Point point)
        {
            _config.Add(point);
            _configOffset.Add(new Point(point.X + Offset.X, point.Y + Offset.Y));
            _endPoint = _config.Last();
        }

        public void AddPoints(List<Point> point)
        {
            _config.AddRange(point);
            foreach (Point p in _config)
            {
                _configOffset.Add(new Point(p.X + Offset.X, p.Y + Offset.Y));
            }
            _endPoint = _config.Last();
        }

        public List<Point> ShowWithoutOffset()
        {
            return _config;
        }

        public List<Point> Show()
        {
            return _configOffset;
        }
        public override string ToString()
        {
            return _configOffset.Aggregate("", (current, point) => current + $"({point.X},{point.Y}) ");
        }

        public Point EndPoint => new Point(_configOffset.Last().X, _configOffset.Last().Y);

        public Point this[int index]
        {
            get
            {
                if ((index >= 0) && (index < _configOffset.Count))
                    {
                        return new Point(_configOffset[index].X, _configOffset[index].Y);
                    }
                return Offset;
            }
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _configOffset.Select(i => new Point(i.X, i.Y)).GetEnumerator();
        }
    }
}
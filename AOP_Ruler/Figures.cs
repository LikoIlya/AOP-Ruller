// Ilya Likhoshva
// AOP_Ruler
// AgentFormOutput.cs
// 02.11.2015
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AOP_Ruler
{
    class Figure
    {
        private List<Point> _config;    // Конфигурация фигуры без учета начального смещения 
        private Point _endPoint;        // Конечная точка фигуры

        public Figure() : this(new List<Point>()) {}

        public Figure(List<Point> config)
        {
            _config = config;
        }

        public Point Offset { get; set; }

        public Point Purpose { get; set; }

        public int Count => _config.Count;

        private int DifferenceX  // Разница между целью и конечной точкой по Х координате
          => Math.Abs(Purpose.X - EndPoint.X);

        private int DifferenceY  // Разница между целью и конечной точкой по Y координате
            => Math.Abs(Purpose.Y - EndPoint.Y);

        public double DifferenceXY  // Разница между целью и конечной точкой по (Х, Y) координате
            => Math.Sqrt(DifferenceX * DifferenceX - DifferenceY * DifferenceY);

        public void AddPoint(Point point)
        {
            _config.Add(point);
            _endPoint = _config.Last();
        }
        public void AddPoints(List<Point> point)
        {
            _config.AddRange(point);
            _endPoint = _config.Last();
        }
        public Point[] Show()   // Возвращает массив точек без учета offset
        {
            return _config.ToArray();
        }

        public Point EndPoint => new Point(_config.Last().X + Offset.X, _config.Last().Y + Offset.Y);

        public Point this[int index]
        {
            get
            {
                if ((index >= 0) && (index < _config.Count))
                {
                    return new Point(_config[index].X + Offset.X, _config[index].Y + Offset.Y);
                }
                else return Offset;
            }
        }
        public IEnumerator<Point> GetEnumerator()
        {
            return _config.Select(i => new Point(i.X + Offset.X, i.Y + Offset.Y)).GetEnumerator();
        }
    }
}

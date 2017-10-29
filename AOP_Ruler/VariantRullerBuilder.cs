// Ilya Likhoshva
// AOP Ruler
// VariantRullerBuilder.cs
// 25.11.2015

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AOP_Ruler
{
    public class VariantRullerBuilder
    {
        private List<Figure> _figures;

        #region Конструкторы класса VariantRullerBuilder

        public VariantRullerBuilder(BothBell configs) : this(configs.Child)
        { }

        public VariantRullerBuilder(SingleBell configs) : this(configs.ToArrayBell())
        { }

        public VariantRullerBuilder(int[] configs)
        {
            _figures = new List<Figure>();
            _conf = configs;
            BuildFiguresRight(_conf);
            BuildFiguresDown(_conf);
            BuildFiguresLeft(_conf);
            BuildFiguresUp(_conf);
        }

        #endregion

        private readonly int[] _conf;

        public List<Figure> OutputFigures()
        {
            return _figures;
        }

        private void BuildFiguresLeft(int[] conf)
        {
            List<Point> tempFigPoints = new List<Point> { new Point(0, 0), new Point(-1, 0) };
            Build_Arr(tempFigPoints, conf[index], index);
        }

        private void BuildFiguresRight(int[] conf)
        {
            List<Point> tempFigPoints = new List<Point> { new Point(0, 0), new Point(1, 0) };
            Build_Arr(tempFigPoints, conf[index], index);
        }

        private void BuildFiguresUp(int[] conf)
        {
            List<Point> tempFigPoints = new List<Point> { new Point(0, 0), new Point(0, 1) };
            Build_Arr(tempFigPoints, conf[index], index);
        }

        private void BuildFiguresDown(int[] conf)
        {
            List<Point> tempFigPoints = new List<Point> { new Point(0, 0), new Point(0, -1) };
            Build_Arr(tempFigPoints, conf[index], index);
        }

        public override string ToString()
        {
            return _figures.Aggregate("", (current, fig) => current + (fig.ToString() + ";"));
        }
        private Point P_Left(Point first, Point last)
        {
            if (first.X == last.X)
            {
                if (first.Y < last.Y)
                    last.X--;
                else
                    last.X++;
            }
            else if (first.Y == last.Y)
            {

                if (first.X < last.X)
                    last.Y++;
                else
                    last.Y--;
            }
            else
            {
                last.X = 0;
                last.Y = 0;
            }
            return last;
        }

        private Point P_Forward(Point first, Point last)
        {
            if (first.X == last.X)
            {
                if (first.Y < last.Y)
                    last.Y++;
                else
                    last.Y--;
            }
            else if (first.Y == last.Y)
            {
                if (first.X < last.X)
                    last.X++;
                else
                    last.X--;
            }
            else
            {
                last.X = 0;
                last.Y = 0;
            }
            return last;
        }

        private Point P_Right(Point first, Point last)
        {
            if (first.X == last.X)
            {
                if (first.Y < last.Y)
                    last.X++;
                else
                    last.X--;
            }
            else if (first.Y == last.Y)
            {
                if (first.X < last.X)
                    last.Y--;
                else
                    last.Y++;
            }
            else
            {
                last.X = 0;
                last.Y = 0;
            }
            return last;
        }

        private int index = 0;

        private void Build_Arr(List<Point> s, int chB, int ind)
        {
            Point p;
            int checkBell = chB;
            if (s.Count < _conf.Sum() + 1)
            {
                //                    CheckBell = Conf[index];
                if (checkBell == 1)
                {
                    p = P_Left(s[s.Count - 2],
                        s[s.Count - 1]);
                    if (!s.Contains(p))
                    {
                        var newS = new List<Point>(s) { p };
                        Build_Arr(newS, _conf[ind + 1], ind + 1);
                    }
                }
                if (checkBell > 1)
                {
                    p = P_Forward(s[s.Count - 2],
                        s[s.Count - 1]);
                    if (!s.Contains(p))
                    {
                        var newS = new List<Point>(s) { p };
                        Build_Arr(newS, checkBell - 1, ind);
                    }
                }
                if (checkBell == 1)
                {

                    p = P_Right(s[s.Count - 2],
                        s[s.Count - 1]);
                    if (s.Contains(p)) return;
                    var newS = new List<Point>(s) { p };
                    Build_Arr(newS, _conf[ind + 1], ind + 1);
                }
            }
            else
            {
                _figures.Add(new Figure(s));
                /*
                                    s = null;
                */
            }
        }
    }

}
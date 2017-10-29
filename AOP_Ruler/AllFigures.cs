// Ilya Likhoshva
// AOP Ruler
// AllFigures.cs
// 25.11.2015

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AOP_Ruler
{
    public class AllFigures
    {
        public List<Bell> bellList = new List<Bell>();
        List<Figure> _allFigures = new List<Figure>();
        private Size _size = new Size(10, 10);
        public AllFigures() : this(10) { }

        public AllFigures(int Maximum) : this(Maximum, new Size(10, 10)) { }

        public AllFigures(int Maximum, Size size)
        {
            for (int i = 1; i <= Maximum; i++)
                {
                    bellList.Add(new Bell());
                    bellList.Last().Count = i;
                    bool notEndWork = true;
                    while (notEndWork)
                    {
                        notEndWork = bellList.Any(bell => ((bell != null) && (!bell.endwork())));
                    }
                    foreach (BothBell bothBell in bellList.Last().ShowBellFull())
                    {
                        _allFigures.AddRange((new VariantRullerBuilder(bothBell)).OutputFigures());
                    }
                }
            _size = size;
        }

        public List<Figure> SetOffset(Point offset, Point endPoint)
        {
            List<Figure> res = new List<Figure>(_allFigures);
            Figure tempFigure = new Figure();
            foreach (var figure in res)
            {
                figure.Offset = offset;
                figure.Purpose = endPoint;
            }
            return res;
        }

        public List<Figure> Show(int n, Point startPoint, Point endPoint, int countAttempt)
        {
            List<Figure> res = new List<Figure>();
            List<Figure> tempRes = new List<Figure>();
            List<Figure> tmp = SetOffset(startPoint, endPoint);
            int i = 0; 
            while ((countAttempt > res.Count) && (i <= n))
            {
                if (n == 0)
                {
                    tempRes = tmp.Where((f) => (f.DifferenceXY == i) &&
                                               (f.MaxX <= _size.Width) &&
                                               (f.MinX >= 0) &&
                                               (f.MaxY <= _size.Height) &&
                                               (f.MinY >= 0)).Select((f) => f).ToList();
                }
                else
                {
                    tempRes = tmp.Where((f) => (f.DifferenceXY == i) &&
                                               (f.Count == n) &&
                                               (f.MaxX <= _size.Width) &&
                                               (f.MinX >= 0) &&
                                               (f.MaxY <= _size.Height) &&
                                               (f.MinY >= 0)).Select((f) => f).ToList();
                }
                if (tempRes != null) res.AddRange(tempRes);
                i++;
            }
            return res;
        }
        public List<List<Point>> ShowListPoint(int n, Point startPoint, Point endPoint, int countAttempt)
        {
            List<Figure> res = Show(n,startPoint,endPoint,countAttempt);
            return res.Select(figure => figure.Show()).ToList();
        }
    }
}
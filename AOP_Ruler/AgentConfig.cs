using System.Collections.Generic;
using System.Drawing;

namespace AOP_Ruler
{
    class AgentConfig
    {
        public AgentConfig(string name, Color color, int senseOfPurpose, Purpose purpose, int worship, Temper temper, double lifeCircle, 
                           int attempt, SortedList <TypeMessege, int> responceTime, Point startPoint, int length, List<List<Point>> listConfig)
        {
            Name = name;
            Color = color;
            SenseOfPurpose = senseOfPurpose;// Целеустремленность. Диапазон (0..100). Измеряется в процентах
            Purpose = purpose;              // Цель, см. класс Purpose
            Worship = worship;              // Вероисповедание. Диапазон (-100..100). -100 - агент антагонист любым вероучениям. 100 - полностью верующий агент.
            Temper = temper;                // Характер. Значение по умолчанию Temper.Сангвінік
            LifeCircle = lifeCircle;        // Время жизни агента. Если _lifeCircle = 0 - бессмертен. Время задается в милисекундах. Значение по умолчанию 0 - бессмертен.
            Attempt = attempt;              // Количество попыток построить фигуру
            ResponceTime = responceTime;    // Время выполнения (отклика) на каждое из действий агента
            StartPoint = startPoint;        // Начальная точка фигуры
            Length = length;                // Длина агента (линейки)
            ListConfig = listConfig;
        }

        public List<List<Point>> ListConfig { get; set; }

        public string Name { get; set; }         // Имя агента
        public Color Color { get; set; }         // Цвет агента
        public  int SenseOfPurpose { get; set; } // Целеустремленность. Диапазон (0..100). Измеряется в процентах
        public  int Length { get; set; }         // Длина агента (линейки)
        public Purpose Purpose { get; set; }     // Цель, см. класс Purpose
        public int Worship { get; set; }         // Вероисповедание. Диапазон (-100..100). -100 - агент антагонист любым вероучениям. 100 - полностью верующий агент.
        public Temper Temper { get; set; }       // Характер. Значение по умолчанию Temper.Сангвінік
        public double LifeCircle { get; set; }   // Время жизни агента. Если _lifeCircle = 0 - бессмертен. Время задается в милисекундах. Значение по умолчанию 0 - бессмертен.
        public int Attempt { get; set; }         // Количество попыток построить фигуру
        public SortedList<TypeMessege, int> ResponceTime { get; set; } // Время выполнения (отклика) на каждое из действий агента
        public Point StartPoint { get; set; }    // Начальная точка фигуры
    }
}

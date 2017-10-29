using System.Collections.Generic;
using System.Drawing;

namespace AOP_Ruler
{
    class EnvironmentConfig
    {
        public EnvironmentConfig(int height, int width, Point offsetXY, SortedList<TypeMessege, int> response)
        {
            Height = height;
            Width = width;
            OffsetXY = offsetXY;
            Response = response;
        }
        public int Height { get; set; }    // Высота пространства (Height Space)
        public int Width { get; set; }     // Длина пространства (Width Space)
        public Point OffsetXY { get; set; }    // Смещение начала координат
        public SortedList<TypeMessege, int> Response { get; set; }  // Скорость реакции окружающей среды на различные виды событий

    }
}

using System;
using System.Collections.Generic;
using System.Drawing;

namespace AOP_Ruler
{
    /// <summary>
    /// Класс в котором организовано хранилище всех построенных фигур агентом
    /// </summary>
    public class Space : IComparable
    {
        #region Поля класса Space

        private int _id;                                // ID агента
        private SortedList<Point, StatePoint> _field;   // Коллекция 

        #endregion

        #region Конструкторы класса Space

        #endregion

        #region Свойства класса Space

        #endregion

        #region Методы класса Space

        #endregion

        public int CompareTo(object obj)
        {
            return 0;
        }
    }
}

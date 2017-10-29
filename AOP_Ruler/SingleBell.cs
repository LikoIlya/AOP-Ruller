using System;
using System.Collections.Generic;
using System.Linq;

namespace AOP_Ruler
{
    public enum TypeAlignCfg
    {
        Left,
        Right
    }
    public enum TypeSorted
    {
        Up,
        Down
    }

    [Serializable]
    public class SingleBell : IComparable<SingleBell>
    {
        #region Поля класса SingleBell
        private List<int> _config;  // Конфигурация
        private int _length; //  Количество выводимых элементов конфигурации
        private TypeSorted _sorted; // Тип сортировки
        private TypeAlignCfg _align;    // Тип выравнивания
        private int[] _show; // Массив для отображения конфигурации форматированной длины и выравнивания
        private int _full;
        #endregion
        #region Конструкторы класса SingleBell
        public SingleBell() : this(new List<int>()) { }
        public SingleBell(List<int> config) : this(config, TypeSorted.Up) { }
        public SingleBell(List<int> config, TypeSorted sorted) : this(config, sorted, TypeAlignCfg.Right) { }
        public SingleBell(List<int> config, TypeSorted sorted, TypeAlignCfg align) : this(config, config.Count, sorted, align) { }
        public SingleBell(List<int> config, int length, TypeSorted sorted, TypeAlignCfg align)
        {
            _config = config;
            _show = new int[0];
            Sorted = sorted;
            Align = align;
            Length = length;
        }
        #endregion
        #region Свойства класса SingleBell

        public int Count
        {
            get { return _config.Count; }
        }
        public int Sum => _config.Sum();

        public int Length
        {
            get { return _length; }
            set
            {
                if (value > 0) _length = value;
                RefreshShow();
                RefreshFull();
            }
        }
        public TypeSorted Sorted
        {
            get { return _sorted; }
            set { _sorted = value; }
        }

        public TypeAlignCfg Align
        {
            get { return _align; }
            set { _align = value; }
        }

        public int Capacity
        {
            get { return _config.Capacity; }
            set { if (value >= 0) _config.Capacity = value; }
        }

        public int Full
        {
            get { return _full; }
        }
        #endregion
        #region Методы класса SingleBell

        private void RefreshFull()
        {
            if (Length > Count) _full = 1;
            else if (Length == Count) _full = 0;
            else _full = -1;

        }
        private void RefreshShow()
        {
            _show = new int[Length];
            if (Length > Count)
            {
                if (Align == TypeAlignCfg.Left)
                {
                    for (int i = 0; i < Count; i++) _show[i] = _config[i];
                    for (int i = Count; i < Length; i++) _show[i] = 0;
                }
                else
                {
                    for (int i = 0; i < (Length - Count); i++) _show[i] = 0;
                    for (int i = (Length - Count); i < Length; i++) _show[i] = _config[i - Length + Count];
                }
            }
            else if (Length == Count) _show = _config.ToArray();
            else
            {
                if (Align == TypeAlignCfg.Left) for (int i = 0; i < Length; i++) _show[i] = _config[i];
                else for (int i = 0; i < Length; i++) _show[i] = _config[i + Count - Length];
            }

        }
        public void Clear()
        {
            _config.Clear();
            RefreshShow();
            RefreshFull();
        }

        public void Add(int element)
        {
            _config.Add(element);
            RefreshShow();
            RefreshFull();
        }

        public void Insert(int index, int element)
        {
            _config.Insert(index, element);
            RefreshShow();
            RefreshFull();
        }

        public void Delete(int element)
        {
            _config.Remove(element);
            RefreshShow();
            RefreshFull();
        }

        public int[] Show()
        {
            return _show;
        }
        public int this[int index]
        {
            get { return _config[index]; }
            set
            {
                if ((index < Count) && (value >= 0))
                    _config[index] = value;
            }
        }
        public IEnumerator<int> GetEnumerator()
        {

            foreach (int i in _config)
                yield return i;
        }

        public int[] ToArray()
        {
            return _show;
        }

        public int[] ToArrayBell()
        {
            return _config.ToArray();
        }
        public override string ToString()
        {
            string s = "";
            foreach (int i in _show) s += i.ToString();
            return s;
        }
        public string ToStringBell()
        {
            string s = "";
            foreach (int i in _config) s += i.ToString();
            return s;
        }
        public int CompareTo(SingleBell other)
        {
            int result = 0;
            int max = this.Length;
            if (other.Length > max) max = other.Length;
            // Сохраняем текущие настройки длины 
            int thisLength = this.Length;
            int otherLength = other.Length;
            // Устанавливаем обом объектам одинаковую длину (максимальную из двох)
            this.Length = max;
            other.Length = max;
            if (_align == TypeAlignCfg.Left)
            {
                for (int i = 0; i < max; i++)
                {
                    result = this.Show()[i].CompareTo(other.Show()[i]);
                    if (result != 0) break;
                }
                if (_sorted == TypeSorted.Down) result *= -1;
            }
            else
            {
                for (int i = (max - 1); i >= 0; i--)
                {
                    result = this.Show()[i].CompareTo(other.Show()[i]);
                    if (result != 0) break;
                }
                if (_sorted == TypeSorted.Down) result *= -1;
            }
            // Восстанавливаем значение Length у сравниваемых объектов 
            this.Length = thisLength;
            other.Length = otherLength;
            return result;
        }

        public SingleBell Clone()
        {
            List<int> a = new List<int>(_length);
            foreach (int i in _config) a.Add(i);
            return new SingleBell(a, _length, _sorted, _align);
        }

        public override bool Equals(object obj)
        {
            if (obj is SingleBell)
                return ToString().Equals(((SingleBell) (obj)).ToString());
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion

    }
}

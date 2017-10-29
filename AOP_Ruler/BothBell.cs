using System;

namespace AOP_Ruler
{
    public enum TypeCompare
    {
        FirstParents,
        FirstChild
    }

    [Serializable]
    public class BothBell : IComparable<BothBell>
    {
        #region Конструкторы класса BothBell
        public BothBell() : this(new SingleBell(), new SingleBell()) { }
        public BothBell(SingleBell parents, SingleBell child) : this(parents, child, TypeCompare.FirstParents, TypeSorted.Up, TypeAlignCfg.Left) { }
        public BothBell(SingleBell parents, SingleBell child, TypeCompare compare, TypeSorted sorted, TypeAlignCfg align)
        {
            this._parents = parents;
            this._child = child;
            Compare = compare;
            Sorted = sorted;
            Align = align;
        }
        #endregion
        #region Поля класса BothBell
        private SingleBell   _parents;
        private SingleBell   _child;
        private TypeCompare  _compare;
        private TypeSorted   _sorted;
        private TypeAlignCfg _align;
        #endregion
        #region Свойства класса BothBell
        public SingleBell Parents
        {
            get { return _parents; }
            set
            {
                if (value != null)
                    _parents = value;
            }
        }

        public SingleBell Child
        {
            get { return _child; }
            set
            {
                if (value != null)
                    _child = value; 
            }
        }

        public TypeCompare Compare
        {
            get { return _compare; }
            set { _compare = value; }
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
        #endregion
        #region Методы класса BothBell

        public override string ToString()
        {
            return Parents.ToString() + " --> " + Child.ToString();
        }

        public int CompareTo(BothBell other)
        {
            // Сохраняем все настройки
            TypeSorted sortedParentsThis   = this.Parents.Sorted;
            TypeSorted sortedParentsOther  = other.Parents.Sorted;
            TypeSorted sortedChildThis     = this.Child.Sorted;
            TypeSorted sortedChildOther    = other.Child.Sorted;
            TypeAlignCfg alignParentsThis  = this.Parents.Align;
            TypeAlignCfg alignParentsOther = other.Parents.Align;
            TypeAlignCfg alignChildThis    = this.Child.Align;
            TypeAlignCfg alignChildOther   = other.Child.Align;
            // Производим установки в соответствии со свойствами класса BothBell
            this.Parents.Sorted  = this.Sorted;
            other.Parents.Sorted = this.Sorted;
            this.Child.Sorted    = this.Sorted;
            other.Child.Sorted   = this.Sorted;
            this.Parents.Align   = this.Align;
            other.Parents.Align  = this.Align;
            this.Child.Align     = this.Align;
            other.Child.Align    = this.Align;
            int result = this.Parents.CompareTo(other.Parents);
            if (result == 0)
                result = this.Child.CompareTo(other.Child);
            // Производим восстановление свойств объектов Parents и Child как в этом объекте, так и в объекте other
            this.Parents.Sorted = sortedParentsThis;
            other.Parents.Sorted = sortedParentsOther;
            this.Child.Sorted = sortedChildThis;
            other.Child.Sorted = sortedChildOther;
            this.Parents.Align = alignParentsThis;
            other.Parents.Align = alignParentsOther;
            this.Child.Align = alignChildThis;
            other.Child.Align = alignChildOther;
            //
            return result;
        }
        public override bool Equals(object obj)
        {
            if (obj is BothBell)
                if (CompareTo((BothBell)(obj)) == 0) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        #endregion
    }
}

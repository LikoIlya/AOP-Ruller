using System;
using System.Collections.Generic;
using System.Threading;

namespace AOP_Ruler
{
    public abstract class ABell
    {
        public virtual int[][] Show()
        {
            return new int[0][];
        }
        public abstract int[][] ShowFirst();
    }
    public interface IBell
    {
        int Count
        {
            get;
            set;
        }
    }
    public class Bell : ABell, IBell, ICloneable
    {
        #region Конструкторы класса Bell
        public Bell() : this(0) { }
        public Bell(int count)
        {
            aBell = new List<SingleBell>();
            aBellFull = new List<BothBell>();
            aBellTemp = new SingleBell();
            T = new Thread[0];
            S = new bool[0];
            amountBell = new A000041(count);
            amountBellFull = new A005418(count);
            countOfAddBellFullElementEvent = 0;
            LengthChild   = count;
            LengthParents = count;
            Count = count;
        }
        #endregion
        #region Поля класса Bell
        private int count;                  // Количество звеньв линейки
        private List<SingleBell> aBell;     // Начальный список чисел Белла (наборы длин)
        private List<BothBell> aBellFull;   // Полный список всех перестановок набор длин
        private SingleBell aBellTemp;       // Временный массив для построения набора длин
        private A000041 amountBell;         // Вычесленное количество элементов будущего массива aBell
        private A005418 amountBellFull;     // Вычесленное количество элементов будущего массива aBellFull
        private Thread[] T;                 // Массив потоков, запущенных при вычислении перестановок
        private bool[] S;                   // Массив индикаторов окончания потоков
        private int _lengthChild;           // Максимальная длина конфигурации, хранящейся в aBell и в aBellFull[i].Parents
        private int _lengthParents;         // Максимальная длина конфигурации aBellFull[i]
        public DateTime timeStartBell;      // Время начала построения массива aBell
        public DateTime timeStartBellFull;  // Время начала построения массива aBellFull
        public delegate void delAddBellElement(int position, int amount, SingleBell A);// position - текущая позиция из amount, A - массив последовательности
        public static event delAddBellElement AddBellElement;  // Создание события добавления элемента массива aBell
        public delegate void delAddBellFullElement(int position, int amount, BothBell A, int relation);// position - текущая позиция из amount, A - массив последовательности, relation - для какого элемента массива aBell был сформирован текущий элемент
        public static event delAddBellFullElement AddBellFullElement;  // Создание события добавления элемента массива aBell
        private int countOfAddBellFullElementEvent;
        public TimeSpan TimeBell, TimeBellFull;
        #endregion
        #region Свойства класса Bell
        public int Length
        {
            get { return aBellFull.Count; }
        }
        public int Count
        {
            get { return count; }
            set
            {
                timeStartBell = DateTime.Now;
                timeStartBellFull = DateTime.Now;
                if (value > 0)
                {
                    count = value;
                    CreateBell();
                    //PermutationBell();
                }
                TimeBell = DateTime.Now - timeStartBell;
            }
        }

        public int LengthChild
        {
            get { return _lengthChild; }
            set { if (value > 0) _lengthChild = value; }
        }

        public int LengthParents
        {
            get { return _lengthParents; }
            set { if (value > 0) _lengthParents = value; }
        }
        #endregion
        #region Методы класса Bell
        public int[] this[int index]
        {
            get
            {
                if ((index > -1) && (index < aBellFull.Count))
                    return aBellFull[index].Child.Show();
                else
                    return null;
            }
        }
        public IEnumerator<int[]> GetEnumerator()
        {

            foreach (BothBell i in aBellFull)
                yield return i.Child.Show();
        }
        public bool endWork()
        {
            bool a = true;
            for (int i = 0; i < T.Length; i++)
            {
                if (T[i] == null)
                {
                    a = false;
                    break;
                }
                else if (T[i].ThreadState == ThreadState.Running)
                {
                    a = false;
                    break;
                }
            }
            return a;
        }
        public bool endwork()
        {
            bool a = true;
            for (int i = 0; i < S.Length; i++)
            {
                if (!S[i])
                {
                    a = false;
                    break;
                }
            }
            return a;
        }
        void Init()
        {
            amountBell.N = count;
            amountBellFull.N = count;
            T = new Thread[amountBell.Count - 1];
            S = new bool[amountBell.Count - 1];
            for (int i = 0; i < S.Length; i++) S[i] = false;
            aBell.Clear(); // Устанавливаем длину списка aBell равной 0
            aBell.Capacity = amountBell.Count;
            aBellFull.Clear();  // Устанавливаем длину списка aBellFull равной 0
            aBellFull.Capacity = amountBellFull.Count;
            aBellTemp.Clear();
            aBellTemp.Capacity = count;  // Устанавливаем длину массива aBellTemp равной Count
            for (int i = 0; i < count; i++) aBellTemp.Add(0);
            //Array.Resize<int>(ref aBellTemp, count);    // Устанавливаем длину массива aBellTemp равной Count
            //if (aBellTemp != null)
            //    aBellTemp.Clear(); // Обнуляем временный массив
            //Array.Clear(aBellTemp, 0, aBellTemp.Length); // Обнуляем временный массив
            countOfAddBellFullElementEvent = 1;
        }
        void CopyTemp()
        {

            int c = 0;  // Количество непустых элементов, находящихся в aBellTemp
            foreach (int i in aBellTemp) if (i > 0) c++;
            aBell.Add(new SingleBell(new List<int>(c), c, TypeSorted.Up, TypeAlignCfg.Right));
            for (int i = (aBellTemp.Count - 1); i >= 0; i--)
                if (aBellTemp[i] != 0)
                    aBell[aBell.Count - 1].Add(aBellTemp[i]);
            if (AddBellElement != null) // Если хоть один метод подписан на данное событие, то вызываем метод, которы генерирует событие
                AddBellElement(aBell.Count, amountBell.Count, aBell[aBell.Count - 1]);  // Вызов события
            if (aBell.Count == 1)
            {
                lock (this)
                {
                    aBellFull.Add(new BothBell(aBell[0], aBell[0]));
                    if (AddBellFullElement != null)
                        AddBellFullElement(countOfAddBellFullElementEvent++, amountBellFull.Count, aBellFull[0], 0); // Вызов события AddBellFullElement нахождения очередного элемента массива конфигураций обобщенных чисел Белла
                    if (countOfAddBellFullElementEvent > amountBellFull.Count)
                        //Suspend();
                        Stoped();
                }
            }
            else
            {
                // Используем этот код, если используем многопоточность
                T[aBell.Count - 2] = new Thread(new ParameterizedThreadStart(Generate));
                T[aBell.Count - 2].IsBackground = true;
                T[aBell.Count - 2].Start((object)(aBell.Count - 1));
                // Используем этот код, когда используем однопоточную систему
                //Generate((object)(aBell.Count - 1));
            }
        }
        int aSum(int position) // Метон подсчета всех длин во временном массиве до i-й позиции
        {
            int s = 0;
            for (int i = 0; i <= position; i++) s += aBellTemp[i];
            return s;
        }
        void Fill(int position)
        {
            int s = aSum(position);
            position++;
            int endPoint = position + count - s;
            for (int i = position; i < endPoint; i++) aBellTemp[i] = 1;
            for (int i = endPoint; i < count; i++) aBellTemp[i] = 0;
        }
        void Build()
        {
            int position = 0;
            while (aBellTemp[0] < count)
            {
                if ((position == 0) ||
                    ((aBellTemp[position] < aBellTemp[position - 1]) && (aSum(position) < count)))
                {
                    aBellTemp[position]++;
                    Fill(position);
                    CopyTemp();
                    position++;
                }
                else position--;
            }
        }
        void CreateBell()
        {
            Init();
            Build();
        }
        /*void PermutationBell()  // Метод для полных перестановок полученных конфигураций в массиве aBell[]
        {                       // Результат заносится в массив aBellFull[]
            
            aBellFull.Add(new BothBell(aBell[0], aBell[0]));
            lock (this)
            {
                if (AddBellFullElement != null) AddBellFullElement(countOfAddBellFullElementEvent++, amountBellFull.Count, aBell[0], 0, aBell[0]); // Вызов события AddBellFullElement нахождения очередного элемента массива конфигураций обобщенных чисел Белла
            }
            T = new Thread[aBell.Count - 1];
            S = new bool[aBell.Count - 1];
            for (int i = 0; i < S.Length; i++) S[i] = false;
            for (int i = 1; i < aBell.Count; i++)
            {
                T[i - 1] = new Thread(new ParameterizedThreadStart(Generate));
                T[i - 1].IsBackground = true;
                T[i - 1].Start((object)i);
                //Generate((object)i);
            }
            TimeBellFull = DateTime.Now - timeStartBellFull;
        }*/
        void Generate(object o)
        {
            int i = (int)o;
            SingleBell A = aBell[i].Clone();
            List<SingleBell> E = new List<SingleBell>();
            Gen(ref E, A, 0, i);
            if (E.Count != 0)
            {
                lock (this)
                {
                    for (int j = 0; j < E.Count; j++)
                    {
                        aBellFull.Add(new BothBell(aBell[i], E[j]));
                    }
                    S[i - 1] = true;
                }
            }
        }
        void Gen(ref List<SingleBell> E, SingleBell A, int k, int p)	// Рукурсивная процедура генерации перестановок
        {								                // Входные данные:
            // A - Входной масссив последовательности
            // k - текущее значение длины сформированной последовательности
            if (A.Count > 0)
            {
                if (k == A.Count)				// Если сформировалась последовательность длиной, которая соответствует 
                {                           // начальной длине массива последовательности, то добавляем ее в итоговый массив
                    if (!SearchBell(E, A))
                    {
                        lock (this)
                        {
                            E.Add(A.Clone());
                            if (AddBellFullElement != null)
                                AddBellFullElement(countOfAddBellFullElementEvent++, amountBellFull.Count, new BothBell(aBell[p], E[E.Count - 1]), p); // Вызов события AddBellFullElement нахождения очередного элемента массива конфигураций обобщенных чисел Белла
                        }
                    }
                }
                else                        // Если длина последовательности не достигла длине 
                                            // перевоначального массива,
                {							// производим циклические перестановки
                    SingleBell R = A.Clone(); // Создаем временный массив
                    for (int i = k; i < A.Count; i++)
                    {
                        for (int j = 0; j < R.Count; j++)
                        {
                            if (j == k) R[j] = A[i];
                            else if (j == (i)) R[j] = A[k];
                            else R[j] = A[j];
                        }
                        if (stopThread()) i = A.Count;
                        else Gen(ref E, R, k + 1, p);
                    }
                    R = null;
                }
            }
        }
        #region stopThread
        /// <summary>
        /// Метод, который указывает, что все конфигурации найдены, поэтому необходимо завершить все вычисления в потоках.
        /// Возвращает true, если все конфигурации найдены, и falce - если еще не все конфигурации найдены, и, необходимо вести дальнейший поиск.
        /// </summary>
        /// <returns></returns>
        private bool stopThread()
        {
            return (countOfAddBellFullElementEvent > amountBellFull.Count);
        }
        #endregion
        struct tempArr      // Структура, необходимая для осуществления перестановок
        {
            int[] a;        // Ствол дерева перестановок
            int i;          // Текущий номер уровня перестановок в данном стволе 
            public int[] A
            {
                get { return a; }
                set
                {
                    if (value != null)
                        if (value.Length <= a.Length)
                            value.CopyTo(a, 0);
                }
            }
            public int I
            {
                get { return i; }
                set { if (value >= 0) i = value; }
            }
            public tempArr(int j, int i)
            {
                a = new int[j];
                this.i = i;
            }
            public int this[int i]
            {
                get
                {
                    if ((i >= 0) && (i < a.Length)) return a[i];
                    else return -1;
                }
                set
                {
                    if ((i >= 0) && (i < a.Length))
                        if (value > 0) a[i] = value;
                }
            }
            public int Length
            {
                get { return a.Length; }
            }
            public IEnumerator<int> GetEnumerator()
            {

                foreach (int i in a)
                    yield return i;
            }
        }

        /*void Generate(object o)	// Нерекурсивная процедура генерации перестановок
        {								                // Входные данные:
                                                        // A - Входной масссив последовательности
                                                        // k - текущее значение длины сформированной последовательности
            DateTime t1Start = DateTime.Now;
            int p = (int)o;
            tempArr[] A = new tempArr[aBell[p].Count];
            for (int t = 0; t < A.Length; t++) A[t] = new tempArr(aBell[p].Count, t);
            A[0].A = aBell[p].ToArray();
            List<List<int>> E = new List<List<int>>();
            uint k = 0; // Текущая длина последовательности
            do
            {
                Mix(ref A, k++);
                if (k == aBell[p].Count)	// Если сформировалась последовательность длиной, которая соответствует 
                {							// начальной длине массива последовательности, то добавляем ее в итоговый массив
                    k--;
                    if (!SearchBell(E, A[k].A))
                    {
                        E.Add(A[k].A.ToList<int>());
                        if (AddBellFullElement != null) AddBellFullElement(countOfAddBellFullElementEvent++, amountBellFull.Count, A[k].A.ToArray(), p, aBell[p].ToArray()); // Вызов события AddBellFullElement нахождения очередного элемента массива конфигураций обобщенных чисел Белла
                    }
                    if (k > 0)
                    {
                        while (A[k].I >= (A[k].Length - 1)) k--;
                        for (int h = (k + 1); h < A[k].Length; h++) A[h].I = h;
                        A[k].I++;
                    }
                }
            } while (endGenerate(A));
            lock (this)
            {
                k = aBellFull.Count;
                for (int f = 0; f < E.Count; f++)
                {
                    aBellFull.Add(E[f]);
                }
                S[p - 1] = true;
            }
        }*/
        bool endGenerate(tempArr[] A)   // Метод, который определяет точку завершения перестановок
        {
            bool endGen = false;
            for (int h = 0; h < A.Length; h++)
            {
                if (A[h].I < (A[h].Length - 1))
                {
                    endGen = true;
                    h = A.Length + 1;
                }
            }
            return endGen;
        }
        /// <summary>
        /// Метод, производящий перестановку элементов массива
        /// </summary>
        /// <param name="R"></param>
        /// <param name="k"></param>
        /*void Mix(ref tempArr[] R, uint k)    // Метод, производящий перестановку элементов массива
        {
            if (k < (R.Length - 1))
            {
                int a = R[k][R[k].I];
                int b = R[k][k];
                for (int j = 0; j < R.Length; j++)
                {
                    if (j == k) R[k + 1][j] = a;
                    else if (j == R[k].I) R[k + 1][j] = b;
                    else R[k + 1][j] = R[k][j];
                }
            }
        }*/
        bool SearchBell(List<SingleBell> T, SingleBell a) // Метод, который проверяет на наличие дубликата
        {                                   // T - Массив, в котором ведется поиск, 
                                            // a - массив искомой конфигурации
                                            // Возвращает true, если такая конфигурация уже существкует
            bool r = false;
            bool k;
            if (T.Count > 0)
            {
                for (int i = 0; i < T.Count; i++)
                {
                    k = true;
                    for (int j = 0; j < T[i].Count; j++)
                    {
                        if (T[i][j] != a[j])
                        {
                            k = false;
                            j = T[i].Count;
                        }
                    }
                    r = k;
                    if (k) i = T.Count;
                }
                if (!r)
                {
                    for (int i = 0; i < T.Count; i++)
                    {
                        k = true;
                        for (int j = 0; j < T[i].Count; j++)
                        {
                            if (T[i][T[i].Count - j - 1] != a[j])
                            {
                                k = false;
                                j = T[i].Count;
                            }
                        }
                        r = k;
                        if (k) i = T.Count;
                    }
                }

            }
            return r;
        }
        public override int[][] Show()  // Метод, который возвращает итоговый массив конфигураций 
        {                               // со всеми перестановками
            int[][] A = new int[aBellFull.Count][];
            for (int i = 0; i < aBellFull.Count; i++) A[i] = aBellFull[i].Child.ToArray();
            return A;
        }
        public override int[][] ShowFirst() // Метод, который возвращает массив уникальные длин конфигураций
        {
            int[][] E = new int[aBell.Count][];
            for (int i = 0; i < aBell.Count; i++)
            {
                E[i] = aBell[i].ToArray();
            }
            return E;
        }

        public List<SingleBell> ShowBell()
        {
            return aBell;
        }

        public List<BothBell> ShowBellFull()
        {
            return aBellFull;
        } 
        public override string ToString()
        {
            string s = "";
            foreach (int[] i in this.Show())
                foreach (int j in i) s += j.ToString();
            return s;
        }
        public override bool Equals(object obj)
        {
            if ((obj is Bell) && (obj != null))
                if (this.ToString() == obj.ToString())
                    return true;
            return false;
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public object Clone()
        {
            Bell obj = new Bell();
            obj.Count = this.Count;
            return obj;
        }
        public static bool operator ==(Bell A, Bell B)
        {
            return A.Equals(B);
        }
        public static bool operator !=(Bell A, Bell B)
        {
            return !A.Equals(B);
        }
        public static int[][] operator +(Bell A, Bell B)
        {
            int len = 0;
            if (A != null) len += A.Length;
            int tempLen = len;
            if (B != null) len += B.Length;
            int[][] r = new int[len][];
            if (A != null)
            {
                for (int i = 0; i < A.Length; i++)
                {
                    r[i] = new int[A[i].Length];
                }
                Array.Copy(A.Show(), r, A.Length);
            }
            if (B != null)
            {
                for (int i = tempLen; i < r.Length; i++)
                {
                    r[i] = new int[B[i - tempLen].Length];
                }
                Array.Copy(B.Show(), 0, r, tempLen, B.Length);
            }
            return r;
        }
        /// <summary>
        /// Класс для подсчета количества вариантов последовательности A000041
        /// </summary>
        class A000041
        {
            private int[,] d;
            private int n;
            private int count;
            public A000041()
            {
                N = 1;
            }
            public A000041(int n)
            {
                N = n;
            }
            public int N
            {
                get { return n; }
                set
                {
                    if (value >= 0)
                        n = value;
                    d = new int[n + 1, n + 1];
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            d[i, j] = -1;
                    count = dec(n, n);
                }
            }
            public int Count
            {
                get { return count; }
            }
            int dec(int n, int k)
            {
                if (n >= 0 && k <= 0 && d[n, k] > 0) return d[n, k];
                if (n < 0) return 0;
                if (n <= 1 || k == 1) return 1;
                d[n, k] = dec(n, k - 1) + dec(n - k, k);
                return d[n, k];
            }
        }
        /// <summary>
        /// Класс для подсчета количества вариантов последовательности A005418
        /// </summary>
        class A005418
        {
            private int n;
            private int count;
            public A005418()
            {
                N = 1;
            }
            public A005418(int n)
            {
                N = n;
            }
            public int N
            {
                get { return n; }
                set
                {
                    if (value > 0)
                    {
                        int s = 1;
                        for (int i = 2; i <= value; i++)
                        {
                            if ((i % 2) == 0) s *= 2;
                            else s = s * 2 - (int)Math.Pow(2, (i - 3) / 2);
                        }
                        n = value;
                        count = s;
                    }
                }
            }
            public int Count
            {
                get { return count; }
            }
        }
        /// <summary>
        /// Приостанавливает работу всех процессов поиска элементов итогового массива перестановок
        /// </summary>
        public void Suspend()
        {
            int j = 0;
            foreach (Thread i in T)
            {
                if (i != null)
                    if (((i.ThreadState & (ThreadState.Stopped | ThreadState.Suspended)) == 0))
                        i.Suspend();
                j++;
            }
        }
        /// <summary>
        /// Возобновляет работу всех процессов поиска элементов итогового массива перестановок
        /// </summary>
        public void Resume()
        {
            foreach (Thread i in T)
                if (i != null)
                    if (((i.ThreadState & ThreadState.Suspended) > 0) || ((i.ThreadState & ThreadState.SuspendRequested) > 0))
                        i.Resume();
        }
        /// <summary>
        /// Останавливает работу всех процессов поиска элементов итогового массива перестановок
        /// </summary>
        public void Stoped()
        {
            foreach (Thread i in T)
                if (i != null)
                    if (((i.ThreadState & (ThreadState.Stopped | ThreadState.Suspended)) == 0))
                        i.Abort();
        }
        #endregion 
    }
}

namespace AOP_Ruler
{
    public abstract class Engine
    {
        public abstract void NextStep(ref EnvironmentMessage message);
        public delegate void EndWork(string s);
        public event EndWork EndWorkEvent;

        protected void DoEndWork(string s)
        {
            EndWorkEvent?.Invoke(s);
        }
    }
}

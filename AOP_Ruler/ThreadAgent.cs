using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using MediaPlayer;

namespace AOP_Ruler
{
    public class ThreadAgent
    {
        public ThreadAgent() : this(null, null)
        {
        }

        public ThreadAgent(Agent agent, Thread thread)
        {
            Agent = agent;
            Thread = thread;
        }

        public Agent Agent { get; set; }
        public Thread Thread { get; set; }

        public string Name
        {
            get { return Agent.Name; }
            set { Agent.Name = value; }
        }

        public Color Color
        {
            get { return Agent.Color; }
            set { Agent.Color = value; }
        }

        public int TimeToStart
        {
            get { return Agent.TimeToStart; }
            set { Agent.TimeToStart = value; }
        }

        public int LifeCircle
        {
            get { return Agent.LifeCircle; }
            set { Agent.LifeCircle = value; }
        }

        public Temper Temper
        {
            get { return Agent.Temper; }
            set { Agent.Temper = value; }
        }
        public string StartPoint
        {
            get { return "(" + Agent.StartPoint.X + ", " + Agent.StartPoint.Y + ")"; }
        }

        public string EndPoint
        {
            get { return "(" + ((Point)Agent.Purpose.Entity).X + ", " + ((Point)Agent.Purpose.Entity).Y + ")"; }
        }

        public int Length
        {
            get { return Agent.Length; }
            set { Agent.Length = value; }
        }
    }
}

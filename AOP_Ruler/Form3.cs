using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;


namespace AOP_Ruler
{
    public partial class Form3 : Form
    {
        public AllFigures AF;
        private Environment _env;
        private List<ThreadAgent> _agent = new List<ThreadAgent>();
        private AgentFormOutput drawAgent;

        private delegate void ClearAllObject();

        private event ClearAllObject clearAllObject;
        // Делегат, необходимый для создания события

        private event DelNewPoint ChangePoints;
        private  delegate void DelNewPoint(SortedList<int, FigureAgent> changedPoint);
        private readonly Window _parernt;

        //private Agent firstAgent;

        //private Agent secondAgent;

        public Form3(Window host)
        {
            InitializeComponent();
            _parernt = host;
            Init();
        }

        private void Init()
        {
            _env = new Environment { Height = 10, Width = 10, OffsetXY = new Point(0, 0) };
            drawAgent = null;
            drawAgent = new AgentFormOutput(pictureBox1, new Size(_env.Width, _env.Height));
            //firstAgent = new Agent("First Agent", Color.Red, new Point(2, 1), 50,
            //    new Purpose(50, 50, new Point(6, 6)), 50, Temper.Сангвінік) {Engine = new RealEngine()};
            //secondAgent = new Agent("Second Agent", Color.Green, new Point(8, 6), 50,
            //    new Purpose(50, 50, new Point(4, 1)), 50, Temper.Флегматик) {Engine = new RealEngine()};
            if (_agent == null) _agent = new List<ThreadAgent>();
            //_agent.Clear();
            clearAllObject += drawAgent.StopAgentFromOutputAndClearAll;
            clearAllObject += _env.StopEnvironmentAndClearAll;
        }

        public bool Running => (_agent.Any(x => x.Agent.Running));
        public void AfterRemoveForm()
        {
            clearAllObject?.Invoke();
        }

        void EndAgentWork(string s)
        {
            MessageBox.Show(s, "Побудова завершена", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            //    if (a == null)
            //        a = new AgentFormOutput(l, Color.Blue, pictureBox1, env); 
            //    a.Resize(pictureBox1);


        }

        private void Form3_Resize(object sender, EventArgs e)
        {
           // drawAgent.Resize(pictureBox1);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            побудуватиToolStripMenuItem.Enabled = false;
            очиститиToolStripMenuItem.Enabled = true;
            Waitingxaml Wait = new Waitingxaml();
            Wait.Show();
            AF = new AllFigures();
            foreach (ThreadAgent t in _agent)
            {
                t.Agent.ListConfig = AF.ShowListPoint(t.Agent.Length, t.Agent.StartPoint,
                                                      (Point) t.Agent.Purpose.Entity,
                                                      t.Agent.CountAttempt);
            }
            Wait.Close();
            foreach (ThreadAgent t in _agent)
            {
                t.Agent.Engine = new RealEngine();
                t.Agent.Engine.EndWorkEvent += EndAgentWork;
                clearAllObject += t.Agent.StopAgentAndClearAll;

                // --==[ Используем этот код, если используем многопоточность ]==--
                t.Thread = new Thread(t.Agent.ThreadStart) {IsBackground = true};
                t.Thread.Start(t.Thread.ManagedThreadId);
                // --==[ Используем этот код, когда используем однопоточную систему ]==--
                //_agent[i].Agent.ThreadStart((object)(_agent[i].Thread.ManagedThreadId));
            }

        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About About = new About();
            About.ShowDialog();
        }

        private void якБудуватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            info info = new info();
            info.ShowDialog();
        }

        private void налаштуватиСередовищеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_env == null)
                _env = new Environment
                {
                    Height = 10,
                    Width = 10,
                    OffsetXY = new Point(0, 0)
                };
            SettingOfEnvironment SOF = new SettingOfEnvironment(_env);
            SOF.ShowDialog();
            //drawAgent = null;
            //drawAgent = new AgentFormOutput(pictureBox1, new Size(_env.Width, _env.Height));
            drawAgent.Size = new Size(_env.Width, _env.Height);
            //drawAgent.Draw_XY();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clear();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            //_parernt.Close();
            //System.Windows.Application app = System.Windows.Application.Current;
            //System.Windows.Forms.Application.Exit();
            //app.Shutdown();

            _parernt.Show();
            Dispose();
        }

        private void Clear()
        {
            clearAllObject?.Invoke();
            clearAllObject -= drawAgent.StopAgentFromOutputAndClearAll;
            clearAllObject -= _env.StopEnvironmentAndClearAll;
            drawAgent = null;
            _env = null;
            //firstAgent = null;
            //secondAgent = null;
            for (int i = 0; i < _agent.Count; i++)
            {
                clearAllObject -= _agent[i].Agent.StopAgentAndClearAll;
                _agent[i].Agent.Engine.EndWorkEvent -= EndAgentWork;
                _agent[i].Agent = _agent[i].Agent.Clone();
                //_agent[i].Agent = null;
                _agent[i].Thread?.Abort();
                _agent[i].Thread = null;
                //_agent[i] = null;
            }
            //_agent.Clear();
            //_agent = null;
            GC.Collect();
        }


        private void Form3_Shown(object sender, EventArgs e)
        {
            Form3_Resize(sender, e);
        }

        private void очиститиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            очиститиToolStripMenuItem.Enabled = false;
            побудуватиToolStripMenuItem.Enabled = true;
            Clear();
            Thread.Sleep(0);
            Thread.Sleep(200);
            Init();
        }

        private void редагуванняАгентівToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AgentView formAgentView;
            formAgentView = new AgentView();
            formAgentView.DataSource = _agent;
            formAgentView.Show();
        }
    }
}

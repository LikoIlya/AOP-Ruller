using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AOP_Ruler
{
    public partial class AgentView : Form
    {
        private List<ThreadAgent> ds = new List<ThreadAgent>();
        private DataRow dr;
        private DataColumn dc;
        public AgentView()
        {
            InitializeComponent();
        }

        public List<ThreadAgent> DataSource
        {
            get { return ds;}
            set
            {
                if ((value != null) && ((ds == null) || !ds.Equals(value)))
                {
                    ds = value;
                    InitDataSource();
                }
            }
        }
        private void InitDataSource()
        {
            if (ds != null)
            {
                //dataGridView1.DataSource = ds;
                bindingSource1.DataSource = ds;
                //dataGridView1.Columns.Clear();
                //dataGridView1.Columns.AddRange(NameAgent, Color, Temper, TimeToStart, LifeCircle, StartPoint);
            }
        }

        private void InitDataRow()
        {
            //dr = new DataRow();
            //dr.ItemArray
        }
        private void EditAgent()
        {
            Agent editAgent = (Agent)dataGridView1.CurrentRow.Cells["Agent"].Value;
            Agent tempAgent = editAgent.Clone();
            AddAgent AA = new AddAgent(tempAgent, editAgent, ds) {Title = "Редагувати агент"};
            if (AA.ShowDialog() == true)
            {
                dataGridView1.CurrentRow.Cells["Agent"].Value = tempAgent;
            }
            dataGridView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditAgent();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditAgent();
        }

        private void DeleteAgent()
        {
            int index = dataGridView1.SelectedCells[0].RowIndex;
            dataGridView1.Rows.RemoveAt(index);
            dataGridView1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Ви дійсно хочете видалити цей агент?", "Увага", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DeleteAgent();
            }
        }

        private void AddAgent()
        {
            Agent tempAgent = new Agent();
            AddAgent AA = new AddAgent(tempAgent, tempAgent, ds) {Title = "Додати агент"};
            if (AA.ShowDialog() == true)
            {
                //dataGridView1.Rows.Add(new ThreadAgent(tempAgent, null));
                bindingSource1.Add(new ThreadAgent(tempAgent, null));
            }
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddAgent();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == Color.Index)
            {
                Color color = (Color) e.Value;
                e.CellStyle.BackColor = color;
                e.CellStyle.ForeColor = color;
                e.CellStyle.SelectionBackColor = color;
                e.CellStyle.SelectionForeColor = color;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Agent tempAgent;
            tempAgent = new Agent("Перший агент", System.Drawing.Color.Red, new Point(2, 1), 50,
                                  new Purpose(50, 50, new Point(6, 6)), 50,
                                  AOP_Ruler.Temper.Сангвінік); // { Engine = new RealEngine() };
            bindingSource1.Add(new ThreadAgent(tempAgent, null));
            tempAgent = new Agent("Другий агент", System.Drawing.Color.Green, new Point(8, 6), 50,
                                  new Purpose(50, 50, new Point(4, 1)), 50,
                                  AOP_Ruler.Temper.Флегматик); // { Engine = new RealEngine() };
            bindingSource1.Add(new ThreadAgent(tempAgent, null));
            tempAgent = new Agent("Третій агент", System.Drawing.Color.Blue, new Point(9, 4), 50,
                                  new Purpose(50, 50, new Point(5, 6)), 50,
                                  AOP_Ruler.Temper.Меланхолік) { Length = 0 };
            bindingSource1.Add(new ThreadAgent(tempAgent, null));
            tempAgent = new Agent("Четвертий агент", System.Drawing.Color.Yellow, new Point(8, 8), 50,
                                  new Purpose(50, 50, new Point(10, 10)), 50,
                                  AOP_Ruler.Temper.Холерик) { Length = 0 };
            bindingSource1.Add(new ThreadAgent(tempAgent, null));
            tempAgent = new Agent("П'ятий агент", System.Drawing.Color.DarkMagenta, new Point(1, 4), 50,
                                  new Purpose(50, 50, new Point(3, 8)), 50,
                                  AOP_Ruler.Temper.Сангвінік) { Length = 0 };
            bindingSource1.Add(new ThreadAgent(tempAgent, null));
        }
    }
}

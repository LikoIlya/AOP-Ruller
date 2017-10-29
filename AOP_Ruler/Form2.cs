using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOP_Ruler
{
    public partial class Form2 : Form
    {
        public delegate void dForwardProgressBar(int position, int amount);
        public dForwardProgressBar s;
        public List<Bell> bellList;
        public Form2()
        {
            InitializeComponent();
            this.bellList = new List<Bell>();
        }
        private void ShowAddBell(int position, int amount, SingleBell A)
        {
            this.progressBar1.Maximum = amount;
            this.progressBar1.Value = position;
            this.label4.Text = String.Format("Найдено {0} вариантов из {1}", position, amount);
            if (position == amount)
            {
                Bell.AddBellElement -= ShowAddBell;
            }
        }
        private void ShowAddBellFull(int position, int amount, BothBell A, int relation)
        {
            ProtectForwardProgressBar2(position, amount);
            ProtectForwardLabel4(position, amount);
            if (position == amount)
            {
                button7.Enabled = true;
                Bell.AddBellFullElement -= ShowAddBellFull;
            }
        }
        private void ProtectForwardProgressBar2(int position, int amount)
        {
            if (this.progressBar2.InvokeRequired)
            {
                dForwardProgressBar s = new dForwardProgressBar(ProtectForwardProgressBar2);
                this.Invoke(s, new object[] { position, amount });

            }
            else
            {
                if (position == amount)
                {
                    button7.Enabled = true;
                    timer2.Stop();
                }
                this.progressBar2.Maximum = amount;
                this.progressBar2.Value = position;
            }
        }
        private void ProtectForwardLabel4(int position, int amount)
        {
            if (this.label14.InvokeRequired)
            {
                dForwardProgressBar s = new dForwardProgressBar(ProtectForwardLabel4);
                this.Invoke(s, new object[] { position, amount });
            }
            else
            {

                this.label14.Text = String.Format("Найдено {0} вариантов из {1}", position, amount);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            label3.Enabled = false;
            this.tabItem = 2;
            panel2.Visible = true;
            panel21.Visible = false;
            panel22.Visible = false;
            panel4.Visible = false;
            this.timer1.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            label3.Enabled = false;
            this.tabItem = 1;
            if (StartradioButton1.Checked == false)
            {
                panel21.Visible = true;
            }
            else
            {
                panel22.Visible = true;
            }
            panel4.Visible = false;
            this.timer1.Start();
        }
        private void ChangeItem()
        {
            int r, g, b;
            int speed = 10;
            bool flagTimer = false;
            for (int i = 0; i < this.panel1.Controls.Count; i++)
            {
                if (i == this.tabItem)
                {
                    r = this.panel1.Controls[i].ForeColor.R;
                    g = this.panel1.Controls[i].ForeColor.G;
                    b = this.panel1.Controls[i].ForeColor.B;
                    if (r < 255) { r += speed; flagTimer = true; if (r > 255) r = 255; }
                    if (g < 255) { g += speed; flagTimer = true; if (g > 255) g = 255; }
                    if (b < 255) { b += speed; flagTimer = true; if (b > 255) b = 255; }
                    this.panel1.Controls[i].ForeColor = System.Drawing.Color.FromArgb(r, g, b);
                    r = this.panel1.Controls[i].BackColor.R;
                    g = this.panel1.Controls[i].BackColor.G;
                    b = this.panel1.Controls[i].BackColor.B;
                    if (r > 0) { r -= speed; flagTimer = true; if (r < 0) r = 0; }
                    if (g > 51) { g -= speed; flagTimer = true; if (g < 51) g = 51; }
                    if (b > 102) { b -= speed; flagTimer = true; if (b < 102) b = 102; }
                    this.panel1.Controls[i].BackColor = System.Drawing.Color.FromArgb(r, g, b);
                }
                else
                {
                    r = this.panel1.Controls[i].ForeColor.R;
                    g = this.panel1.Controls[i].ForeColor.G;
                    b = this.panel1.Controls[i].ForeColor.B;
                    if (r > 0) { r -= speed; flagTimer = true; if (r < 0) r = 0; }
                    if (g > 0) { g -= speed; flagTimer = true; if (g < 0) g = 0; }
                    if (b > 0) { b -= speed; flagTimer = true; if (b < 0) b = 0; }
                    this.panel1.Controls[i].ForeColor = System.Drawing.Color.FromArgb(r, g, b);
                    r = this.panel1.Controls[i].BackColor.R;
                    g = this.panel1.Controls[i].BackColor.G;
                    b = this.panel1.Controls[i].BackColor.B;
                    if (r < 240) { r += speed; flagTimer = true; if (r > 240) r = 240; }
                    if (g < 240) { g += speed; flagTimer = true; if (g > 240) g = 240; }
                    if (b < 240) { b += speed; flagTimer = true; if (b > 240) b = 240; }
                    this.panel1.Controls[i].BackColor = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
            if (!flagTimer)
                timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.ChangeItem();
        }

        private void StartradioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (StartradioButton1.Checked == false)
                label2.Text = "Ручной режим";
            else label2.Text = "Автоматический режим";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.tabItem = 1;
            this.timer1.Start();
            label3.Enabled = false;
            if (StartradioButton1.Checked == false)
            {
                panel21.Visible = true;
            }
            else
            {
                panel22.Visible = true;
            }
            panel4.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Enabled = false;
            this.tabItem = 2;
            this.timer1.Start();
            panel21.Visible = false;
            panel22.Visible = false;
            panel2.Visible = true;
            panel4.Visible = false;
        }

        private void AutoradioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoradioButton2.Checked)
            {
                panel3.Enabled = true;
                Autolabel1.Enabled = false;
                AutotextBox1.Enabled = false;
            }
            else
            {
                panel3.Enabled = false;
                Autolabel1.Enabled = true;
                AutotextBox1.Enabled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label3.Enabled = true;
            int _count;
            if (int.TryParse(AutotextBox1.Text, out _count))
            {
                bellList.Add(new Bell());
                Bell.AddBellElement += ShowAddBell;
                Bell.AddBellFullElement += ShowAddBellFull;
                bellList[0].Count = _count;
                timer2.Start();
                this.tabItem = 0;
                this.timer1.Start();
                panel2.Visible = false;
                panel21.Visible = false;
                panel22.Visible = false;
                panel4.Visible = true;
            }
            else
                MessageBox.Show("Ви ввели " + AutotextBox1.Text, "Введіть коректні данні (число)", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.tabItem = 0;
            this.timer1.Start();
            panel2.Visible = false;
            panel21.Visible = false;
            panel22.Visible = false;
            panel4.Visible = true;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label3.Enabled = false;
            this.tabItem = 2;
            panel2.Visible = true;
            panel21.Visible = false;
            panel22.Visible = false;
            panel4.Visible = false;
            this.timer1.Start();
            this.Hide();
        }

        private TimeSpan BellOst, BellFullOst;

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Maximum - progressBar1.Value != 0)
                BellOst = new TimeSpan((DateTime.Now - bellList[0].timeStartBell).Ticks / progressBar1.Value
                                   * (progressBar1.Maximum - progressBar1.Value));
            else
                BellOst = TimeSpan.Zero;
            if (progressBar1.Value != progressBar1.Maximum)
            {
                label5.Text = "Время выполнения: " + (DateTime.Now - bellList[0].timeStartBell);
                label6.Text = "Прогнозируемое время выполнения: " + BellOst;

                label7.Text = "Время выполнения всего: " + (DateTime.Now - bellList[0].timeStartBell +
                                                            BellOst);
            }
            if (progressBar2.Maximum - progressBar2.Value != 0)
                BellFullOst = new TimeSpan((DateTime.Now - bellList[0].timeStartBellFull).Ticks /
                                           progressBar2.Value * (progressBar2.Maximum - progressBar2.Value));
            else
                BellFullOst = TimeSpan.Zero;
            if (progressBar2.Value != progressBar2.Maximum)
            {
                label13.Text = "Время выполнения: " + (DateTime.Now - bellList[0].timeStartBellFull);
                label12.Text = "Прогнозируемое время выполнения: " + BellFullOst;
                label11.Text = "Время выполнения всего: " + (DateTime.Now - bellList[0].timeStartBellFull +
                                                             BellFullOst);

            }
        }
    }
}

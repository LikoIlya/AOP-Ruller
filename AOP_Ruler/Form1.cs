using System;
using System.Windows;
using System.Windows.Forms;


namespace Ruler
{
    public partial class Form1 : Form
    {
        private readonly Window _parernt;
        public Form1(Window host)
        {
            InitializeComponent();
            _parernt = host;
        }
        private void конфигурацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (configureForm == null)
                configureForm = new Form2();
            configureForm.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parernt.Show();
        }

        private void массивОбобщенныхЧиселБеллаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!((ShowForm != null) && (ShowForm.Disposing) && (ShowForm.IsDisposed)))
                ShowForm = new Form3();
            ShowForm.Show();
        }
    }

}

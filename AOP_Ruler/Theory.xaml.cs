using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Xps.Packaging;

namespace AOP_Ruler
{
    /// <summary>
    /// Логика взаимодействия для Theory.xaml
    /// </summary>
    public partial class Theory
    {
        private Window Parernt;
        public Theory(Window host)
        {
            InitializeComponent();
            Parernt = host;
        }

        void open(string s)
        {
            TextRange tr = new TextRange(
             Browser.Document.ContentStart, Browser.Document.ContentEnd);
            using (FileStream fs = File.Open(s, FileMode.Open))
            {
                tr.Load(fs, DataFormats.Rtf);
            }
        }

        private Uri _url;
        private bool _i = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            open(Directory.GetCurrentDirectory() + "/Resources/Theory/Що таке АОП.rtf");
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            if (_i) return;
            TreeViewItem tvi = (TreeViewItem) sender;
            open(Directory.GetCurrentDirectory() + "/Resources/Theory/" + tvi.Header + ".rtf");
            _i = true;
        }
        private void TreeViewItem1_Selected(object sender, RoutedEventArgs e)
        {
            if (_i) return;
            TreeViewItem tvi = (TreeViewItem)sender;
            open(Directory.GetCurrentDirectory() + "/Resources/Theory/" + tvi.Header + ".rtf");
            _i = true;
        }

        private void TreeViewItem_Unselected(object sender, RoutedEventArgs e)
        {
            _i = false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Parernt.Show();
        }
    }
}

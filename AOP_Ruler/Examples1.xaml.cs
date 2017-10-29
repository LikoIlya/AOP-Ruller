using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AOP_Ruler
{
    /// <summary>
    /// Логика взаимодействия для Examples1.xaml
    /// </summary>
    public partial class Examples1 : Window
    {
        private string[] files;
        private int i = 0;

        private readonly Window _parent;
        public Examples1(Window host)
        {
            InitializeComponent();
            _parent = host;
            DispatcherTimer timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};
            timer.Tick += timer_Tick;
            timer.Start();
            try
            {
                files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Resources\\Videos");
                mePlayer.Source = new Uri(files[0]);
                bmp1.Source =
                    new BitmapImage(
                        new Uri(Directory.GetCurrentDirectory() + "\\Resources\\thumbs" +
                                files[0].Substring(files[0].LastIndexOf("\\")) + ".bmp"));
                lbl1.Text =
                    files[0].Substring(files[0].LastIndexOf("\\") + 1)
                        .Remove(files[0].Substring(files[0].LastIndexOf("\\") + 1).Length - 4);
                bmp2.Source =
                    new BitmapImage(
                        new Uri(Directory.GetCurrentDirectory() + "\\Resources\\thumbs" +
                                files[1].Substring(files[1].LastIndexOf("\\")) + ".bmp"));
                lbl2.Text =
                    files[1].Substring(files[1].LastIndexOf("\\") + 1)
                        .Remove(files[1].Substring(files[1].LastIndexOf("\\") + 1).Length - 4);
                bmp3.Source =
                    new BitmapImage(
                        new Uri(Directory.GetCurrentDirectory() + "\\Resources\\thumbs" +
                                files[2].Substring(files[2].LastIndexOf("\\")) + ".bmp"));
                lbl3.Text =
                    files[2].Substring(files[2].LastIndexOf("\\") + 1)
                        .Remove(files[2].Substring(files[2].LastIndexOf("\\") + 1).Length - 4);
                bmp4.Source =
                    new BitmapImage(
                        new Uri(Directory.GetCurrentDirectory() + "\\Resources\\thumbs" +
                                files[3].Substring(files[3].LastIndexOf("\\")) + ".bmp"));
                lbl4.Text =
                    files[3].Substring(files[0].LastIndexOf("\\") + 1)
                        .Remove(files[3].Substring(files[3].LastIndexOf("\\") + 1).Length - 4);
                mePlayer.Play();
                mePlayer.Pause();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Ой, ошибочка вышла: " + ee.ToString());
            }
        }
       
        void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                try
                {
                    if (mePlayer.NaturalDuration.HasTimeSpan)
                        lblStatus.Content = $"{mePlayer.Position.ToString(@"mm\:ss")} / {mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
                }
                catch (Exception ee)
                { }
            }
            else
                lblStatus.Content = "No file selected...";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {   
            if (mePlayer.Source == null) return;
            mePlayer.Play();
            _playNow = true;
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (mePlayer.Source == null) return;
            mePlayer.Pause();
            _playNow = false;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (mePlayer.Source == null) return;
            mePlayer.Stop();
            _playNow = false;
        }

        private void Examples_FormClosed(object sender, EventArgs e)
        {
            _parent.Show();
            Close();
        }

        private bool _playNow = false;

        private void mePlayer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mePlayer.Source == null)
                {
                    mePlayer.Play();
                    _playNow = true;
                }
            else
                {
                    if (_playNow)
                        {
                            mePlayer.Pause();
                            _playNow = false;
                        }
                    else
                        {
                            mePlayer.Play();
                            _playNow = true;
                        }
                }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
            if (i>0)
                mePlayer.Source = new Uri(files[--i]);
            mePlayer.Play();
            _playNow = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           mePlayer.Stop();
            if (i + 1 < files.Length)
                mePlayer.Source = new Uri(files[++i]);
            mePlayer.Play();
            _playNow = true;
        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (((StackPanel) sender).Tag.ToString())
                {
                    case "0":
                        mePlayer.Source = new Uri(files[0]);
                    i = 0;
                        break;
                    case "1":
                        mePlayer.Source = new Uri(files[1]);
                    i = 1;
                        break;
                    case "2":
                        mePlayer.Source = new Uri(files[2]);
                    i = 2;
                        break;
                    case "3":
                        mePlayer.Source = new Uri(files[3]);
                    i = 3;
                        break;
                }
            mePlayer.Play();
            _playNow = true;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Input;

namespace AOP_Ruler
{
    /// <summary>
    /// Логика взаимодействия для WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        private Form3 workForm;
        public WelcomeWindow()
        {
            InitializeComponent();
        }
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void exit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application app = Application.Current;
            this.Close();
            app.Shutdown();
        }

        private void label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Theory Th = new Theory(this);
            Th.Show();
            Hide();
        }

        private void label1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Examples1 Th = new Examples1(this);
            Th.Show();
            Hide();
        }
        private void label_Copy1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            pashalka workForm = new pashalka();
            workForm.ShowDialog();
            //Hide();
        }

        private void label_Copy_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Tesing Testing = new Tesing(this);
            Testing.Show();
            Hide();
        }

        private void Label_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            About About = new About();
            About.ShowDialog();
        }
    }
}

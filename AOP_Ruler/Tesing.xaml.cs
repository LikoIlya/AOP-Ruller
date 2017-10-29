using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AOP_Ruler;

namespace AOP_Ruler
{
    /// <summary>
    /// Логика взаимодействия для Tesing.xaml
    /// </summary>
    public partial class Tesing
    {
        private readonly Window _parent;

        public Tesing(Window host)
        {
            InitializeComponent();
            _parent = host;
        }

        private List<Question> _currentTest;
        private int _currentQuestion;
        private string _currentUser;

        public int Mark { get; set; } = 0;
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            File.SetAttributes(Directory.GetCurrentDirectory() + "/Resources/Results", FileAttributes.Hidden);
        }

        private bool _i = true;

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            if (_i || textBox.Text == "Пух Вінні Аланович")
                textBox.Text = "";
            _i = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (_i || textBox.Text == "" || textBox.Text == "Пух Вінні Аланович")
                textBox.Focus();
            else
                {
                    _currentUser = textBox.Text;
                    Stack1.Visibility = Visibility.Collapsed;
                    if (radioButton_Copy.IsChecked != null && (bool) radioButton_Copy.IsChecked)
                        tryAgain.Visibility = Visibility.Collapsed;
                    Mark = 0;
                    _currentTest =
                        new Tests(Directory.GetCurrentDirectory() + "/Resources/Questions.xml").GetQuestions();
                    _currentQuestion = 0;
                    ShowQuestion(_currentQuestion);
                    Stack2.Visibility = Visibility.Visible;
                }
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!_i && textBox.Text != "") return;
            textBox.Text = "Пух Вінні Аланович";
            textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 160, 160, 160));
        }

        private void ShowQuestion(int indexOfQuestion)
        {
            if ((indexOfQuestion >= 0 && indexOfQuestion < _currentTest.Count) &&
                (indexOfQuestion >= 0 && indexOfQuestion < 12))
                {
                    label1.Content = "Питання №" + (indexOfQuestion + 1);
                    textBlock.Text = _currentTest[indexOfQuestion].GetQuestion();
                    textBlock1.Text = _currentTest[indexOfQuestion].GetAnswers()[0];
                    textBlock2.Text = _currentTest[indexOfQuestion].GetAnswers()[1];
                    textBlock3.Text = _currentTest[indexOfQuestion].GetAnswers()[2];
                    textBlock4.Text = _currentTest[indexOfQuestion].GetAnswers()[3];
                }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestion > 0)
                {
                    ShowQuestion(--_currentQuestion);
                    if (Mark > 0)
                        Mark--;
                }
        }

        private void Button_Answer_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestion + 1 < _currentTest.Count && _currentQuestion + 1 < 12)
                {
                    Button b = (Button) sender;
                    if (_currentTest[_currentQuestion].Answer(int.Parse((string) b.Tag)))
                        Mark++;
                    ShowQuestion(++_currentQuestion);
                }
            else
                {
                    Button b = (Button) sender;
                    if (_currentTest[_currentQuestion].Answer(int.Parse((string) b.Tag)))
                        Mark++;
                    Stack1.Visibility = Visibility.Collapsed;
                    Stack2.Visibility = Visibility.Collapsed;
                    label2.Content =
                        $"{_currentUser}, мої вітання!";
                    StreamReader sr = new StreamReader(new FileStream(
                        Directory.GetCurrentDirectory() + "/Resources/Results/" + _currentUser, FileMode.OpenOrCreate));
                    int lastMark;
                    string lstmrk = sr.ReadToEnd();
                    if (lstmrk.Length >= 1 &&
                        int.TryParse(Convert.ToString(lstmrk[lstmrk.Length - 3]), out lastMark))
                        {
                            if (Mark - lastMark >= 0)
                                switch (Mark - lastMark)
                                    {
                                        case 0:
                                            label3.Content =
                                                "Ти виконав тест. Цього разу так само як і минулого!";
                                            break;
                                        case 1:
                                            label3.Content =
                                                $"Ти виконав тест. Цього разу навіть на {Mark - lastMark} бал краще!";
                                            break;
                                        case 2:
                                            label3.Content =
                                                $"Ти виконав тест. Цього разу навіть на {Mark - lastMark} бала краще!";
                                            break;
                                        case 3:
                                            label3.Content =
                                                $"Ти виконав тест. Цього разу навіть на {Mark - lastMark} бала краще!";
                                            break;
                                        case 4:
                                            label3.Content =
                                                $"Ти виконав тест. Цього разу навіть на {Mark - lastMark} бала краще!";
                                            break;
                                        default:
                                            label3.Content =
                                                $"Ти виконав тест. Цього разу навіть на {Mark - lastMark} балів краще!";
                                            break;
                                    }
                        }
                    sr.Close();
                    if (Mark < 6)
                        label4.Content = "Твій результат " + Mark + ". Працюй краще!";
                    else if (Mark >= 6 && Mark < 10)
                        label4.Content = "Твій результат " + Mark + ". Непогано!";
                    else if (Mark >= 10 && Mark < 12)
                        label4.Content = "Твій результат " + Mark + ". Дуже гарно!";
                    StreamWriter fsStream =
                        File.AppendText(Directory.GetCurrentDirectory() + "/Resources/Results/" + _currentUser);
                    if (radioButton_Copy.IsChecked != null && (bool) radioButton_Copy.IsChecked)
                        fsStream.Write("Контроль ");
                    fsStream.WriteLine(DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString() + " " +
                                       _currentUser + " " + Mark);
                    fsStream.Close();
                    Stack3.Visibility = Visibility.Visible;
                }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _parent.Show();
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _parent.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Mark = 0;
            _currentTest = new Tests(Directory.GetCurrentDirectory() + "/Resources/Questions.xml").GetQuestions();
            _currentTest.Shuffle();
            _currentQuestion = 0;
            ShowQuestion(_currentQuestion);
            Stack3.Visibility = Visibility.Collapsed;
            Stack2.Visibility = Visibility.Visible;
        }
    }
}


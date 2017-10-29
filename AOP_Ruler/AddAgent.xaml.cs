using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using TextBox = System.Windows.Controls.TextBox;

namespace AOP_Ruler
{
    /// <summary>
    /// Логика взаимодействия для AddAgent.xaml
    /// </summary>
    public partial class AddAgent
    {
        private readonly SortedList<Temper, string> _tempers = new SortedList<Temper, string>
        {
            {AOP_Ruler.Temper.Сангвінік, "Сангвінік"},
            {AOP_Ruler.Temper.Холерик, "Холерик"},
            {AOP_Ruler.Temper.Меланхолік, "Меланхолік"},
            {AOP_Ruler.Temper.Флегматик, "Флегматик"}
        }; 
        private Agent _nAgent;
        private Agent _realAgent;
        public AddAgent(Agent inputWork, Agent inputReal, List<ThreadAgent> agentList)
        {
            _nAgent = inputWork;
            _realAgent = inputReal;
            AgentsList = agentList;
            InitializeComponent();
            ZapolnChange();
        }

        public List<ThreadAgent> AgentsList { get; set; }

        private IEnumerable<string> Validate(Point point)
        {
            return AgentsList.Where(x => x.Agent.StartPoint.Equals(point) && !x.Agent.Equals(_realAgent)).Select(x => x.Agent.Name);
        }

        private bool ValidateBool(Point point)
        {
            return Validate(point).ToList().Any();
        }

        private List<string> ValidateList(Point point)
        {
            return Validate(point).ToList();
        }

        private void ZapolnChange()
        {
            Name.Text = _nAgent.Name;
            _startPoint = _nAgent.StartPoint;
            StartPointX.Text = _nAgent.StartPoint.X.ToString();
            StartPointY.Text = _nAgent.StartPoint.Y.ToString();
            SenseOfPurpose.Value = _nAgent.SenseOfPurpose;
            TextSOP.Text = (_nAgent.SenseOfPurpose).ToString();
            LengthOfLine.Text = _nAgent.Length.ToString();
            Importance.Value = _nAgent.Purpose.Importance;
            TextImp.Text = (_nAgent.Purpose.Importance).ToString();
            WorshipIdeals.Value = _nAgent.Purpose.Worship;
            TextWorId.Text = (_nAgent.Purpose.Worship).ToString();
            _endPoint = (Point)_nAgent.Purpose.Entity;
            EndPointX.Text = ((Point)_nAgent.Purpose.Entity).X.ToString();
            EndPointY.Text = ((Point)_nAgent.Purpose.Entity).Y.ToString();
            Worship.Value = _nAgent.Worship;
            TimeOfLife.Text = _nAgent.LifeCircle.ToString();
            _nAgent.LifeCircle++;
            _nAgent.LifeCircle--;
            Time_Of_Start.Text = _nAgent.TimeToStart.ToString();
            TextWorsh.Text = (_nAgent.Worship).ToString();
            NumberOfTries.Text = _nAgent.CountAttempt.ToString();
            Temper.ItemsSource = _tempers.Values;
            Temper.SelectedItem = _tempers[_nAgent.Temper];
            TimePause.Text = _nAgent.TimePause.ToString();
            ResTimeInitEnv.Text = _nAgent.ResponseTimeInitEnvironment.ToString();
            ResTimeNewAgent.Text = _nAgent.ResponseTimeNewAgent.ToString();
            ResTimeGetPoint.Text = _nAgent.ResponseTimeGetPoint.ToString();
            ResTimeDelPoint.Text = _nAgent.ResponseTimeDeletePoint.ToString();
            ResTimeDelAllPoint.Text = _nAgent.ResponseTimeDeleteAllPoint.ToString();
            Color.Background =
                        new SolidColorBrush(System.Windows.Media.Color.FromArgb(_nAgent.Color.A, _nAgent.Color.R, _nAgent.Color.G,
                            _nAgent.Color.B));
            Color.Foreground =
                new SolidColorBrush(System.Windows.Media.Color.FromArgb(_nAgent.Color.A, (byte)(0xFF - _nAgent.Color.R),
                   (byte)(0xFF - _nAgent.Color.G), (byte)(0xFF - _nAgent.Color.B)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_nAgent == null) return;
            ColorDialog cd = new ColorDialog {
                FullOpen = true,
                AnyColor = true,
                AllowFullOpen = true
            };
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _nAgent.Color = cd.Color;
                    Color.Background =
                        new SolidColorBrush(System.Windows.Media.Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G,
                            cd.Color.B));
                    Color.Foreground =
                        new SolidColorBrush(System.Windows.Media.Color.FromArgb(cd.Color.A, (byte)(0xFF - cd.Color.R),
                           (byte)(0xFF - cd.Color.G), (byte)(0xFF - cd.Color.B)));
                }

        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_nAgent!=null)
                    _nAgent.Name = Name.Text;
        }

        private Point _startPoint = new Point(0, 0);
        private void StartPointX_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nAgent == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(StartPointX.Text, out k) && StartPointX.Text != "" &&
                    uint.Parse(StartPointX.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    StartPointX.Select(0, StartPointX.Text.Length);
                }
                if (ValidateBool(new Point((int) k, _startPoint.Y)))
                {
                    string s = $"Дана точка ({k},{_startPoint.Y}) вже є початком для агент";
                    if (ValidateList(new Point((int) k, _startPoint.Y)).Count > 1)
                    {
                        s += "ів: ";
                        s = ValidateList(new Point((int) k, _startPoint.Y))
                            .Aggregate(s, (current, name) => current + $"{name}, ");
                    }
                    else
                    {
                        s += $"а {ValidateList(new Point((int) k, _startPoint.Y))[0]}, ";
                    }
                    s += "це призводить до конфлікту агентів. Побудується лише один агент з цієї точки. Ви впевненні?";
                    if (MessageBox.Show(s,
                        @"Помилка введення", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                        StartPointX.Text = _startPoint.X.ToString();
                    else
                        _startPoint.X = (int) k;
                }
                else
                    _startPoint.X = (int) k;
                _nAgent.StartPoint = _startPoint;
                ZapolnChange();
            }
            catch (Exception)
            {
                StartPointX.Text = "0";
                ZapolnChange();
            }
        }

        private void StartPointY_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nAgent == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(StartPointY.Text, out k) && StartPointY.Text != "" &&
                    uint.Parse(StartPointY.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    StartPointY.Select(0, StartPointY.Text.Length);
                }
                if (ValidateBool(new Point(_startPoint.X, (int)k)))
                {
                    string s = $"Дана точка ({_startPoint.X},{k}) вже є початком для агент";
                    if (ValidateList(new Point(_startPoint.X, (int)k)).Count > 1)
                    {
                        s += "ів: ";
                        s = ValidateList(new Point(_startPoint.X, (int)k))
                            .Aggregate(s, (current, name) => current + $"{name}, ");
                    }
                    else
                    {
                        s += $"а {ValidateList(new Point(_startPoint.X, (int)k))[0]}, ";
                    }
                    s += "це призводить до конфлікту агентів. Побудується лише один агент з цієї точки. Ви впевненні?";
                    if (MessageBox.Show(s,
                        @"Помилка введення", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                        StartPointY.Text = _startPoint.Y.ToString();
                    else
                        _startPoint.Y = (int)k;
                }
                else
                    _startPoint.Y = (int)k;
                _nAgent.StartPoint = _startPoint;
                ZapolnChange();
            }
            catch (Exception)
            {
                StartPointY.Text = "0";
                ZapolnChange();
            }
        }

        private void SenseOfPurpose_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_nAgent == null) return;
            _nAgent.SenseOfPurpose = (int)SenseOfPurpose.Value;
            TextSOP.Text = ((int)SenseOfPurpose.Value).ToString();
            ZapolnChange();
        }

        private void LengthOfLine_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nAgent == null) return;
            uint k;
            //if (!uint.TryParse(LengthOfLine.Text, out k) && StartPointY.Text != "" && uint.Parse(EndPointY.Text) >= int.MaxValue)
            //    {
            //       MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення", MessageBoxButtons.OK,
            //            MessageBoxIcon.Warning);
            //    LengthOfLine.Select(0, LengthOfLine.Text.Length);
            //}
            uint.TryParse(LengthOfLine.Text, out k);
            if (k > 10)
                {
                    MessageBox.Show(@"Ви ввели число більше за 10, довжина лінійки дорівнюватиме 10!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                LengthOfLine.Text = "10";
                _nAgent.Length = 10;
                }
            if (k < 0)
            {
                MessageBox.Show(@"Ви ввели число менше від 0, довжина лінійки дорівнюватиме 1!",
                    @"Помилка введення", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                LengthOfLine.Text = "1";
                _nAgent.Length = 1;
            }
            if (k > -1 && k < 11)
                _nAgent.Length = (int) k;
            ZapolnChange();
        }

        private void Importance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_nAgent == null) return;
            _nAgent.PurposeImportance = (int)Importance.Value;
            TextImp.Text = ((int)Importance.Value).ToString();
            ZapolnChange();
        }

        private void WorshipIdeals_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_nAgent == null) return;
            _nAgent.PurposeWorship = (int)WorshipIdeals.Value;
            TextWorId.Text = ((int)WorshipIdeals.Value).ToString();
            ZapolnChange();
        }

        private Point _endPoint = new Point(0, 0);
        private void EndPointX_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nAgent == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(EndPointX.Text, out k) && EndPointX.Text != "" &&
                    uint.Parse(EndPointX.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    EndPointX.Select(0, EndPointX.Text.Length);
                }
                _endPoint.X = (int) k;
                _nAgent.Purpose.Entity = _endPoint;
                ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ((TextBox)sender).Text = "0";
                ZapolnChange();
            }
        }

        private void EndPointY_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nAgent == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(EndPointY.Text, out k) && EndPointY.Text != "" &&
                    uint.Parse(EndPointY.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    EndPointY.Select(0, EndPointY.Text.Length);
                }
                _endPoint.Y = (int) k;
                _nAgent.Purpose.Entity = _endPoint;
                ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                EndPointY.Text = "0";
                ZapolnChange();
            }
        }

        private void Worship_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_nAgent == null) return;
            _nAgent.Worship = (int)Worship.Value;
            TextWorsh.Text = ((int)Worship.Value).ToString();
            ZapolnChange();
        }

        private void TimeOfLife_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nAgent == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(TimeOfLife.Text, out k) && TimeOfLife.Text != "" && uint.Parse(TimeOfLife.Text) >= int.MaxValue)
            {
               MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                TimeOfLife.Select(0, TimeOfLife.Text.Length);
            }
            _nAgent.LifeCircle = (int)k;
            ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                TimeOfLife.Text = "0";
                ZapolnChange();
            }
           
        }

        private void NumberOfTries_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (_nAgent == null) return;
            //uint k;
            //if (!uint.TryParse(NumberOfTries.Text, out k) && NumberOfTries.Text != "" && uint.Parse(EndPointY.Text) >= int.MaxValue)
            //    {
            //       MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення", MessageBoxButtons.OK,
            //            MessageBoxIcon.Warning);
            //    NumberOfTries.Select(0, NumberOfTries.Text.Length);
            //}
            //_nAgent.CountAttempt = (int)k;
        }

        private void Temper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_nAgent == null || Temper.Text == "") return;
            _nAgent.Temper = _tempers.Keys[_tempers.Values.IndexOf(Temper.SelectedItem.ToString())];
            ZapolnChange();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult == null)
            {
                DialogResult = false;
            }
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            ZapolnChange();
        }

        private void Time_Of_Start_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nAgent == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(Time_Of_Start.Text, out k) && Time_Of_Start.Text != "" && uint.Parse(Time_Of_Start.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення", MessageBoxButtons.OK,
                         MessageBoxIcon.Warning);
                    Time_Of_Start.Select(0, Time_Of_Start.Text.Length);
                }
                _nAgent.TimeToStart = (int)k;
                ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Time_Of_Start.Text = "0";
                ZapolnChange();
            }
        }


    }
}

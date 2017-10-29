using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace AOP_Ruler
{
    /// <summary>
    /// Логика взаимодействия для SettingOfEnvironment.xaml
    /// </summary>
    public partial class SettingOfEnvironment : Window
    {
        private Environment _nEnvironment;

        public SettingOfEnvironment(Environment env)
        {
            _nEnvironment = env;
            InitializeComponent();
            ZapolnChange();
        }
        private void ZapolnChange()
        {
            Height.Text = _nEnvironment.Height.ToString();
            Width.Text = _nEnvironment.Width.ToString();
            ForPoints = _nEnvironment.Respose;
            AnsAddPoint.Text = _nEnvironment.Respose[TypeMessege.GetPoint].ToString();
            AnsAddAgent.Text = _nEnvironment.Respose[TypeMessege.NewAgent].ToString();
            AnsDelPoint.Text = _nEnvironment.Respose[TypeMessege.DeletePoint].ToString();
            AnsDelAllPoint.Text = _nEnvironment.Respose[TypeMessege.DeleteAllPoint].ToString();
            AnsEnvBorn.Text = _nEnvironment.Respose[TypeMessege.InitEnvironment].ToString();
            KofSOPStep.Text = Agent.KStepSenseOfPurpose.ToString();
            KofImpStep.Text = Agent.KStepPurposeImportance.ToString();
            KofPurpWorshStep.Text = Agent.KStepPurposeWorship.ToString();
            KofWorshStep.Text = Agent.KStepWorship.ToString();
            KofLifeStep.Text = Agent.KStepLifeCircle.ToString();
            KofSOPTries.Text = Agent.KTrySenseOfPurpose.ToString();
            KofImpTries.Text = Agent.KTryPurposeImportance.ToString();
            KofPurpWorshTries.Text = Agent.KTryPurposeWorship.ToString();
            KofWorshTries.Text = Agent.KTryWorship.ToString();
            KofLifeTries.Text = Agent.KTryLifeCircle.ToString();
            AverageTimePause.Text = Agent.AverageTimePause.ToString();
            StartCountAttempt.Text = Agent.StartCountAttempt.ToString();
        }
        private void Height_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            uint k = 10;
            try
                {
                    if (uint.TryParse(Height.Text, out k) && Height.Text != "" &&
                        uint.Parse(Height.Text) >= 20)
                        {
                            MessageBox.Show($"Введіть невід'ємне число менше за 20, будь ласка!",
                                @"Помилка введення", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            Height.Select(0, Height.Text.Length);
                            k = 10;
                            Height.Text = "10";
                        }
                    else
                        {
                            _nEnvironment.Height = (int) k;
                            ZapolnChange();
                        }
                    if (k > 10 &&
                        MessageBox.Show(@"Ви впевнені? Прорахунок може зайняти досить багато часу та ресурсів!",
                            @"Велике число", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                        {

                            _nEnvironment.Height = (int) 10;
                            ZapolnChange();
                        }

                }
            catch (Exception)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за 20, будь ласка!",
                        @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    ((TextBox) sender).Text = "1";
                    ZapolnChange();
                }
        }

        private void Width_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            uint k;
            try
                {
                if (uint.TryParse(Width.Text, out k) && Width.Text != "" &&
                    uint.Parse(Width.Text) >= 20)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за 20, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    Width.Select(0, Width.Text.Length);
                    k = 10;
                    Width.Text = "10";
                }
                else
                {
                    _nEnvironment.Width = (int)k;
                    ZapolnChange();
                }
                if (k > 10 &&
                    MessageBox.Show(@"Ви впевнені? Прорахунок може зайняти досить багато часу та ресурсів!",
                        @"Велике число", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                {

                    _nEnvironment.Width = (int)10;
                    ZapolnChange();
                }

            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за 20, будь ласка!", @"Помилка введення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ((TextBox) sender).Text = "1";
                ZapolnChange();
            }
        }

        private SortedList<TypeMessege,int> ForPoints;

        private void AnsAddPoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(AnsAddPoint.Text, out k) && AnsAddPoint.Text != "" &&
                    uint.Parse(AnsAddPoint.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AnsAddPoint.Select(0, AnsAddPoint.Text.Length);
                }
                ForPoints[TypeMessege.GetPoint] = (int) k;
                _nEnvironment.Respose = ForPoints;
                ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ((TextBox) sender).Text = "0";
                ZapolnChange();
            }
        }

        private void AnsAddAgent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(AnsAddAgent.Text, out k) && AnsAddAgent.Text != "" &&
                    uint.Parse(AnsAddAgent.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AnsAddAgent.Select(0, AnsAddAgent.Text.Length);
                }
                ForPoints[TypeMessege.NewAgent] = (int) k;
                _nEnvironment.Respose = ForPoints;
                ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ((TextBox) sender).Text = "0";
                ZapolnChange();
            }
        }

        private void AnsDelPoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(AnsDelPoint.Text, out k) && AnsDelPoint.Text != "" &&
                    uint.Parse(AnsDelPoint.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AnsDelPoint.Select(0, AnsDelPoint.Text.Length);
                }
                ForPoints[TypeMessege.DeletePoint] = (int) k;
                _nEnvironment.Respose = ForPoints;
                ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ((TextBox) sender).Text = "0";
                ZapolnChange();
            }
        }

        private void AnsDelAllPoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(AnsDelAllPoint.Text, out k) && AnsDelAllPoint.Text != "" &&
                    uint.Parse(AnsDelAllPoint.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AnsDelAllPoint.Select(0, AnsDelAllPoint.Text.Length);
                }
                ForPoints[TypeMessege.DeleteAllPoint] = (int) k;
                _nEnvironment.Respose = ForPoints;
                ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ((TextBox) sender).Text = "0";
                ZapolnChange();
            }
        }

        private void AnsEnvBorn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            uint k;
            try
            {
                if (!uint.TryParse(AnsEnvBorn.Text, out k) && AnsEnvBorn.Text != "" &&
                    uint.Parse(AnsEnvBorn.Text) >= int.MaxValue)
                {
                    MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!",
                        @"Помилка введення", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AnsEnvBorn.Select(0, AnsEnvBorn.Text.Length);
                }
                ForPoints[TypeMessege.InitEnvironment] = (int) k;
                _nEnvironment.Respose = ForPoints;
                ZapolnChange();
            }
            catch (Exception)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ((TextBox) sender).Text = "0";
                ZapolnChange();
            }
        }

        private void KofSOPStep_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofSOPStep.Text, out k);
            if (!float.TryParse(KofSOPStep.Text, out k) && KofSOPStep.Text != "" && (float.Parse(KofSOPStep.Text) <= 0 || float.Parse(KofSOPStep.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofSOPStep.Select(0, KofSOPStep.Text.Length);
            }
            Agent.KStepSenseOfPurpose = k;
            ZapolnChange();
        }

        private void KofImpStep_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofImpStep.Text, out k);
            if (!float.TryParse(KofImpStep.Text, out k) && KofImpStep.Text != "" && (float.Parse(KofImpStep.Text) <= 0 || float.Parse(KofImpStep.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    KofImpStep.Select(0, KofImpStep.Text.Length);
                }
            Agent.KStepPurposeImportance = k;
            ZapolnChange();
        }

        private void KofPurpWorshStep_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofPurpWorshStep.Text, out k);
            if (!float.TryParse(KofPurpWorshStep.Text, out k) && KofPurpWorshStep.Text != "" && (float.Parse(KofPurpWorshStep.Text) <= 0 || float.Parse(KofPurpWorshStep.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofPurpWorshStep.Select(0, KofPurpWorshStep.Text.Length);
            }
            Agent.KStepPurposeWorship = k;
            ZapolnChange();
        }

        private void KofWorshStep_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofWorshStep.Text, out k);
            if (!float.TryParse(KofWorshStep.Text, out k) && KofWorshStep.Text != "" && (float.Parse(KofWorshStep.Text) <= 0 || float.Parse(KofWorshStep.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofWorshStep.Select(0, KofWorshStep.Text.Length);
            }
            Agent.KStepWorship = k;
            ZapolnChange();
        }

        private void KofLifeStep_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofLifeStep.Text, out k);
            if (!float.TryParse(KofLifeStep.Text, out k) && KofLifeStep.Text != "" && (float.Parse(KofLifeStep.Text) <= 0 || float.Parse(KofLifeStep.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofLifeStep.Select(0, KofLifeStep.Text.Length);
            }
            Agent.KStepLifeCircle = k;
            ZapolnChange();
        }

        private void KofSOPTries_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofSOPTries.Text, out k);
            if (!float.TryParse(KofSOPTries.Text, out k) && KofSOPTries.Text != "" && (float.Parse(KofSOPTries.Text) <= 0 || float.Parse(KofSOPTries.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofSOPTries.Select(0, KofSOPTries.Text.Length);
            }
            Agent.KTrySenseOfPurpose = k;
            ZapolnChange();
        }

        private void KofImpTries_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofImpTries.Text, out k);
            if (!float.TryParse(KofImpTries.Text, out k) && KofImpTries.Text != "" && (float.Parse(KofImpTries.Text) <= 0 || float.Parse(KofImpTries.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofImpTries.Select(0, KofImpTries.Text.Length);
            }
            Agent.KTryPurposeImportance = k;
            ZapolnChange();
        }

        private void KofPurpWorshTries_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofPurpWorshTries.Text, out k);
            if (!float.TryParse(KofPurpWorshTries.Text, out k) && KofPurpWorshTries.Text != "" && (float.Parse(KofPurpWorshTries.Text) <= 0 || float.Parse(KofPurpWorshTries.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofPurpWorshTries.Select(0, KofPurpWorshTries.Text.Length);
            }
            Agent.KTryPurposeWorship = k;
            ZapolnChange();
        }

        private void KofWorshTries_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofWorshTries.Text, out k);
            if (!float.TryParse(KofWorshTries.Text, out k) && KofWorshTries.Text != "" && (float.Parse(KofWorshTries.Text) <= 0 || float.Parse(KofWorshTries.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofWorshTries.Select(0, KofWorshTries.Text.Length);
            }
            Agent.KTryWorship = k;
            ZapolnChange();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void KofLifeTries_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_nEnvironment == null) return;
            float k;
            float.TryParse(KofLifeTries.Text, out k);
            if (!float.TryParse(KofLifeTries.Text, out k) && KofLifeTries.Text != "" && (float.Parse(KofLifeTries.Text) <= 0 || float.Parse(KofLifeTries.Text) >= 1))
                {
                    MessageBox.Show(@"Введіть невід'ємне число від 0 до 1, будь ласка!", @"Помилка введення",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                KofLifeTries.Select(0, KofLifeTries.Text.Length);
            }
            Agent.KTryLifeCircle = k;
            ZapolnChange();
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            ZapolnChange();
        }

        private void StartCountAttempt_TextChanged(object sender, TextChangedEventArgs e)
        {
            uint k;
            if (!uint.TryParse(StartCountAttempt.Text, out k) && StartCountAttempt.Text != "" && uint.Parse(StartCountAttempt.Text) >= int.MaxValue)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                StartCountAttempt.Select(0, StartCountAttempt.Text.Length);
            }
            Agent.StartCountAttempt = (int) k;
            ZapolnChange();
        }

        private void AverageTimePause_TextChanged(object sender, TextChangedEventArgs e)
        {
            uint k;
            if (!uint.TryParse(AverageTimePause.Text, out k) && AverageTimePause.Text != "" && uint.Parse(AverageTimePause.Text) >= int.MaxValue)
            {
                MessageBox.Show($"Введіть невід'ємне число менше за {int.MaxValue}, будь ласка!", @"Помилка введення", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AverageTimePause.Select(0, AnsDelPoint.Text.Length);
            }
            Agent.AverageTimePause = (int)k;
            ZapolnChange();
        }
    }
}

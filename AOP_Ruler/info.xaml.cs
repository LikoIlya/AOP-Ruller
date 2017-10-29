using System;
using System.IO;

namespace AOP_Ruler
{
    /// <summary>
    /// Логика взаимодействия для info.xaml
    /// </summary>
    public partial class info
    {
        public info()
        {
            InitializeComponent();
            web.Navigate(new Uri(Directory.GetCurrentDirectory() + "/Resources/info.htm")); 
        }
    }
}

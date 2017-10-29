using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Media.Animation;

namespace _3D_Gallery
{
    public partial class ListBoxGalleryTemplate : ResourceDictionary
    {
        public ListBoxGalleryTemplate()
        {
            InitializeComponent();
        }

        TextBlock tb;

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject dobj = VisualTreeHelper.GetParent(
                (ListBoxItem)((FrameworkElement)sender).TemplatedParent);

            while ((dobj as ListBox) == null)
            {
                dobj = VisualTreeHelper.GetParent(dobj);
            }

            if (dobj != null)
                (dobj as ListBox).SelectedIndex = -1;
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            tb = sender as TextBlock;
        }

        private void DoubleAnimation_Changed(object sender, EventArgs e)
        {
            DoubleAnimation dbn = sender as DoubleAnimation;
            dbn.To = Convert.ToDouble(tb.Text);
        }
    }
}


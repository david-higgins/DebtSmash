using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DebtSmash.Views.WPF
{
    /// <summary>
    /// Interaction logic for GetConnString.xaml
    /// </summary>
    public partial class GetConnString : Window
    {
        public GetConnString()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public String[] others { get { return (String[])GetValue(othersProperty); } set { SetValue(othersProperty, value); if (value.Length > 0) cothers.SelectedIndex = 0; } }
        public static readonly DependencyProperty othersProperty = DependencyProperty.Register("others", typeof(String[]), typeof(GetConnString), new PropertyMetadata(new String[0]));

        private void cothers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectionString.Text = cothers.SelectedValue as String;
        }

        public const String acd = "AutoConnect.dat";
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (cbauto.IsChecked.HasValue && cbauto.IsChecked.Value)
                File.WriteAllText(acd, cothers.SelectedValue as String);
            else if (File.Exists(acd))  
                File.Delete(acd);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
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
using DebtSmash.Presenter;

namespace DebtSmash.Views.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class MainWindow : Window, IView
    {
        public void Dinv(Action act)
        {
            Dispatcher.Invoke(act);
        }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            DataContext = this;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DebtSmashPresenter.Present(this);
        }

        public UpdateBindingList<Debt> debt { get { return (UpdateBindingList<Debt>)GetValue(debtProperty); } set { Dinv(() => SetValue(debtProperty, value)); } }
        public static readonly DependencyProperty debtProperty = DependencyProperty.Register("debt", typeof(UpdateBindingList<Debt>), typeof(MainWindow), new PropertyMetadata(new UpdateBindingList<Debt>()));

        public void HeresTheDebt(IUpdateList<Debt> debt)
        {
            this.debt = (UpdateBindingList<Debt>)debt;
        }

        private void AlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecuteIfDebtSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DebtList.SelectedIndex >= 0;
        }

        void AddDebt(object sender, ExecutedRoutedEventArgs e)
        {
            var newone = new Debt();
            Setter(newone);
            debt.Add(newone);
        }
        void DeleteSelectedDebt(object sender, ExecutedRoutedEventArgs e)
        {
            debt.RemoveAt(DebtList.SelectedIndex);
        }
        void EditSelectedDebt(object sender, ExecutedRoutedEventArgs e)
        {
            var edit_debt = debt[DebtList.SelectedIndex];
            Setter(edit_debt);
            debt.Update(DebtList.SelectedIndex);
        }
        void Setter(Debt toset)
        {
            int tb = 0;
            int.TryParse(DebtBurnCount.Text, out tb);
            toset.name = DebtName.Text;
            toset.description = DebtDesc.Text;
            toset.timesBurned = tb;
        }

        public string GetConnectionString(String[] others)
        {
            if (File.Exists(GetConnString.acd))
                return File.ReadAllText(GetConnString.acd);
            var gcs = new GetConnString();
            gcs.others = others;
            gcs.ShowDialog();
            return gcs.ConnectionString.Text;
        }

        public bool loading { get { return (bool)GetValue(loadingProperty); } set { Dinv(() => SetValue(loadingProperty, value)); } }
        public static readonly DependencyProperty loadingProperty = DependencyProperty.Register("loading", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (File.Exists(GetConnString.acd))
                File.Delete(GetConnString.acd);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
             e.CanExecute =  File.Exists(GetConnString.acd);
        }

        MarkdownSharp.Markdown md = new MarkdownSharp.Markdown();
        private void DebtDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            DescHTML.NavigateToString(GetMarkdownPage(DebtDesc.Text, Properties.Settings.Default.markdowncssDefault));
        }
        String GetMarkdownPage(String markdowntext, String markdowncss)
        {
            return
@"
<html>
  <head>
    <style>
"
+ markdowncss +
@"
    </style>
  </head>
  <body>
"
+ md.Transform(markdowntext) + 
@"
  </body>
</html>
";
        }

    }

    class InvertedBooleanToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is bool && (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    namespace Commands
    {
        public static class Debt
        {
            public static readonly RoutedUICommand Delete = new RoutedUICommand
                    (
                            "Delete Debt",
                            "Delete Debt",
                            typeof(Debt),
                            new InputGestureCollection()
                                {
                                        new KeyGesture(Key.D, ModifierKeys.Alt)
                                }
                    );

            public static readonly RoutedUICommand Add = new RoutedUICommand
                    (
                            "Delete Add",
                            "Delete Add",
                            typeof(Debt),
                            new InputGestureCollection()
                //{
                //        new KeyGesture(Key.F4, ModifierKeys.Alt)
                //}
                    );

            public static readonly RoutedUICommand Edit = new RoutedUICommand
                    (
                            "Delete Edit",
                            "Delete Edit",
                            typeof(Debt),
                            new InputGestureCollection()
                //{
                //        new KeyGesture(Key.F4, ModifierKeys.Alt)
                //}
                    );

        }

        public static class App
        {
            public static readonly RoutedUICommand ForgetServer = new RoutedUICommand();
        }
    }
}

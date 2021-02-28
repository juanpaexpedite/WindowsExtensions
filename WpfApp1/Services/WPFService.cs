using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WindowsExtensions.Views;

namespace WPFCore
{
    public class WPFService
    {
        private static bool designmode = true;

        public static bool IsDesignMode()
        {
            return designmode == true;
        }

        public static void SetDesignMode(bool mode)
        {
            designmode = mode;
        }

        private static Window owner;

        public static void SetDialogsOwner(Window mainwindow)
        {
            owner = mainwindow;
        }

        public static void ShowDialogBox<T>() where T : Window
        {
            var dlg = Activator.CreateInstance<T>();
            dlg.Owner = owner;

            dlg.ShowDialog();
        }

        #region HyperLink Navigation Attached Property

        public static string Navigate = String.Empty;

        public static bool GetNavigate(DependencyObject obj)
        {
            return (bool)obj.GetValue(NavigateProperty);
        }

        public static void SetNavigate(DependencyObject obj, bool value)
        {
            obj.SetValue(NavigateProperty, value);
        }
        public static readonly DependencyProperty NavigateProperty =
            DependencyProperty.RegisterAttached(nameof(Navigate), typeof(bool), typeof(WPFService), new UIPropertyMetadata(false, OnNavigateChanged));

        private static void OnNavigateChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var hyperlink = sender as Hyperlink;

            if (args.NewValue is bool bargs)
                hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
            else
                hyperlink.RequestNavigate -= Hyperlink_RequestNavigate;
        }

        private static void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        #endregion
    }
}

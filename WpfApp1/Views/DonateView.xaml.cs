using System;
using System.Collections.Generic;
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
using WindowsExtensions.Services;

namespace WindowsExtensions.Views
{
    /// <summary>
    /// Interaction logic for DonateView.xaml
    /// </summary>
    public partial class DonateView : Window
    {
        public DonateView()
        {
            InitializeComponent();
            ReadDogeAmount();
        }

        private void CopyWallet_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText("D6VB4FVDQa4B6EdXZTXTNpx5LjDD2eGrFb");
            LabelCopied.Visibility = Visibility.Visible;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void ReadDogeAmount()
        {
            string amount = await HttpService.ReadDogeAmount();
            DogeAmountInformation.Text = $"If you find this useful you can donate doges to my wallet with balance: {amount}";
        }
    }
}

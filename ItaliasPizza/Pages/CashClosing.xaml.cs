using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliasPizza.Pages
{
    /// <summary>
    /// Interaction logic for CashClosing.xaml
    /// </summary>
    public partial class CashClosing : Page
    {
        private CashClosingDetails cashClosingDetails;
        public CashClosing()
        {
            InitializeComponent();
            cashClosingDetails = CashReconciliationOperations.GetCashClosingDetails();
            ShowCashInfo();
        }

        private void ShowCashInfo()
        {
            LblPreviousDayCash.Content += cashClosingDetails.PreviousDayCash.ToString();
            LblTotalSalesCash.Content += cashClosingDetails.TotalSalesCash.ToString();
            LblTotalSpentCash.Content += cashClosingDetails.TotalSpentCash.ToString();
            LblTotalCash.Content += ((cashClosingDetails.PreviousDayCash + cashClosingDetails.TotalSalesCash)-(cashClosingDetails.TotalSpentCash)).ToString();
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}

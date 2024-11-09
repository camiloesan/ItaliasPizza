using ItaliasPizza.DataAccessLayer;
using System;
using System.Collections;
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
    /// Interaction logic for SupplierOrders.xaml
    /// </summary>
    public partial class SupplierOrders : Page
    {
        public SupplierOrders()
        {
            InitializeComponent();
            DtgSupplierOrders.ItemsSource = SupplierOrderOperations.GetSupplierOrderDetails();
        }

        private void Btn_Employees(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Supplies(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Orders(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Products(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Suppliers(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Reports(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_SupplierOrders(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_Edit(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_RegisterNewSupplier(object sender, RoutedEventArgs e)
        {

        }
    }
}

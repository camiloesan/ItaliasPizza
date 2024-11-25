using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Pages.InventoryReport;
using ItaliasPizza.Utils;
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
        private const string SUPPLIER_NAME_ATTRIBUTE = "Proveedor";
        private const string SUPPLIES_ATTRIBUTE = "Insumos";
        private const string ORDER_DATE_ATTRIBUTE = "Fecha de orden";
        private const string EXPECTED_DATE_ATTRIBUTE = "Fecha estimada";
        private const string ARRIVAL_DATE_ATTRIBUTE = "Fecha de entrega";
        private const string STATUS_ATTRIBUTE = "Estado";
        private List<SupplierOrderDetails> supplierOrderDetails = SupplierOrderOperations.GetSupplierOrderDetails();
        public SupplierOrders()
        {
            InitializeComponent();
            DtgSupplierOrders.ItemsSource = supplierOrderDetails;
            FillCbFilter();
        }

        private void FillCbFilter()
        {
            List<string> filters = new List<string>{
                SUPPLIER_NAME_ATTRIBUTE,
                SUPPLIES_ATTRIBUTE,
                ORDER_DATE_ATTRIBUTE,
                EXPECTED_DATE_ATTRIBUTE,
                ARRIVAL_DATE_ATTRIBUTE,
                STATUS_ATTRIBUTE
            };

            CbFilter.ItemsSource = filters;
            CbFilter.SelectedIndex = 0;
        }

        private void Btn_Employees(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Employees();
        }

        private void Btn_Supplies(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Inventory();
        }

        private void Btn_Orders(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Suppliers(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SuppliersList();
        }

        private void Btn_Reports(object sender, RoutedEventArgs e)
        {
			Application.Current.MainWindow.Content = new FinishInventoryReport();
		}

        private void Btn_Products(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Products();
        }
        private void Btn_SupplierOrders(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SupplierOrders();
        }

        private void Btn_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Login();
        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearcher.Text.ToLower();
            List<SupplierOrderDetails> filteredList = new List<SupplierOrderDetails>();

            switch (CbFilter.Text)
            {
                case SUPPLIER_NAME_ATTRIBUTE:
                    filteredList = supplierOrderDetails
                        .Where(s => s.SupplierName.ToLower().Contains(searchText))
                        .ToList();
                    break;

                case SUPPLIES_ATTRIBUTE:
                    filteredList = supplierOrderDetails
                        .Where(s => s.Supplies.ToLower().Contains(searchText))
                        .ToList();
                    break;

                case ORDER_DATE_ATTRIBUTE:
                    filteredList = supplierOrderDetails
                        .Where(s => s.OrderDate.Contains(searchText))
                        .ToList();
                    break;

                case EXPECTED_DATE_ATTRIBUTE:
                    filteredList = supplierOrderDetails
                        .Where(s => s.ExpectedDate.Contains(searchText))
                        .ToList();
                    break;
                
                case ARRIVAL_DATE_ATTRIBUTE:
                    filteredList = supplierOrderDetails
                        .Where(s => s.ArrivalDate.Contains(searchText))
                        .ToList();
                    break;
                        
                case STATUS_ATTRIBUTE:
                    filteredList = supplierOrderDetails
                        .Where(s => s.Status.Contains(searchText))
                        .ToList();
                    break;
            }

            if (supplierOrderDetails != filteredList && filteredList.Count > 0)
            {
                DtgSupplierOrders.ItemsSource = filteredList;
            }
            else
            {
                MessageBox.Show("No existen coincidencias");
                DtgSupplierOrders.ItemsSource = supplierOrderDetails;
            }
        }
        private void Btn_Edit(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button.DataContext is SupplierOrderDetails supplierOrderDetails)
            {
                Application.Current.MainWindow.Content = new SupplierOrderModification(supplierOrderDetails);
            }
        }

        private void Btn_RegisterNewSupplierOrder(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SupplierOrderRegister();
        }
    }
}

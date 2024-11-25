using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Pages.InventoryReport;
using ItaliasPizza.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizza.Pages
{
    public partial class Inventory : Page
    {
        private readonly List<SupplyDetails> productDetailsList = SupplyOperations.GetSupplyDetailsXes();

        public Inventory()
        {
            InitializeComponent();
            ShowInventory();
            FillCbFilter();
        }

        private void ShowInventory()
        {
            DtgInventory.ItemsSource = productDetailsList;
        }

        private const string NAME = "Nombre";
        private const string QUANTITY = "Cantidad";
        private const string CATEGORY = "Categoría";
        private const string STATUS = "Estado";

        private void FillCbFilter()
        {
            List<string> filters = new List<string>
            {
                NAME,
                QUANTITY,
                CATEGORY,
                STATUS
            };

            CbFilter.ItemsSource = filters;
            CbFilter.SelectedIndex = 0;
        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {
            string filter = CbFilter.SelectedItem.ToString();
            string search = TxtSearcher.Text.ToLower();

            List<SupplyDetails> filteredList = new List<SupplyDetails>();

            switch (filter)
            {
                case NAME:
                    filteredList = productDetailsList.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToList();
                    break;
                case QUANTITY:
                    filteredList = productDetailsList.Where(p => p.Quantity.ToLower().Contains(search.ToLower())).ToList();
                    break;
                case CATEGORY:
                    filteredList = productDetailsList.Where(p => p.Category.ToLower().Contains(search.ToLower())).ToList();
                    break;
                case STATUS:
                    filteredList = productDetailsList.Where(p => p.Status.ToLower().Contains(search.ToLower())).ToList();
                    break;
            }

            DtgInventory.ItemsSource = filteredList;
        }

        private void Btn_InventoryReport(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new StartInventoryReport();
		}

        private void Btn_SupplyRegister(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SupplyRegister();
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

        private void EditSupply_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button.DataContext is SupplyDetails selectedSupply)
            {
                Application.Current.MainWindow.Content = new SupplyModification(selectedSupply);
            }
        }
    }
}

using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SuppliersList : Page
    {
        private const string SUPPLIER_NAME_ATTRIBUTE = "Nombre";
        private const string SUPPLIER_CATEGORIES_ATTRIBUTE = "Categorías";
        private const string SUPPLIER_PHONE_ATTRIBUTE = "Teléfono";
        private List<SupplierDetails> supplierDetailsList = SupplierOperations.GetAllSuppliersWithCategories();
        public SuppliersList()
        {
            InitializeComponent();
            ShowSuppliers();
            FillCbFilter();
        }

        private void FillCbFilter()
        {
            List<string> filters = new List<string>();
            filters.Add(SUPPLIER_NAME_ATTRIBUTE);
            filters.Add(SUPPLIER_CATEGORIES_ATTRIBUTE);
            filters.Add(SUPPLIER_PHONE_ATTRIBUTE);

            CbFilter.ItemsSource = filters;
            CbFilter.SelectedIndex = 0;
        }

        private void ShowSuppliers()
        {
            DtgSuppliers.ItemsSource = supplierDetailsList;
        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearcher.Text.ToLower();
            List<SupplierDetails> filteredList = new List<SupplierDetails>();

            switch (CbFilter.Text)
            {
                case SUPPLIER_NAME_ATTRIBUTE:
                    filteredList = supplierDetailsList
                        .Where(s => s.Name.ToLower().Contains(searchText))
                        .ToList();
                    break;

                case SUPPLIER_CATEGORIES_ATTRIBUTE:
                    filteredList = supplierDetailsList
                        .Where(s => s.Categories.ToLower().Contains(searchText))
                        .ToList();
                    break;

                case SUPPLIER_PHONE_ATTRIBUTE:
                    filteredList = supplierDetailsList
                        .Where(s => s.Phone.Contains(searchText))
                        .ToList();
                    break;
            }

            if (supplierDetailsList != filteredList && filteredList.Count > 0)
            {
                DtgSuppliers.ItemsSource = filteredList;
            } else
            {
                MessageBox.Show("No existen coincidencias");
                DtgSuppliers.ItemsSource = supplierDetailsList;
            }
        }
        private void Btn_Edit(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null && button.CommandParameter != null)
            {
                //var supplierId = button.CommandParameter;
                //SupplierDetails.IdSupplier = supplierId;
                //Application.Current.MainWindow.Content = new SupplierModification();
            }
        }

        private void Btn_RegisterNewSupplier(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SupplierRegister();
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

        }

        private void Btn_Reports(object sender, RoutedEventArgs e)
        {

        }
    }
}

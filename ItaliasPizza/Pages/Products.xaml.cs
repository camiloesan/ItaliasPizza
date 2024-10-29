using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizza.Pages
{
    public partial class Products : Page
    {
        private readonly List<ProductDetails> productDetailsList = ProductOperations.GetProductDetails();

        public Products()
        {
            InitializeComponent();
            ShowProducts();
            FillCbFilter();
        }

        private void ShowProducts()
        {
            DtgProducts.ItemsSource = productDetailsList;
        }

        private const string NAME = "Nombre";
        private const string PRICE = "Precio";
        private const string SIZE = "Tamaño";
        private const string TYPE = "Tipo";
        private const string STATUS = "Estado";
        private void FillCbFilter()
        {
            List<string> filters = new List<string>
            {
                NAME,
                PRICE,
                SIZE,
                TYPE,
                STATUS
            };

            CbFilter.ItemsSource = filters;
            CbFilter.SelectedIndex = 0;
        }

        private void Btn_Products(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Employees(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Employees();
        }

        private void Btn_Supplies(object sender, RoutedEventArgs e)
        {

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

        private void Btn_Search(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearcher.Text.ToLower();
            List<ProductDetails> filteredList = new List<ProductDetails>();

            switch (CbFilter.Text)
            {
                case NAME:
                    filteredList = productDetailsList
                        .Where(s => s.Name.ToLower().Contains(searchText))
                        .ToList();
                    break;
                case PRICE:
                    filteredList = productDetailsList
                        .Where(s => s.Price.ToString().Contains(searchText))
                        .ToList();
                    break;
                case SIZE:
                    filteredList = productDetailsList
                        .Where(s => s.Size.ToLower().Contains(searchText))
                        .ToList();
                    break;
                case TYPE:
                    filteredList = productDetailsList
                        .Where(s => s.Type.Contains(searchText))
                        .ToList();
                    break;
                case STATUS:
                    filteredList = productDetailsList
                        .Where(s => s.Status.Contains(searchText))
                        .ToList();
                    break;
            }

            if (productDetailsList != filteredList && filteredList.Count > 0)
            {
                DtgProducts.ItemsSource = filteredList;
            }
            else
            {
                MessageBox.Show("No existen coincidencias");
                DtgProducts.ItemsSource = productDetailsList;
            }
        }

        private void Btn_RegisterProduct(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new ProductRegister();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button.DataContext is ProductDetails selectedProduct)
            {
                Application.Current.MainWindow.Content = new ProductModification(selectedProduct.IdProduct);
            }
        }
    }
}

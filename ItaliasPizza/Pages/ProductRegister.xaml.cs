using Database;
using ItaliasPizza.DataAccessLayer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItaliasPizza.Pages
{
    public partial class ProductRegister : Page
    {
        public ProductRegister()
        {
            InitializeComponent();
            CbProductType.ItemsSource = ProductTypeOperations.GetProductTypes();
        }

        private bool AreFieldsFilled()
        {
            if (string.IsNullOrEmpty(TxtName.Text.Trim()) 
                || string.IsNullOrEmpty(TxtPrice.Text.Trim()) 
                || string.IsNullOrEmpty(CbSize.Text.Trim()) 
                || CbSize.SelectedIndex == 0
                || CbProductType.SelectedIndex == -1 
                || CbAvailability.SelectedIndex == 0)
            {
                return false;
            }
            return true;
        }

        private void SaveProduct()
        {
            bool status = false;
            if (CbAvailability.SelectedIndex == 1)
            {
                status = true;
            }
            else if (CbAvailability.SelectedIndex == 2)
            {
                status = false;
            }

            var product = new Product
            {
                Name = TxtName.Text.Trim(),
                Size = CbSize.Text.Trim(),
                Price = decimal.Parse(TxtPrice.Text.Trim()),
                IdType = CbProductType.SelectedIndex + 1,
                Status = status,
            };

            int result = ProductOperations.SaveProduct(product);

            if (result > 0)
            {
                MessageBox.Show("El producto se ha añadido exitosamente");
            }
            else
            {
                MessageBox.Show("Error al guardar el producto, inténtalo de nuevo más tarde");
            }
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            HighlightInvalidFields();
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Por favor, llena todos los campos");
                return;
            }

            if (ProductOperations.IsProductNameDuplicated(TxtName.Text.Trim()))
            {
                MessageBox.Show("Ya existe un producto con ese nombre, intente uno nuevo");
                return;
            }

            SaveProduct();
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Products();
        }

        private void HighlightInvalidFields()
        {
            string name = TxtName.Text.Trim();
            string price = TxtPrice.Text.Trim();
            ResetTextFormBorders();

            if (string.IsNullOrEmpty(name))
            {
                TxtName.BorderBrush = Brushes.Red;
                TxtName.BorderThickness = new Thickness(2);
            }

            if (string.IsNullOrEmpty(price) || ProductOperations.IsProductNameDuplicated(name))
            {
                TxtPrice.BorderBrush = Brushes.Red;
                TxtPrice.BorderThickness = new Thickness(2);
            }
        }

        private void ResetTextFormBorders()
        {
            TxtName.BorderBrush = Brushes.Black;
            TxtName.BorderThickness = new Thickness(1);
            TxtPrice.BorderBrush = Brushes.Black;
            TxtPrice.BorderThickness = new Thickness(1);
        }
    }
}

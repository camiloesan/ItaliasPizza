using Database;
using ItaliasPizza.DataAccessLayer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItaliasPizza.Pages
{
    public partial class ProductModification : Page
    {
        Guid productId = Guid.Empty;
        readonly Product product;

        public ProductModification(Guid productId)
        {
            InitializeComponent();
            CbProductType.ItemsSource = ProductTypeOperations.GetProductTypes();
            this.productId = productId;
            product = GetProductInfo(productId);
            LoadProductInfo(product);
        }

        private Product GetProductInfo(Guid productId)
        {
            return ProductOperations.GetProductById(productId);
        }

        private void LoadProductInfo(Product product)
        {
            TxtName.Text = product.Name;
            TxtPrice.Text = product.Price.ToString();
            CbSize.Text = product.Size;
            CbProductType.SelectedIndex = product.IdType - 1;
            CbAvailability.SelectedIndex = product.Status ? 1 : 2;
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            HighlightInvalidFields();
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Por favor, llena todos los campos");
                return;
            }
            else if (!TxtName.Text.Equals(product.Name) && ProductOperations.IsProductNameDuplicated(TxtName.Text))
            {
                MessageBox.Show("El nombre del producto ya existe, por favor elige otro");
                return;
            }
            else
            {
                UpdateProduct();
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

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

        private void UpdateProduct()
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
                IdProduct = productId,
                Name = TxtName.Text.Trim(),
                Size = CbSize.Text.Trim(),
                Price = decimal.Parse(TxtPrice.Text.Trim()),
                IdType = CbProductType.SelectedIndex + 1,
                Status = status,
            };

            int result = ProductOperations.UpdateProduct(product);

            if (result > 0)
            {
                MessageBox.Show("El producto se ha actualizado exitosamente");
            }
            else
            {
                MessageBox.Show("Error al actualizar el producto, inténtalo de nuevo más tarde");
            }
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

            if (string.IsNullOrEmpty(price) || (!name.Equals(product.Name) && ProductOperations.IsProductNameDuplicated(name)))
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

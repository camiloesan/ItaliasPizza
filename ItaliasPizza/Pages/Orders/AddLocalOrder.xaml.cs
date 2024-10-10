using Database;
using ItaliasPizza.DataAccessLayer;
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

namespace ItaliasPizza.Pages.Orders
{
	/// <summary>
	/// Interaction logic for AddLocalOrder.xaml
	/// </summary>
	public partial class AddLocalOrder : Page
	{
		public AddLocalOrder()
		{
			InitializeComponent();
			/*var items = new ObservableCollection<Product>()
			{
				new Product {Name = "Pizza", Price = 100},
			};
			DtgActiveProducts.ItemsSource = items;*/
			FillProductList();
		}

		private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Product selectedProduct = (Product)button.DataContext;
			AddProductToOrder(selectedProduct);
		}

		private void RemoveProduct_Click(object sender, RoutedEventArgs e)
		{

		}

		private void BtnFinishOrder(object sender, MouseButtonEventArgs e)
		{
			
        }

		private void BtnCancelOrder(object sender, MouseButtonEventArgs e)
		{
			
		}

		private void ImgReturn(object sender, MouseButtonEventArgs e)
		{
			
		}

		private void AddProductToOrder(Product selectedProduct)
		{
			if (EnoughSupplies(selectedProduct))
			{
				var selectedProducts = new List<Product>(); //TODO: Que se pueda agregar más de un producto
				selectedProducts.Add(selectedProduct);
				FillSelectedProductsList(selectedProducts);
			}
			else
			{
				MessageBox.Show("No hay suficientes insumos para más de este producto.");
			}
		}

		private bool EnoughSupplies(Product selectedProduct)
		{
			return ProductOperations.GetPreparableProductQuantity(selectedProduct) > 0;
		}

		private void FillSelectedProductsList(List<Product> selectedProducts)
		{
			var items = new ObservableCollection<Product>(selectedProducts);
			DtgSelectedProducts.ItemsSource = items;
		}

		private void FillProductList()
		{
			// Fill product list
			var activeProducts = ProductOperations.GetActiveProducts();

			var items = new ObservableCollection<Product>(activeProducts);
			DtgActiveProducts.ItemsSource = items;
		}
	}
}

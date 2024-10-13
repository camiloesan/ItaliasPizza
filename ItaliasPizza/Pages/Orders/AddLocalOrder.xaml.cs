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

namespace ItaliasPizza.Pages.Orders
{
	/// <summary>
	/// Interaction logic for AddLocalOrder.xaml
	/// </summary>
	public partial class AddLocalOrder : Page
	{
		private List<LocalOrderProductDetails> localOrderProductsDetails = new List<LocalOrderProductDetails>();
		private Product selectedProduct;

		public AddLocalOrder()
		{
			InitializeComponent();
			FillProductList();
		}

		private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			selectedProduct = (Product)button.DataContext;

			if (!IsProductAlreadySelected(selectedProduct))
			{
				AddProductToOrder(selectedProduct);
			}
			else 
			{
				MessageBox.Show("El producto ya ha sido seleccionado.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button) sender;
			var selectedProductDetails = (LocalOrderProductDetails)button.DataContext;

			localOrderProductsDetails.Remove(selectedProductDetails);
			FillSelectedProductsList();
		}

		private void BtnFinishOrder_Click(object sender, MouseButtonEventArgs e)
		{
			if (localOrderProductsDetails.Count == 0)
			{
				MessageBox.Show("Por favor, seleccione al menos un producto", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (CmbTableNumber.SelectedIndex == -1)
			{
				MessageBox.Show("Por favor, seleccione una mesa", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			MessageBoxResult messageBoxResult = MessageBox.Show("¿Está seguro de que desea finalizar la orden?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.No || messageBoxResult == MessageBoxResult.None)
			{
				return;
			}

			int registeredLocalOrderResult = 0;
			OrderStatus pendingStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");

			List<LocalOrderProduct> localOrderProducts = new List<LocalOrderProduct>();

			LocalOrder localOrder = new LocalOrder
			{
				IdLocalOrder = Guid.NewGuid(),
				Waiter = SessionDetails.IdEmployee,
				IdOrderStatus = pendingStatus.IdOrderStatus,
				Table = int.Parse(CmbTableNumber.Text),
				Date = DateTime.Now,
				Total = localOrderProductsDetails.Sum(x => x.SubTotal)
			};

			foreach (var item in localOrderProductsDetails)
			{
				localOrderProducts.Add(new LocalOrderProduct
				{
					IdLocalOrderProduct = Guid.NewGuid(),
					IdLocalOrder = localOrder.IdLocalOrder,
					IdProduct = item.IdProduct,
					Quantity = item.Quantity,
					SubTotal = item.SubTotal
				});
			}

			registeredLocalOrderResult = LocalOrderOperations.SaveLocalOrderWithProducts(localOrder, localOrderProducts);

			if (registeredLocalOrderResult > 0)
			{
				MessageBox.Show("Orden registrada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
				Application.Current.MainWindow.Content = new ViewOrders();
			}
			else
			{
				MessageBox.Show("Ocurrió un error al registrar la orden.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			
		}

		private void BtnCancelOrder_Click(object sender, MouseButtonEventArgs e)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Está seguro de que desea cancelar el registro de la orden?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.No || messageBoxResult == MessageBoxResult.None)
			{
				return;
			}

			Application.Current.MainWindow.Content = new ViewOrders();
		}

		private void AddProductToOrder(Product selectedProduct)
		{
			if (!EnoughSupplies(selectedProduct))
			{
				MessageBox.Show("No hay suficientes insumos para más de este producto.");
				return;
			}

			BtnCancelOrder.IsEnabled = false;
			BtnFinishOrder.IsEnabled = false;
			ProductAmountForm.Visibility = Visibility.Visible;
		}

		private bool IsProductAlreadySelected(Product selectedProduct)
		{
			bool isAlreadySelected = false;

			var idSelelectedProduct = selectedProduct.IdProduct;

			foreach (var item in localOrderProductsDetails)
			{
				if (item.IdProduct == idSelelectedProduct)
				{
					isAlreadySelected = true;
					break;
				}
			}

			return isAlreadySelected;
		}

		private void BtnSaveProductAmount_Click(object sender, RoutedEventArgs e)
		{
			var productAmount = TxtProductAmount.Text;

			if (string.IsNullOrEmpty(productAmount))
			{
				MessageBox.Show("Por favor, ingrese la cantidad de productos a agregar.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			} else if (int.Parse(productAmount) == 0)
			{
				MessageBox.Show("Por favor, ingrese una cantidad válida.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			} else if (!IsInputNumber(productAmount))
			{
				MessageBox.Show("Por favor, ingrese una cantidad válida.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			} else 
			{
				LocalOrderProductDetails newLocalOrderProduct = new LocalOrderProductDetails
				{
					IdProduct = selectedProduct.IdProduct,
					Name = selectedProduct.Name,
					Quantity = int.Parse(productAmount), // TODO: Que se pueda agregar más de un producto con un cuadro de entrada
					SubTotal = selectedProduct.Price * int.Parse(productAmount)
				};

				localOrderProductsDetails.Add(newLocalOrderProduct);
				FillSelectedProductsList();
				ResetAmountForm();
			}
		}

		private void BtnCancelProductAmount_Click(object sender, RoutedEventArgs e)
		{
			ResetAmountForm();
		}

		private void ResetAmountForm()
		{
			ProductAmountForm.Visibility = Visibility.Hidden;
			BtnCancelOrder.IsEnabled = true;
			BtnFinishOrder.IsEnabled = true;
			TxtProductAmount.Text = string.Empty;
		}

		private bool EnoughSupplies(Product selectedProduct)
		{
			return ProductOperations.GetPreparableProductQuantity(selectedProduct) > 0;
		}

		private void FillSelectedProductsList()
		{
			var items = new ObservableCollection<LocalOrderProductDetails>(localOrderProductsDetails);
			DtgSelectedProducts.ItemsSource = items;

			var total = localOrderProductsDetails.Sum(x => x.SubTotal);

			LblTotal.Content = total;
		}

		private void FillProductList()
		{
			var activeProducts = ProductOperations.GetActiveProducts();

			var items = new ObservableCollection<Product>(activeProducts);
			DtgActiveProducts.ItemsSource = items;
		}

		private void TxtProductAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsInputNumber(e.Text);
		}

		private bool IsInputNumber(string input)
		{
			string pattern = @"^\d+$";
			return System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
		}
	}
}

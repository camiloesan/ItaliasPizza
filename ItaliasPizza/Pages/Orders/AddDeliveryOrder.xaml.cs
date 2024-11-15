using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Pages.Clients;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ItaliasPizza.Pages.Orders
{
	/// <summary>
	/// Interaction logic for AddDeliveryOrder.xaml
	/// </summary>
	public partial class AddDeliveryOrder : Page
	{
		private Product selectedProduct;
		private List<DeliveryOrderProductDetails> deliveryOrderProductsDetails = new List<DeliveryOrderProductDetails>();

		private Client foundClient = new Client();
		private Client selectedClient;
		private Address selectedAddress;
		
		public AddDeliveryOrder()
		{
			InitializeComponent();
			FillProductList();
			BtnChangeAddress.IsEnabled = false;
		}

		private void FillProductList()
		{
			DtgActiveProducts.ItemsSource = new ObservableCollection<Product>(ProductOperations.GetActiveProducts());
		}

		private void FillSelectedProductsList()
		{
			DtgSelectedProducts.ItemsSource = new ObservableCollection<DeliveryOrderProductDetails>(deliveryOrderProductsDetails);
			var total = deliveryOrderProductsDetails.Sum(x => x.SubTotal);
			LblTotal.Content = total;
		}

		private void DisableMainForm()
		{
			DtgActiveProducts.IsEnabled = false;
			DtgSelectedProducts.IsEnabled = false;
			BtnSearchClient.IsEnabled = false;
			BtnChangeAddress.IsEnabled = false;
			BtnCancelOrder.IsEnabled = false;
			BtnSaveOrder.IsEnabled = false;
		}

		private void EnableMainForm()
		{
			DtgActiveProducts.IsEnabled = true;
			DtgSelectedProducts.IsEnabled = true;
			BtnSearchClient.IsEnabled = true;
			if (selectedAddress != null) {
				BtnChangeAddress.IsEnabled = true;
			}
			BtnCancelOrder.IsEnabled = true;
			BtnSaveOrder.IsEnabled = true;
		}

		private void ResetAmountForm()
		{
			EnableMainForm();
			BrdProductAmountForm.Visibility = Visibility.Hidden;
			TxtProductAmount.Text = string.Empty;
		}

		private void ShowFoundClientInformation(Client foundClient)
		{
			BrdFoundClientPopUp.Visibility = Visibility.Visible;

			LblFoundClientName.Content = foundClient.FirstName;
			LblFoundClientLastName.Content = foundClient.LastName;
			LblFoundClientPhone.Content = foundClient.Phone;

			DtgClientAdresses.ItemsSource = new ObservableCollection<Address>(AddressOperations.GetClientAddresses(foundClient));
		}

		private bool IsProductAlreadySelected(Product selectedProduct)
		{
			bool isAlreadySelected = false;

			var idSelectedProduct = selectedProduct.IdProduct;

			foreach (var item in deliveryOrderProductsDetails)
			{
				if (item.IdProduct == idSelectedProduct)
				{
					isAlreadySelected = true;
					break;
				}
			}

			return isAlreadySelected;
		}

		private bool EnoughSupplies(Product selectedProduct)
		{
			bool areEnoughSupplies = true;
			if (string.IsNullOrEmpty(TxtProductAmount.Text))
			{
				areEnoughSupplies = ProductOperations.GetPreparableProductQuantity(selectedProduct) > 0;
			}
			else
			{
				areEnoughSupplies = ProductOperations.GetPreparableProductQuantity(selectedProduct) > int.Parse(TxtProductAmount.Text);
			}
			return areEnoughSupplies;
		}

		private void AddProductToOrder(Product selectedProduct)
		{
			if (ProductOperations.GetPreparableProductQuantity(selectedProduct) == -0)
			{
				MessageBox.Show("Ha ocurrido un error al calcular la cantidad preparable de este producto.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (!EnoughSupplies(selectedProduct))
			{
				MessageBox.Show("No hay suficientes insumos para más de este producto.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			DisableMainForm();
			BrdProductAmountForm.Visibility = Visibility.Visible;
		}

		private int RegisterDeliveryOrder()
		{
			OrderStatus pendingStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");

			List<DeliveryOrderProduct> deliveryOrderProducts = new List<DeliveryOrderProduct>();

			DeliveryOrder deliveryOrder = new DeliveryOrder()
			{
				IdDeliveryOrder = Guid.NewGuid(),
				IdClient = selectedClient.IdClient,
				//IdAddress = selectedAddress.IdAddress, // TODO: Add address to delivery order model & database relation
				IdOrderStatus = pendingStatus.IdOrderStatus,
				Total = deliveryOrderProductsDetails.Sum(x => x.SubTotal),
				Date = DateTime.Now,
				DeliveryDriver = Guid.Empty
			};

			foreach (var item in deliveryOrderProductsDetails)
			{
				deliveryOrderProducts.Add(new DeliveryOrderProduct()
				{
					IdDeliveryOrderProduct = Guid.NewGuid(),
					IdDeliveryOrder = deliveryOrder.IdDeliveryOrder,
					IdProduct = item.IdProduct,
					Quantity = item.Quantity,
					SubTotal = item.SubTotal
				});
			}

			return DeliveryOrderOperations.SaveDeliveryOrderWithProducts(deliveryOrder, deliveryOrderProducts);
		}


		private void BtnSearchClient_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(TxtSearchNumber.Text))
			{
				MessageBox.Show("Ingrese un número de teléfono.","Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			} else if (TxtSearchNumber.Text.Length < 10)
			{
				MessageBox.Show("Ingrese un número de teléfono válido.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			foundClient = ClientOperations.GetClientByPhoneNumber(TxtSearchNumber.Text);

			if (foundClient == null)
			{
				MessageBoxResult result = MessageBox.Show("El número de teléfono no está registrado. ¿Desea registrar un nuevo cliente?", "Cliente no encontrado", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (result == MessageBoxResult.Yes)
				{
					Application.Current.MainWindow.Content = new RegisterClient(TxtSearchNumber.Text);
				}
			}
			else
			{
				DisableMainForm();
				ShowFoundClientInformation(foundClient);
			}
		}

		private void BtnChangeAddress_Click(object sender, RoutedEventArgs e)
		{
			ShowFoundClientInformation(selectedClient);
			DisableMainForm();
			LblSelectedClientStreet.Content = selectedAddress.Street;
			LblSelectedClientAddressNumber.Content = selectedAddress.Number;
			LblSelectedClientColony.Content = selectedAddress.Colony;
		}


		private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			selectedProduct = (Product)button.DataContext;

			if (IsProductAlreadySelected(selectedProduct))
			{
				MessageBox.Show("El producto ya ha sido seleccionado.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			AddProductToOrder(selectedProduct);

		}

		private void BtnSaveProductAmount_Click(object sender, RoutedEventArgs e)
		{
			var productAmount = TxtProductAmount.Text;
			var preparableProductQuantity = ProductOperations.GetPreparableProductQuantity(selectedProduct);

			if (string.IsNullOrEmpty(productAmount))
			{
				MessageBox.Show("Por favor, ingrese la cantidad de productos a agregar.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			else if (int.Parse(productAmount) == 0)
			{
				MessageBox.Show("Por favor, ingrese una cantidad válida.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			else if (!IsInputNumber(productAmount))
			{
				MessageBox.Show("Por favor, ingrese una cantidad válida.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			else if (!EnoughSupplies(selectedProduct))
			{
				MessageBox.Show("No hay suficientes insumos para más de este producto. \nCantidad preparable: " + preparableProductQuantity, "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			else
			{

				DeliveryOrderProductDetails newDeliveryOrderProduct = new DeliveryOrderProductDetails
				{
					IdProduct = selectedProduct.IdProduct,
					Name = selectedProduct.Name,
					Quantity = int.Parse(productAmount),
					SubTotal = selectedProduct.Price * int.Parse(productAmount)
				};

				deliveryOrderProductsDetails.Add(newDeliveryOrderProduct);
				FillSelectedProductsList();
				ResetAmountForm();
			}
		}

		private void BtnCancelProductAmount_Click(object sender, RoutedEventArgs e)
		{
			ResetAmountForm();
		}

		private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			var selectedProductDetails = (DeliveryOrderProductDetails)button.DataContext;

			deliveryOrderProductsDetails.Remove(selectedProductDetails);
			FillSelectedProductsList();
		}

		private void BtnCancelOrder_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("¿Desea cancelar la orden?", "Cancelar orden", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				// Application.Current.MainWindow.Content = new OrdersMenu(); REGRESAR AL MENU PRINCIPAL DEL CAJERO
			}

			return;
		}

		private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
		{
			if (deliveryOrderProductsDetails.Count == 0)
			{
				MessageBox.Show("Agregue al menos un producto a la orden.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (selectedClient == null)
			{
				MessageBox.Show("Seleccione un cliente para la orden.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			MessageBoxResult messageBoxResult = MessageBox.Show("¿Desea finalizar la orden?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (messageBoxResult == MessageBoxResult.No || messageBoxResult == MessageBoxResult.None)
			{
				return;
			}

			

			int registerDeliveryOrderResult = RegisterDeliveryOrder();

			if (registerDeliveryOrderResult == -1 || registerDeliveryOrderResult == 0)
			{
				MessageBox.Show("Ocurrió un error al registrar la orden.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			
			MessageBox.Show("Orden registrada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

		}

		private void BtnSelectAddress_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Address address = (Address)button.DataContext;

			LblSelectedClientStreet.Content = address.Street;
			LblSelectedClientAddressNumber.Content = address.Number;
			LblSelectedClientColony.Content = address.Colony;

			selectedAddress = address;
		}

		private void BtnCancelSelectClient_Click(object sender, RoutedEventArgs e)
		{
			BrdFoundClientPopUp.Visibility = Visibility.Hidden;
			foundClient = null;
			selectedAddress = null;
			EnableMainForm();
		}

		private void BtnAddClientAddress_Click(object sender, RoutedEventArgs e)
		{	
			MessageBoxResult result = MessageBox.Show("¿Agregar una dirección para el cliente?", "Agregar dirección", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				BrdFoundClientPopUp.Visibility = Visibility.Hidden;
				BrdAddClientAddress.Visibility = Visibility.Visible;
			}
		}

		private void BtnConfirmClient_Click(object sender, RoutedEventArgs e)
		{
			if (selectedAddress == null)
			{
				MessageBox.Show("Seleccione una dirección para el cliente.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			selectedClient = foundClient;

			BrdFoundClientPopUp.Visibility = Visibility.Hidden;
			EnableMainForm();

			LblClientName.Content = selectedClient.FirstName;
			LblClientLastName.Content = selectedClient.LastName;
			LblClientPhone.Content = selectedClient.Phone;

			LblClientStreet.Content = selectedAddress.Street;
			LblClientAddressNumber.Content = selectedAddress.Number;
			LblClientColony.Content = selectedAddress.Colony;
			LblClientPostalCode.Content = selectedAddress.PostalCode;
			LblClientReferences.Content = selectedAddress.Reference;
		}

		private bool IsInputNumber(string input)
		{
			string pattern = @"^\d+$";
			return System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
		}

		private bool IsPhoneNumberValid(string phone)
		{
			return phone.Length == 10;
		}

		private void TxtNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsInputNumber(e.Text);
		}

		// -- CU19 - ADD CLIENT ADDRESS --

		private bool AreFieldsFilled()
		{
			return !string.IsNullOrEmpty(TxtStreet.Text)
				&& !string.IsNullOrEmpty(TxtNumber.Text)
				&& !string.IsNullOrEmpty(TxtColony.Text)
				&& !string.IsNullOrEmpty(TxtPostalCode.Text);
		}
	
		private void SetCreatedAddressAsSelectedAddress(Address createdAddress)
		{
			selectedAddress = createdAddress;
		}

		private void BtnCancelSaveAddress_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("¿Desea cancelar el registro de dirección de cliente?", "Cancelar registro", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				BrdAddClientAddress.Visibility = Visibility.Hidden;
				BrdFoundClientPopUp.Visibility = Visibility.Visible;
			}
		}

		private void BtnSaveAddress_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Desea registrar la dirección de cliente?", "Confirmación", MessageBoxButton.YesNo);

			if (messageBoxResult == MessageBoxResult.No)
			{
				return;
			}

			if (!AreFieldsFilled())
			{
				MessageBox.Show("Llene todos los campos.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			Address newAddress = new Address()
			{
				IdAddress = Guid.NewGuid(),
				Street = TxtStreet.Text,
				Number = int.Parse(TxtNumber.Text),
				Colony = TxtColony.Text,
				PostalCode = TxtPostalCode.Text,
				Reference = TxtReference.Text,
				Status = true,
				IdClient = foundClient.IdClient
			};

			int result = AddressOperations.SaveAddress(newAddress);

			switch (result)
			{
				case 1:
					MessageBox.Show("Dirección registrada exitosamente.");
					SetCreatedAddressAsSelectedAddress(newAddress);
					BrdAddClientAddress.Visibility = Visibility.Hidden;

					ShowFoundClientInformation(foundClient);
					LblSelectedClientStreet.Content = selectedAddress.Street;
					LblSelectedClientAddressNumber.Content = selectedAddress.Number;
					LblSelectedClientColony.Content = selectedAddress.Colony;
					break;
				case 0:
					MessageBox.Show("No se pudo registrar la dirección.");
					break;
				case -1:
					MessageBox.Show("Ocurrió un error con la base de datos inténtelo más tarde.");
					break;
				default:
					break;
			}
		}
	}
}

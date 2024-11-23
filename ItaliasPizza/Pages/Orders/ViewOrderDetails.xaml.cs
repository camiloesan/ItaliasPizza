using ItaliasPizza.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ItaliasPizza.DataAccessLayer;
using System.Collections.ObjectModel;
using Database;
using System;

namespace ItaliasPizza.Pages.Orders
{
	/// <summary>
	/// Interaction logic for ViewOrderDetails.xaml
	/// </summary>
	public partial class ViewOrderDetails : Page
	{
		private OrderDetails _orderDetails;
		private string _notDeliveredReason;
		private const int TRANSACTION_TYPE_SALE = 1;
		public ViewOrderDetails(OrderDetails orderDetails)
		{
			_orderDetails = orderDetails;

			InitializeComponent();
			FillDtgOrderProducts();
			InitializeUserTypeButtons();
		}

		private void InitializeUserTypeButtons()
		{
			switch (SessionDetails.UserType) 
			{
				case "Cocinero":
					BtnPreparation.Visibility = Visibility.Visible;
					BtnPrepared.Visibility = Visibility.Visible;
					BtnTransit.Visibility = Visibility.Hidden;
					BtnDelivered.Visibility = Visibility.Hidden;
					BtnNotDelivered.Visibility = Visibility.Visible;
					break;
				case "Repartidor":
					BtnPreparation.Visibility = Visibility.Hidden;
					BtnPrepared.Visibility = Visibility.Hidden;
					BtnTransit.Visibility = Visibility.Visible;
					BtnDelivered.Visibility = Visibility.Visible;
					BtnNotDelivered.Visibility = Visibility.Visible;
					break;
			}
		}

		private void ImgReturn(object sender, MouseButtonEventArgs e)
		{
			Application.Current.MainWindow.Content = new ViewOrders();
		}

		private void SaveTransaction()
		{
            var sale = new Transaction
            {
                IdTransaction = Guid.NewGuid(),
                IdTransactionType = TRANSACTION_TYPE_SALE,
                Date = DateTime.Now,
                Amount = int.Parse(_orderDetails.TotalPrice),
                Description = "Sale: " + _orderDetails.TotalPrice,
                RegisteredBy = SessionDetails.IdEmployee
            };
            TransactionOperations.SaveTransaction(sale);
        }

		private void BtnPreparation_Click(object sender, MouseButtonEventArgs e)
		{
			var updatedProduct = 0;
			MessageBoxResult result = MessageBox.Show("¿Desea cambiar el estado del producto a 'En preparación'?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				OrderStatus orderStatus = OrderStatusOperations.GetOrderStatusByName("En preparacion");

				if (_orderDetails.OrderType == "Local")
				{
					LocalOrder localOrder = LocalOrderOperations.GetLocalOrderById(_orderDetails.OrderId);
					updatedProduct = LocalOrderOperations.UpdateLocalOrderStatus(localOrder, orderStatus);					
				}
				else if (_orderDetails.OrderType == "A domicilio")
				{
					DeliveryOrder deliveryOrder = DeliveryOrderOperations.GetDeliveryOrderById(_orderDetails.OrderId);
					updatedProduct = DeliveryOrderOperations.UpdateDeliveryOrderStatus(deliveryOrder, orderStatus);
				}

				if (updatedProduct > 0)
				{
					MessageBox.Show("El estado del pedido ha sido actualizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
					Application.Current.MainWindow.Content = new ViewOrders();
				}
				else
				{
					MessageBox.Show("Ha ocurrido un error al actualizar el estado del pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void BtnTransit_Click(object sender, MouseButtonEventArgs e)
		{
			var updatedProduct = 0;
			MessageBoxResult result = MessageBox.Show("¿Desea cambiar el estado del producto a 'En tránsito'?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				OrderStatus orderStatus = OrderStatusOperations.GetOrderStatusByName("En camino");

				DeliveryOrder deliveryOrder = DeliveryOrderOperations.GetDeliveryOrderById(_orderDetails.OrderId);
				updatedProduct = DeliveryOrderOperations.UpdateDeliveryOrderStatus(deliveryOrder, orderStatus);

				if (updatedProduct > 0)
				{
					MessageBox.Show("El estado del pedido ha sido actualizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
					Application.Current.MainWindow.Content = new ViewOrders();
				}
				else
				{
					MessageBox.Show("Ha ocurrido un error al actualizar el estado del pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void BtnPrepared_Click(object sender, MouseButtonEventArgs e)
		{
			var updatedProduct = 0;
			MessageBoxResult result = MessageBox.Show("¿Desea cambiar el estado del producto a 'Listo para entregar'?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				OrderStatus orderStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");

				if (_orderDetails.OrderType == "Local")
				{
					LocalOrder localOrder = LocalOrderOperations.GetLocalOrderById(_orderDetails.OrderId);
					updatedProduct = LocalOrderOperations.UpdateLocalOrderStatus(localOrder, orderStatus);
				}
				else if (_orderDetails.OrderType == "A domicilio")
				{
					DeliveryOrder deliveryOrder = DeliveryOrderOperations.GetDeliveryOrderById(_orderDetails.OrderId);
					updatedProduct = DeliveryOrderOperations.UpdateDeliveryOrderStatus(deliveryOrder, orderStatus);
				}

				if (updatedProduct > 0)
				{
					MessageBox.Show("El estado del pedido ha sido actualizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
					Application.Current.MainWindow.Content = new ViewOrders();
				}
				else
				{
					MessageBox.Show("Ha ocurrido un error al actualizar el estado del pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void BtnDelivered_Click(object sender, MouseButtonEventArgs e)
		{
			var updatedProduct = 0;
			MessageBoxResult result = MessageBox.Show("¿Desea cambiar el estado del producto a 'Entregado'?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
                OrderStatus orderStatus = OrderStatusOperations.GetOrderStatusByName("Entregado");
                switch (SessionDetails.UserType)
                {
                    case "Cocinero":
                        LocalOrder localOrder = LocalOrderOperations.GetLocalOrderById(_orderDetails.OrderId);
                        updatedProduct = LocalOrderOperations.UpdateLocalOrderStatus(localOrder, orderStatus);
                        break;
                    case "Repartidor":
                        DeliveryOrder deliveryOrder = DeliveryOrderOperations.GetDeliveryOrderById(_orderDetails.OrderId);
                        updatedProduct = DeliveryOrderOperations.UpdateDeliveryOrderStatus(deliveryOrder, orderStatus);
                        break;
                }	

				if (updatedProduct > 0)
				{
					SaveTransaction();
					MessageBox.Show("El estado del pedido ha sido actualizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
					Application.Current.MainWindow.Content = new ViewOrders();
				}
				else
				{
					MessageBox.Show("Ha ocurrido un error al actualizar el estado del pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void BtnNotDelivered_Click(object sender, MouseButtonEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("¿Desea cambiar el estado del producto a 'No entregado'?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				NotDeliveredForm.Visibility = Visibility.Visible;
				BtnTransit.IsEnabled = false;
				BtnDelivered.IsEnabled = false;
				BtnNotDelivered.IsEnabled = false;
				BtnCancelOrder.IsEnabled = false;
			}
		}

		private void BtnCancelOrder_Click(object sender, MouseButtonEventArgs e)
		{
			var updatedProduct = 0;
			MessageBoxResult result = MessageBox.Show("¿Desea cancelar el pedido?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				OrderStatus orderStatus = OrderStatusOperations.GetOrderStatusByName("Cancelado");

				if (_orderDetails.OrderType == "Local")
				{
					LocalOrder localOrder = LocalOrderOperations.GetLocalOrderById(_orderDetails.OrderId);
					updatedProduct = LocalOrderOperations.UpdateLocalOrderStatus(localOrder, orderStatus);

					// Registrar perdida de insumos en caso de que haya estado preparado
				}
				else if (_orderDetails.OrderType == "A domicilio")
				{
					DeliveryOrder deliveryOrder = DeliveryOrderOperations.GetDeliveryOrderById(_orderDetails.OrderId);
					updatedProduct = DeliveryOrderOperations.UpdateDeliveryOrderStatus(deliveryOrder, orderStatus);

					// Registrar perdida de insumos en caso de que haya estado preparado
				}

				if (updatedProduct > 0)
				{
					MessageBox.Show("El estado del pedido ha sido actualizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
					Application.Current.MainWindow.Content = new ViewOrders();
				}
				else
				{
					MessageBox.Show("Ha ocurrido un error al actualizar el estado del pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void BtnSaveReason_Click(object sender, RoutedEventArgs e)
		{
			_notDeliveredReason = TxtNotDeliveredReason.Text;

			if (string.IsNullOrEmpty(_notDeliveredReason))
			{
					MessageBox.Show("Por favor, ingrese el motivo por el cual no se pudo entregar el pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			} else if (_notDeliveredReason.Length > 100)
			{
				MessageBox.Show("El motivo no puede exceder los 100 caracteres", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			} else 
			{
				NotDeliveredOrder(_notDeliveredReason);
			}
		}

		private void NotDeliveredOrder(string reason)
		{
			int updatedProduct = 0;
			int updatedReason = 0;
			OrderStatus orderStatus = OrderStatusOperations.GetOrderStatusByName("No entregado");

			DeliveryOrder deliveryOrder = DeliveryOrderOperations.GetDeliveryOrderById(_orderDetails.OrderId);
			updatedProduct = DeliveryOrderOperations.UpdateDeliveryOrderStatus(deliveryOrder, orderStatus);
			updatedReason = DeliveryOrderOperations.SetNotDeliveredReason(deliveryOrder, reason);

			if (updatedProduct > 0 && updatedReason > 0)
			{
				MessageBox.Show("El estado del pedido ha sido actualizado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
				NotDeliveredForm.Visibility = Visibility.Hidden;
				BtnTransit.IsEnabled = true;
				BtnDelivered.IsEnabled = true;
				BtnNotDelivered.IsEnabled = true;
				BtnCancelOrder.IsEnabled = true;
				Application.Current.MainWindow.Content = new ViewOrders();
				_notDeliveredReason = null;
			}
			else
			{
				MessageBox.Show("Ha ocurrido un error al actualizar el estado del pedido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				NotDeliveredForm.Visibility = Visibility.Hidden;
				BtnTransit.IsEnabled = true;
				BtnDelivered.IsEnabled = true;
				BtnNotDelivered.IsEnabled = true;
				BtnCancelOrder.IsEnabled = true;
				_notDeliveredReason = null;
			}
		}

		private void BtnCancelReason_Click(object sender, RoutedEventArgs e)
		{
			NotDeliveredForm.Visibility = Visibility.Hidden;
			BtnTransit.IsEnabled = true;
			BtnDelivered.IsEnabled = true;
			BtnNotDelivered.IsEnabled = true;
			BtnCancelOrder.IsEnabled = true;
		}

		private void FillDtgOrderProducts()
		{
			var orderProducts = ConvertOrderProductsToVisibleProducts();

			var items = new ObservableCollection<OrderProductDetails>(orderProducts);
			DtgOrderProducts.ItemsSource = items;
		}

		private List<OrderProductDetails> ConvertOrderProductsToVisibleProducts()
		{
			var orderProducts = new List<OrderProductDetails>();
			var orderType = _orderDetails.OrderType;
			var orderId = _orderDetails.OrderId;

			if (orderType == "Local")
			{
				var localOrderProducts = LocalOrderProductOperations.GetLocalOrderProductsByOrderId(orderId);

				foreach (var localOrderProduct in localOrderProducts)
				{
					var orderProduct = new OrderProductDetails();
					orderProduct.OrderProductId = localOrderProduct.IdLocalOrderProduct;

					var product = ProductOperations.GetProductById(localOrderProduct.IdProduct);
					orderProduct.ProductName = product.Name + " " + product.Size;

					orderProduct.Quantity = localOrderProduct.Quantity.ToString();
					orderProduct.UnitPrice = product.Price;
					orderProduct.SubTotal = (orderProduct.UnitPrice * localOrderProduct.Quantity);

					orderProducts.Add(orderProduct);
				}
			}
			else if (orderType == "A domicilio")
			{
				var deliveryOrderProducts = DeliveryOrderProductOperations.GetDeliveryOrderProductsByOrderId(orderId);

				foreach (var deliveryOrderProduct in  deliveryOrderProducts)
				{
					var orderProduct = new OrderProductDetails();
					orderProduct.OrderProductId = deliveryOrderProduct.IdDeliveryOrderProduct;

					var product = ProductOperations.GetProductById(deliveryOrderProduct.IdProduct);
					orderProduct.ProductName = product.Name + " " + product.Size;

					orderProduct.Quantity = deliveryOrderProduct.Quantity.ToString();
					orderProduct.UnitPrice = product.Price;
					orderProduct.SubTotal = (orderProduct.UnitPrice * deliveryOrderProduct.Quantity);

					orderProducts.Add(orderProduct);
				}
			}

			return orderProducts;
		}		
	}
}

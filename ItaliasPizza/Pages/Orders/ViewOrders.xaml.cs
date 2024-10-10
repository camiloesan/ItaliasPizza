using System;
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
using ItaliasPizza.Utils;
using ItaliasPizza.DataAccessLayer;
using Database;
using System.Collections.ObjectModel;

namespace ItaliasPizza.Pages.Orders
{
	/// <summary>
	/// Interaction logic for ViewOrders.xaml
	/// </summary>
	public partial class ViewOrders : Page
	{
		

		public ViewOrders()
		{
			InitializeComponent();

			UserTypeLabel.Content = SessionDetails.UserType;
			if (SessionDetails.UserType == "Repartidor")
			{
				DeliveryFilterBar.Visibility = Visibility.Visible;
				DeliveryCurrentFilter.Content = "Listo para entrega";
				BtnAddLocalOrder.Visibility = Visibility.Hidden;

				OrderStatus readyStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
				FillDtgDeliveryOrders(readyStatus);
			}
			else if (SessionDetails.UserType == "Cocinero")
			{
				CookFilterBar.Visibility = Visibility.Visible;
				CookCurrentFilter.Content = "Pendientes";
				BtnAddLocalOrder.Visibility = Visibility.Hidden;

				OrderStatus pendingStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");
				FillDtgOrders(pendingStatus);
			}
			else if (SessionDetails.UserType == "Mesero")
			{
				WaiterFilterBar.Visibility = Visibility.Visible;
				WaiterCurrentFilter.Content = "Listos para entrega";

				OrderStatus readyStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
				FillDtgLocalOrders(readyStatus);
			}
		}

		private void ImgReturn_Click(object sender, MouseButtonEventArgs e)
		{

		}

		private void BtnCancelOrder_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void BtnViewOrder_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;

			var order = button.DataContext as OrderDetails;

			Application.Current.MainWindow.Content = new ViewOrderDetails(order);
		}

		private void BtnAddLocalOrder_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new AddLocalOrder();
		}

		private void FillDtgOrders(OrderStatus statusFilter)
		{
			var orders = ConvertOrdersToGenericOrder(statusFilter);
	
			var items = new ObservableCollection<OrderDetails>(orders);
			DtgOrders.ItemsSource = items;
		}

		private void FillDtgDeliveryOrders(OrderStatus statusFilter)
		{
			var orders = ConvertDeliveryOrderToGenericOrderByStatus(statusFilter);

			var items = new ObservableCollection<OrderDetails>(orders);
			DtgOrders.ItemsSource = items;
		}
		
		private void FillDtgLocalOrders(OrderStatus statusFilter)
		{
			var orders = ConvertLocalOrdersToGenericOrderByStatus(statusFilter);

			var items = new ObservableCollection<OrderDetails>(orders);
			DtgOrders.ItemsSource = items;
		}

		private List<OrderDetails> ConvertOrdersToGenericOrder(OrderStatus orderStatus){
			var orders = new List<OrderDetails>();
			//var orderStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");

			var deliveryOrders = DeliveryOrderOperations.GetDeliveryOrdersByStatus(orderStatus);
			var localOrders = LocalOrderOperations.GetLocalOrdersByStatus(orderStatus);

			foreach (var deliveryOrder in deliveryOrders)
			{
				var order = new OrderDetails();
				order.OrderId = deliveryOrder.IdDeliveryOrder;

				var client = ClientOperations.GetClientByDeliveryOrder(deliveryOrder);
				order.Client = client.FirstName + " " + client.LastName;

				order.Table = "N/A";

				order.OrderType = "A domicilio";
				order.OrderDate = deliveryOrder.Date;

				var thisOrderStatus = OrderStatusOperations.GetOrderStatusByName(orderStatus.Status);
				order.Status = thisOrderStatus.Status;

				order.TotalPrice = deliveryOrder.Total.ToString();
				orders.Add(order);
			}

			foreach (var localOrder in localOrders)
			{
				var order = new OrderDetails();
				order.OrderId = localOrder.IdLocalOrder;
				order.Client = "N/A";
				order.Table = localOrder.Table.ToString();
				order.OrderType = "Local";
				order.OrderDate = localOrder.Date;

				var thisOrderStatus = OrderStatusOperations.GetOrderStatusByName(orderStatus.Status);
				order.Status = thisOrderStatus.Status;

				order.TotalPrice = localOrder.Total.ToString();
				orders.Add(order);
			}

			orders.OrderBy(order => order.OrderDate);

			foreach (var order in orders)
			{
				Console.WriteLine(order.OrderId + " " + order.OrderType + " " + order.OrderDate);
			}

			return orders;
		}

		private List<OrderDetails> ConvertLocalOrdersToGenericOrderByStatus(OrderStatus orderStatus)
		{
			var orders = new List<OrderDetails>();
			//var orderStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");

			var localOrders = LocalOrderOperations.GetLocalOrdersByStatus(orderStatus);

			foreach (var localOrder in localOrders)
			{
				var order = new OrderDetails();
				order.OrderId = localOrder.IdLocalOrder;
				order.Client = "N/A";
				order.Table = localOrder.Table.ToString();
				order.OrderType = "Local";
				order.OrderDate = localOrder.Date;

				var thisOrderStatus = OrderStatusOperations.GetOrderStatusByName(orderStatus.Status);
				order.Status = thisOrderStatus.Status;

				order.TotalPrice = localOrder.Total.ToString();
				orders.Add(order);
			}

			orders.OrderBy(order => order.OrderDate);

			foreach (var order in orders)
			{
				Console.WriteLine(order.OrderId + " " + order.OrderType + " " + order.OrderDate);
			}

			return orders;
		}

		private List<OrderDetails> ConvertDeliveryOrderToGenericOrderByStatus(OrderStatus orderStatus)
		{
			var orders = new List<OrderDetails>();

			var deliveryOrders = DeliveryOrderOperations.GetDeliveryOrdersByStatus(orderStatus);

			foreach (var deliveryOrder in deliveryOrders)
			{
				var order = new OrderDetails();
				order.OrderId = deliveryOrder.IdDeliveryOrder;

				var client = ClientOperations.GetClientByDeliveryOrder(deliveryOrder);
				order.Client = client.FirstName + " " + client.LastName;

				order.Table = "N/A";

				order.OrderType = "A domicilio";
				order.OrderDate = deliveryOrder.Date;

				var thisOrderStatus = OrderStatusOperations.GetOrderStatusByName(orderStatus.Status);
				order.Status = thisOrderStatus.Status;

				order.TotalPrice = deliveryOrder.Total.ToString();
				orders.Add(order);
			}

			return orders;
		}

		//Cook buttons
		private void BtnFilterPending_Click(object sender, RoutedEventArgs e)
		{
			OrderStatus pendingStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");
			if (SessionDetails.UserType == "Mesero")
			{
				FillDtgLocalOrders(pendingStatus);
				WaiterCurrentFilter.Content = "Pendientes";
			}
			else if (SessionDetails.UserType == "Cocinero")
			{
				FillDtgOrders(pendingStatus);
				CookCurrentFilter.Content = "Pendientes";
			}
		}

		private void BtnFilterInPreparation_Click(object sender, RoutedEventArgs e)
		{
			OrderStatus preparationStatus = OrderStatusOperations.GetOrderStatusByName("En preparacion");

			if (SessionDetails.UserType == "Cocinero")
			{
				FillDtgOrders(preparationStatus);
				CookCurrentFilter.Content = "En preparacion";
			} else if (SessionDetails.UserType == "Mesero")
			{
				FillDtgLocalOrders(preparationStatus);
				WaiterCurrentFilter.Content = "En preparacion";
			}
		}

		private void BtnFilterReadyToDeliver_Click(object sender, RoutedEventArgs e)
		{
			OrderStatus readyStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar"); 
			if (SessionDetails.UserType == "Repartidor") {
				FillDtgDeliveryOrders(readyStatus);
				DeliveryCurrentFilter.Content = "Listo para entrega";
			} else if (SessionDetails.UserType == "Mesero")
			{
				FillDtgLocalOrders(readyStatus);
				WaiterCurrentFilter.Content = "Listos para entrega";
			} else if (SessionDetails.UserType == "Cocinero")
			{
				FillDtgOrders(readyStatus);
				CookCurrentFilter.Content = "Listos para entrega";
			}
		}

		private void BtnFilterInTransit_Click(object sender, RoutedEventArgs e)
		{
			OrderStatus transitStatus = OrderStatusOperations.GetOrderStatusByName("En camino");
			FillDtgDeliveryOrders(transitStatus);
			DeliveryCurrentFilter.Content = "En transito";
		}
	}
}

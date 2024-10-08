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
			FillDtgOrders();
		}

		private void ImgReturn_Click(object sender, MouseButtonEventArgs e)
		{

		}

		private void BtnCancelOrder_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void BtnViewOrder_Click(object sender, RoutedEventArgs e)
		{

		}

		private void BtnAddLocalOrder_Click(object sender, RoutedEventArgs e)
		{

		}

		private void FillDtgOrders()
		{
			var orders = ConvertOrdersToGenericOrder();
	
			var items = new ObservableCollection<OrderDetails>(orders);
			DtgOrders.ItemsSource = items;
		}

		private List<OrderDetails> ConvertOrdersToGenericOrder(){
			var orders = new List<OrderDetails>();
			var orderStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");

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
	}
}

using ItaliasPizza.Utils;
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
using ItaliasPizza.DataAccessLayer;
using System.Collections.ObjectModel;

namespace ItaliasPizza.Pages.Orders
{
	/// <summary>
	/// Interaction logic for ViewOrderDetails.xaml
	/// </summary>
	public partial class ViewOrderDetails : Page
	{
		private OrderDetails _orderDetails;
		public ViewOrderDetails(OrderDetails orderDetails)
		{
			_orderDetails = orderDetails;

			InitializeComponent();
			FillDtgOrderProducts();
		}

		private void ImgReturn(object sender, MouseButtonEventArgs e)
		{
			Application.Current.MainWindow.Content = new ViewOrders();
		}

		private void BtnPreparation_Click(object sender, MouseButtonEventArgs e)
		{

		}

		private void BtnTransit_Click(object sender, MouseButtonEventArgs e)
		{
			
		}

		private void BtnPrepared_Click(object sender, MouseButtonEventArgs e)
		{

		}

		private void BtnDelivered_Click(object sender, MouseButtonEventArgs e)
		{

		}

		private void BtnCancelOrder_Click(object sender, MouseButtonEventArgs e)
		{

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

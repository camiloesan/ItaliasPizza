using Database;
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
			var items = new ObservableCollection<Product>()
			{
				new Product {Name = "Pizza", Price = 100},
			};
			DtgActiveProducts.ItemsSource = items;
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
	}
}

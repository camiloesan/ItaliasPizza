using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Pages.InventoryReport;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizza.Pages.Clients
{
	/// <summary>
	/// Interaction logic for ViewClients.xaml
	/// </summary>
	public partial class ViewClients : Page
	{
		private const string FIRST_NAME = "Nombre";
		private const string LAST_NAME = "Apellido";
		private const string PHONE = "Teléfono";

		public ViewClients()
		{
			InitializeComponent();
			FillDtgClients(alphabeticalFiftyClients);
			FillCmbFilter();
		}

		private readonly List<Client> alphabeticalFiftyClients = ClientOperations.GetFiftyClients();

		private void FillCmbFilter()
		{
			List<string> filters = new List<string>
			{
				FIRST_NAME,
				LAST_NAME,
				PHONE
			};
			CmbFilter.ItemsSource = filters;
			CmbFilter.SelectedIndex = 0;
		}

		private void FillDtgClients(List<Client> clients)
		{
			DtgClients.ItemsSource = new ObservableCollection<Client>(clients);
		}

		private List<Client> SearchClients(string searchQuery)
		{
			List<Client> clients = ClientOperations.GetClients();
			List<Client> filteredClients = null;

			switch (CmbFilter.Text)
			{
				case FIRST_NAME:
					filteredClients = clients.Where(c => c.FirstName.ToLower().Contains(searchQuery)).ToList();
					break;
				case LAST_NAME:
					filteredClients = clients.Where(c => c.LastName.ToLower().Contains(searchQuery)).ToList();
					break;
				case PHONE:
					filteredClients = clients.Where(c => c.Phone.Contains(searchQuery)).ToList();
					break;
			}

			return filteredClients;
		}

		private void BtnSearch_Click(object sender, RoutedEventArgs e)
		{
			string search = TxtSearchBar.Text.ToLower();
			var filteredList = SearchClients(search);

			if (filteredList.Count == 0)
			{
				MessageBox.Show("No se encontraron resultados", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			FillDtgClients(filteredList);
		}

		private void BtnEditClient_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Client client = (Client)button.DataContext;

			Application.Current.MainWindow.Content = new ClientModification(client);
		}
        private void Btn_Employees(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Employees();
        }

        private void Btn_Supplies(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Inventory();
        }

        private void Btn_Suppliers(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SuppliersList();
        }

        private void Btn_Clients(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new ViewClients();
        }

        private void Btn_Products(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Products();
        }
        private void Btn_SupplierOrders(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SupplierOrders();
        }

        private void Btn_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Login();
        }
    }
}

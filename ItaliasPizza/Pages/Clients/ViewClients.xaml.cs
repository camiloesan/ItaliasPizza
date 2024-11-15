using Database;
using ItaliasPizza.DataAccessLayer;
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
	}
}

using Database;
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

namespace ItaliasPizza.Pages.Clients
{
	/// <summary>
	/// Interaction logic for RegisterClient.xaml
	/// </summary>
	public partial class RegisterClient : Page
	{
		public RegisterClient()
		{
			InitializeComponent();
		}

		private void ImgReturn(object sender, MouseButtonEventArgs e)
		{

		}

		private void BtnSaveClient_Click(object sender, RoutedEventArgs e)
		{
			HighlightInvalidFields();
			if (!AreFieldsFilled())
			{
				MessageBox.Show("Por favor llene todos los campos.");
				return;
			}
			else if (!IsInputNumber(TxtPhone.Text) || !IsPhoneNumberValid(TxtPhone.Text))
			{
				MessageBox.Show("Ingrese un número telefónico válido.");
				return;
			}
			else if (ClientOperations.IsPhoneRegistered(TxtPhone.Text))
			{
				MessageBox.Show("El número telefónico ya está registrado.");
				return;
			}
			else if (!IsInputNumber(TxtNumber.Text))
			{
				MessageBox.Show("Ingrese un número de casa válido.");
				return;
			}
			else if (!IsInputNumber(TxtPostalCode.Text))
			{
				MessageBox.Show("Ingrese un código postal válido.");
				return;
			}

			MessageBoxResult messageBoxResult = MessageBox.Show("¿Desea registrar el cliente?", "Confirmación", MessageBoxButton.YesNo);

			if (messageBoxResult == MessageBoxResult.No)
			{
				return;
			}

			var clientId = Guid.NewGuid();
			var name = TxtName.Text;
			var lastName = TxtLastName.Text;
			var phone = TxtPhone.Text;

			var addressId = Guid.NewGuid();
			var street = TxtStreet.Text;
			var number = int.Parse(TxtNumber.Text);
			var colony = TxtColony.Text;
			var postalCode = TxtPostalCode.Text;
			var reference = TxtReference.Text;

			Console.WriteLine("Cliente:");
			Console.WriteLine($"Nombre: {name}");
			Console.WriteLine($"Apellido: {lastName}");
			Console.WriteLine($"Teléfono: {phone}");

			Console.WriteLine("Dirección:");
			Console.WriteLine($"Calle: {street}");
			Console.WriteLine($"Número: {number}");
			Console.WriteLine($"Colonia: {colony}");
			Console.WriteLine($"Código Postal: {postalCode}");
			Console.WriteLine($"Referencia: {reference}");

			Client newClient = new Client
			{
				IdClient = clientId,
				FirstName = name,
				LastName = lastName,
				Phone = phone
			};

			Address newAddress = new Address
			{
				IdAddress = addressId,
				Street = street,
				Number = number,
				Colony = colony,
				PostalCode = postalCode,
				Reference = reference,
				Status = true,
				IdClient = clientId
			};

			int result = ClientOperations.SaveClient(newClient, newAddress);
			if (result == 0)
			{
				MessageBox.Show("No se pudo registrar el cliente.");
				return;
			}
			else
			{
				MessageBox.Show("Cliente registrado exitosamente.");
			}

			EmptyFields();
			// Return to delivery order view
			// Application.Current.MainWindow.Content = new AddDeliveryOrder();
		}

		private void BtnCancelSaveClient_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Desea cancelar el registro del cliente?", "Confirmación", MessageBoxButton.YesNo);

			if (messageBoxResult == MessageBoxResult.No)
			{
				return;
			}

			EmptyFields();
			// Return to delivery order view
			// Application.Current.MainWindow.Content = new AddDeliveryOrder();
		}

		private bool HighlightInvalidFields()
		{
			bool isValid = false;

			string name = TxtName.Text.Trim();
			string lastname = TxtLastName.Text.Trim();
			string phone = TxtPhone.Text.Trim();

			string street = TxtStreet.Text.Trim();
			string number = TxtNumber.Text.Trim();
			string colony = TxtColony.Text.Trim();
			string postalCode = TxtPostalCode.Text.Trim();

			bool isPhoneNumber = IsInputNumber(phone);
			bool isPhoneNumberValid = IsPhoneNumberValid(phone);
			bool isNumber = IsInputNumber(number);
			bool isPostalCodeNumber = IsInputNumber(postalCode);

			if (string.IsNullOrEmpty(name))
			{
				TxtName.BorderBrush = Brushes.Red;
				isValid = false;
			}

			if (string.IsNullOrEmpty(lastname))
			{
				TxtLastName.BorderBrush = Brushes.Red;
				isValid = false;
			}

			if (string.IsNullOrEmpty(phone) || !isPhoneNumber || !isPhoneNumberValid)
			{
				TxtPhone.BorderBrush = Brushes.Red;
				isValid = false;
			}


			if (string.IsNullOrEmpty(street))
			{
				TxtStreet.BorderBrush = Brushes.Red;
				isValid = false;
			}

			if (string.IsNullOrEmpty(number) || !isNumber)
			{
				TxtNumber.BorderBrush = Brushes.Red;
				isValid = false;
			}

			if (string.IsNullOrEmpty(colony))
			{
				TxtColony.BorderBrush = Brushes.Red;
				isValid = false;
			}

			if (string.IsNullOrEmpty(postalCode) || !IsInputNumber(postalCode))
			{
				TxtPostalCode.BorderBrush = Brushes.Red;
				isValid = false;
			}

			return isValid;
		}

		private void ResetTextFormBorders()
		{
			TxtName.BorderBrush = Brushes.Black;
			TxtName.BorderThickness = new Thickness(1);
			TxtLastName.BorderBrush = Brushes.Black;
			TxtLastName.BorderThickness = new Thickness(1);
			TxtPhone.BorderBrush = Brushes.Black;
			TxtPhone.BorderThickness = new Thickness(1);

			TxtStreet.BorderBrush = Brushes.Black;
			TxtStreet.BorderThickness = new Thickness(1);
			TxtNumber.BorderBrush = Brushes.Black;
			TxtNumber.BorderThickness = new Thickness(1);
			TxtColony.BorderBrush = Brushes.Black;
			TxtColony.BorderThickness = new Thickness(1);
			TxtPostalCode.BorderBrush = Brushes.Black;
			TxtPostalCode.BorderThickness = new Thickness(1);
		}

		private bool AreFieldsFilled()
		{
			return !string.IsNullOrEmpty(TxtName.Text)
				&& !string.IsNullOrEmpty(TxtLastName.Text)
				&& !string.IsNullOrEmpty(TxtPhone.Text)
				&& !string.IsNullOrEmpty(TxtStreet.Text)
				&& !string.IsNullOrEmpty(TxtNumber.Text)
				&& !string.IsNullOrEmpty(TxtColony.Text)
				&& !string.IsNullOrEmpty(TxtPostalCode.Text);
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
		private void EmptyFields()
		{
			TxtName.Text = string.Empty;
			TxtLastName.Text = string.Empty;
			TxtPhone.Text = string.Empty;
			TxtStreet.Text = string.Empty;
			TxtNumber.Text = string.Empty;
			TxtColony.Text = string.Empty;
			TxtPostalCode.Text = string.Empty;
			TxtReference.Text = string.Empty;
		}
	}
}

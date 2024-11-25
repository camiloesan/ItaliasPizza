using Database;
using ItaliasPizza.DataAccessLayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ItaliasPizza.Pages.InventoryReport
{
	/// <summary>
	/// Interaction logic for FinishInventoryReport.xaml
	/// </summary>
	public partial class FinishInventoryReport : Page
	{
		private Database.InventoryReport currentInventoryReport = InventoryReportOperations.GetInventoryReport();
		private List<SupplyInventoryReport> supplyInventoryReports;

		private void GetSupplyInventoryReports()
		{
			if (currentInventoryReport != null){
				supplyInventoryReports = (List<SupplyInventoryReport>)InventoryReportOperations.GetInventoryReport().SupplyInventoryReport.ToList();
			}
		}

		public FinishInventoryReport()
		{
			InitializeComponent();

			if (CheckForExistingReport())
			{
				NoReportMessage.Visibility = Visibility.Hidden;
				ReportInfo.Visibility = Visibility.Visible;
				GetSupplyInventoryReports();
				FillSuppliesList();
			} else 
			{
				ReportInfo.Visibility = Visibility.Hidden;
				NoReportMessage.Visibility = Visibility.Visible;
			}
		}

		private bool CheckForExistingReport()
		{
			var reportExists = false;
			if (currentInventoryReport != null)
			{
				reportExists = true;
			}

			return reportExists;
		}

		private void FillSuppliesList() {
			DtgInventoryReportSupplies.ItemsSource = new ObservableCollection<SupplyInventoryReport>(supplyInventoryReports);
		}

		private void BtnSaveReport_Click(object sender, RoutedEventArgs e)
		{
			currentInventoryReport.SupplyInventoryReport = supplyInventoryReports;
			currentInventoryReport.Status = true;
			currentInventoryReport.Observations = TxtObservations.Text;


			int affectedRows = InventoryReportOperations.UpdateInventoryReport(currentInventoryReport);

			if (affectedRows > 0)
			{
				MessageBox.Show("Reporte de inventario finalizado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				MessageBox.Show("Error al finalizar el reporte de inventario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void TxtReportedAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			e.Handled = !IsInputNumber(e.Text, textBox);
		}

		private bool IsInputNumber(string input, TextBox textBox)
		{
			string futureText = textBox.Text.Insert(textBox.CaretIndex, input);

			string pattern = @"^\d{0,5}$|^\d{0,5}\.$|^\d{0,5}\.\d{0,2}$";

			return System.Text.RegularExpressions.Regex.IsMatch(futureText, pattern);
		}

		private void BtnStartReport_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new StartInventoryReport();
		}

		// -- SIDE BAR BUTTONS -- //

		private void Btn_Employees(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Employees();
		}

		private void Btn_Supplies(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Inventory();
		}

		private void Btn_Orders(object sender, RoutedEventArgs e)
		{
			
		}

		private void Btn_Products(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new Products();
		}

		private void Btn_Suppliers(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.Content = new SuppliersList();
		}

		private void Btn_Reports(object sender, RoutedEventArgs e)
		{
			//Application.Current.MainWindow.Content = new Reports();
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

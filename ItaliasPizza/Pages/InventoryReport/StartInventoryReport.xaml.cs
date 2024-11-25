using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using Database;

namespace ItaliasPizza.Pages.InventoryReport
{
	/// <summary>
	/// Interaction logic for StartInventoryReport.xaml
	/// </summary>
	public partial class StartInventoryReport : Page
	{
		private List<SupplyInventoryReportDetails> _supplyInventoryReportsDetails = new List<SupplyInventoryReportDetails>();

		public StartInventoryReport()
		{
			InitializeComponent();
			FillDtgSupplies();
		}

		private void FillDtgSupplies()
		{
			DtgInventory.ItemsSource = new ObservableCollection<SupplyDetails>(SupplyOperations.GetSupplyDetailsXes());
		}

		private void BtnGeneratePDF_Click(object sender, RoutedEventArgs e)
		{
			if (InventoryReportOperations.IsInventoryReportOpen())
			{
				MessageBoxResult messageBoxResult = MessageBox.Show(
				   "Ya existe un reporte de inventario abierto.\n\n¿Desea borrar e iniciar uno nuevo o continuar con el existente?",
				   "Reporte de inventario abierto", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (messageBoxResult == MessageBoxResult.Yes) // Delete and start new report
				{
					InventoryReportOperations.DeleteExistingReport();
					GenerateReport();
				}
				else // Continue with existing report
				{
					Application.Current.MainWindow.Content = new FinishInventoryReport();
				}
			}
			else
			{
				GenerateReport();
			}
		}

		private void GenerateReport()
		{
			int result = RegisterPartialInventoryReport();

			switch (result)
			{
				case 0:
					MessageBox.Show("No se pudo iniciar el reporte de inventario inténtelo más tarde.", "Registro exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
					break;
				case 1:
					if (GeneratePDFTemplate())
					{
						MessageBox.Show("Reporte de inventario registrado correctamente y PDF generado", "Registro exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
					}
					else
					{
						MessageBox.Show("Error al generar el PDF", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
					break;
				case -1:
					MessageBox.Show("Error al registrar el reporte de inventario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					break;
			}
		}

		private int RegisterPartialInventoryReport() 
		{ 
			List<Supply> supplies = SupplyOperations.GetSupplies();
			List<SupplyDetails> supplyDetails = SupplyOperations.GetSupplyDetailsXes();

			Database.InventoryReport inventoryReport = new Database.InventoryReport()
			{
				IdInventoryReport = Guid.NewGuid(),
				Reporter = Guid.Parse("9AD3E07D-5AC4-4F5E-B6C5-EB3CE087DF4D"),
				//Reporter = SessionDetails.IdEmployee,
				ReportDate = DateTime.Now,
				Status = false // false = open, true = closed
			};

			// -- DATABASE LIST -- //
			foreach (var item in supplies)
			{
				inventoryReport.SupplyInventoryReport.Add(new SupplyInventoryReport()
				{
					IdSupplyInventoryReport = Guid.NewGuid(),
					IdInventoryReport = inventoryReport.IdInventoryReport,
					IdSupply = item.IdSupply,
					IdMeasurementUnit = item.IdMeasurementUnit,
					ExpectedAmount = item.Quantity,
					ReportedAmount = 0,
					DifferingAmountReason = string.Empty
				});
	
			}

			// -- PDF LIST -- //
			foreach (var item in supplyDetails)
			{
				_supplyInventoryReportsDetails.Add(new SupplyInventoryReportDetails()
				{
					IdSupply = item.IdSupply,
					SupplyName = item.Name,
					MeasurementUnit = item.MeasurementUnit,
					ExpectedAmount = decimal.Parse(item.RawQuantity),
					DifferingAmountReason = string.Empty
				});
			}

			return InventoryReportOperations.RegisterPartialInventoryReportWithSupplies(inventoryReport);
		}

		private bool GeneratePDFTemplate()
		{
			bool wasFileGenerated = true;

			try
			{
				PdfGenerator.GeneratePdfReport(_supplyInventoryReportsDetails);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				wasFileGenerated = false;
			}

			return wasFileGenerated;
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
			Application.Current.MainWindow.Content = new FinishInventoryReport();
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

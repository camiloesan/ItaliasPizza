using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
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

namespace ItaliasPizza.Pages.Recipes
{
	/// <summary>
	/// Interaction logic for RegisterOrder.xaml
	/// </summary>
	public partial class RegisterRecipe : Page
	{
		private Product currentProduct; // Necesario para obtener el id del producto -> obtener de una pantalla productos
		private List<RecipeSupplyDetails> recipeSuppliesDetails = new List<RecipeSupplyDetails>();
		private Supply selectedSupply;
		public RegisterRecipe() // agregar product como parametro del constructor para poder relacionar el producto con la receta
		{
			//currentProduct = recievedProduct;
			InitializeComponent();
			FillDtgAvailableSupplies();
		}

		private void BtnAddRecipeSupply_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			selectedSupply = button.DataContext as Supply;

			if (!IsSupplyAlreadySelected(selectedSupply))
			{
				BtnSaveRecipe.IsEnabled = false;
				BtnCancelRegistration.IsEnabled = false;
				SupplyAmountForm.Visibility = Visibility.Visible;
				LblSupplyAmount.Content = SupplyOperations.GetMeasurementUnitById(selectedSupply.IdMeasurementUnit).MeasurementUnit1;

			} else {
				MessageBox.Show("El ingrediente ya ha sido seleccionado.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void BtnRemoveRecipeSupply_Click(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			var selectedSupplyDetails = button.DataContext as RecipeSupplyDetails;

			recipeSuppliesDetails.Remove(selectedSupplyDetails);
			FillDtgSelectedSupplies();
		}

		private void BtnSaveRecipe_Click(object sender, RoutedEventArgs e)
		{
			if (recipeSuppliesDetails.Count == 0)
			{
				MessageBox.Show("Por favor, seleccione al menos un ingrediente.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			if (string.IsNullOrEmpty(TxtInstructions.Text))
			{
				MessageBox.Show("Por favor, ingrese las instrucciones.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Desea registrar la receta?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (messageBoxResult == MessageBoxResult.No)
			{
				return;
			}
			
			int registeredRecipeResult = 0;

			List<RecipeSupply> recipeSupplies = new List<RecipeSupply>();

			Recipe newRecipe = new Recipe
			{
				IdRecipe = Guid.NewGuid(),
				IdProduct = currentProduct.IdProduct, // product.IdProduct
				//IdProduct = Guid.NewGuid(),
				Instructions = TxtInstructions.Text
			};

			foreach (var item in recipeSuppliesDetails)
			{
				recipeSupplies.Add(new RecipeSupply
				{
					IdRecipeSupply = Guid.NewGuid(),
					IdRecipe = newRecipe.IdRecipe,
					IdSupply = item.IdSupply,
					SupplyAmount = item.SupplyAmount,
					IdMeasurementUnit = item.IdMeasurementUnit
				});
			}
			registeredRecipeResult = RecipeOperations.SaveRecipeWithSupplies(newRecipe, recipeSupplies);

			if (registeredRecipeResult > 0)
			{
				MessageBox.Show("Receta registrada exitosamente.", "Registro exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
				EmptyFields();
				// Return to products view
				// Application.Current.MainWindow.Content = new ViewProducts();
			}
			else
			{
				MessageBox.Show("Ocurrió un error al registrar la receta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void BtnCancelRegistration_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show("¿Desea cancelar el registro de la receta?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (messageBoxResult == MessageBoxResult.No)
			{
				return;
			}

			EmptyFields();
			ResetAmountForm();
			// Return to products view
			// Application.Current.MainWindow.Content = new ViewProducts();
		}

		private void FillDtgAvailableSupplies()
		{
			var availableSupplies = SupplyOperations.GetSupplies();

			if (availableSupplies != null)
			{
				var items = new ObservableCollection<Supply>(availableSupplies);
				DtgAvailableSupplies.ItemsSource = items;
			}
			else
			{
				MessageBox.Show("No hay insumos registrados.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				DtgAvailableSupplies.ItemsSource = null;
			}
		}

		private bool IsSupplyAlreadySelected(Supply newSelectedSupply) {
			bool isAlreadySelected = false;

			var idSelectedSupply = newSelectedSupply.IdSupply;

			foreach (var item in recipeSuppliesDetails) {

				if (item.IdSupply == idSelectedSupply)
				{
					isAlreadySelected = true;
					break;
				}
			}

			return isAlreadySelected;
		}

		private void BtnSaveSupplyAmount_Click(object sender, RoutedEventArgs e)
		{
			var supplyAmount = TxtSupplyAmount.Text;

			if (string.IsNullOrEmpty(supplyAmount)) {
				MessageBox.Show("Por favor, ingrese la cantidad de ingrediente.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			} else if (supplyAmount == "0")
			{
				MessageBox.Show("La cantidad de ingrediente no puede ser 0.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			} else if (!IsInputDecimal(supplyAmount)) {
				MessageBox.Show("Ingrese una cantidad valida", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			else
			{
				var supplyMeasurementUnit = SupplyOperations.GetMeasurementUnitById(selectedSupply.IdMeasurementUnit);
				var newRecipeSupply = new RecipeSupplyDetails{
					IdSupply = selectedSupply.IdSupply,
					SupplyName = selectedSupply.Name,
					SupplyAmount = decimal.Parse(supplyAmount),
					MeasurementUnit = supplyMeasurementUnit.MeasurementUnit1,
					IdMeasurementUnit = selectedSupply.IdMeasurementUnit
				};
				recipeSuppliesDetails.Add(newRecipeSupply);
				FillDtgSelectedSupplies();
				ResetAmountForm();
			}
		}

		private void FillDtgSelectedSupplies()
		{
			var items = new ObservableCollection<RecipeSupplyDetails>(recipeSuppliesDetails);
			DtgSelectedSupplies.ItemsSource = items;
		}

		private void BtnCancelSupplyAmount_Click(object sender, RoutedEventArgs e)
		{
			ResetAmountForm();
		}

		private void ResetAmountForm()
		{
			SupplyAmountForm.Visibility = Visibility.Hidden;
			BtnSaveRecipe.IsEnabled = true;
			BtnCancelRegistration.IsEnabled = true;
			TxtSupplyAmount.Text = string.Empty;
			LblSupplyAmount.Content = string.Empty;
		}

		private bool IsInputDecimal(string amount)
		{
			string pattern = @"^-?\d+(\.\d+)?$";
			return System.Text.RegularExpressions.Regex.IsMatch(amount, pattern);
		}

		private void EmptyFields()
		{
			TxtInstructions.Text = string.Empty;
			recipeSuppliesDetails.Clear();
			FillDtgSelectedSupplies();
		}
	}
}

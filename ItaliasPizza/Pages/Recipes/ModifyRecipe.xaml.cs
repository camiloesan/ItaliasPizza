using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizza.Pages.Recipes
{
    public partial class ModifyRecipe : Page
    {
        private List<RecipeSupplyDetails> currentSupplies = new List<RecipeSupplyDetails>();
        private List<Supply> availableSupplies = new List<Supply>();
        private Supply selectedSupply;
        private Guid currentProductId = Guid.Empty;
        private Guid recipeId = Guid.Empty;
        private string recipeInstructions = string.Empty;

        public ModifyRecipe(Guid productId)
        {
            var recipe = RecipeOperations.GetRecipeByProductId(productId);
            if (recipe == null)
            {
                MessageBox.Show("No se encontró la receta.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.MainWindow.Content = new Products();
                return;
            }

            InitializeComponent();
            currentProductId = productId;
            recipeId = recipe.IdRecipe;
            recipeInstructions = recipe.Instructions;
            TxtInstructions.Text = recipeInstructions;
            currentSupplies = RecipeOperations.GetRecipeSuppliesDetailsByIdProduct(productId);
            availableSupplies = SupplyOperations.GetSupplies();
            DtgAvailableSupplies.ItemsSource = availableSupplies;
            DtgSelectedSupplies.ItemsSource = currentSupplies;

            CheckEqualSupplies();
        }

        private void CheckEqualSupplies()
        {
            foreach (var item in currentSupplies)
            {
                foreach (var supply in availableSupplies)
                {
                    if (item.IdSupply == supply.IdSupply)
                    {
                        availableSupplies.Remove(supply);
                        break;
                    }
                }
            }
        }

        private void BtnSaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (currentSupplies.Count == 0)
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

            if (messageBoxResult == MessageBoxResult.No || messageBoxResult == MessageBoxResult.None)
            {
                return;
            }

            int modifiedRecipeResult = 0;
            List<RecipeSupply> newRecipeSupplies = new List<RecipeSupply>();

            Recipe recipeToModify = new Recipe
            {
                IdRecipe = recipeId,
                IdProduct = currentProductId,
                Instructions = TxtInstructions.Text
            };

            foreach (var item in currentSupplies)
            {
                newRecipeSupplies.Add(new RecipeSupply
                {
                    IdRecipeSupply = Guid.NewGuid(),
                    IdRecipe = recipeToModify.IdRecipe,
                    IdSupply = item.IdSupply,
                    SupplyAmount = item.SupplyAmount,
                    IdMeasurementUnit = item.IdMeasurementUnit
                });
            }
            modifiedRecipeResult = RecipeOperations.UpdateRecipeWithSupplies(recipeToModify, newRecipeSupplies);

            if (modifiedRecipeResult > 0)
            {
                MessageBox.Show("Receta registrada exitosamente.", "Registro exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                // condicional para cocinero o gerente
                Application.Current.MainWindow.Content = new Products();
            }
            else
            {
                MessageBox.Show("Ocurrió un error al registrar la receta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelRegistration_Click(object sender, RoutedEventArgs e)
        {
            // condicional para cocinero o gerente
            Application.Current.MainWindow.Content = new Products();
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

                RecipeSupplyDetails newRecipeSupply = new RecipeSupplyDetails
                {
                    IdSupply = selectedSupply.IdSupply,
                    SupplyName = selectedSupply.Name,
                    SupplyAmount = 0,
                    MeasurementUnit = SupplyOperations.GetMeasurementUnitById(selectedSupply.IdMeasurementUnit).MeasurementUnit1,
                    IdMeasurementUnit = selectedSupply.IdMeasurementUnit
                };

                

                RefreshSelectedSupplies();
                RefreshAvailableSupplies();

            }
            else
            {
                MessageBox.Show("El ingrediente ya ha sido seleccionado.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRemoveRecipeSupply_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var selectedSupplyDetails = button.DataContext as RecipeSupplyDetails;

            availableSupplies.Add(new Supply
            {
                IdSupply = selectedSupplyDetails.IdSupply,
                Name = selectedSupplyDetails.SupplyName,
                IdMeasurementUnit = selectedSupplyDetails.IdMeasurementUnit
            });
            currentSupplies.Remove(selectedSupplyDetails);

            RefreshSelectedSupplies();
            RefreshAvailableSupplies();
        }

        private void RefreshSelectedSupplies()
        {
            DtgSelectedSupplies.ItemsSource = null;
            DtgSelectedSupplies.ItemsSource = currentSupplies;
        }

        private void RefreshAvailableSupplies()
        {
            DtgAvailableSupplies.ItemsSource = null;
            DtgAvailableSupplies.ItemsSource = availableSupplies;
        }

        private void BtnSaveSupplyAmount_Click(object sender, RoutedEventArgs e)
        {
            var supplyAmount = TxtSupplyAmount.Text;

            if (string.IsNullOrEmpty(supplyAmount))
            {
                MessageBox.Show("Por favor, ingrese la cantidad de ingrediente.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (supplyAmount == "0")
            {
                MessageBox.Show("La cantidad de ingrediente no puede ser 0.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (!IsInputDecimal(supplyAmount))
            {
                MessageBox.Show("Ingrese una cantidad valida", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                var supplyMeasurementUnit = SupplyOperations.GetMeasurementUnitById(selectedSupply.IdMeasurementUnit);
                var newRecipeSupply = new RecipeSupplyDetails
                {
                    IdSupply = selectedSupply.IdSupply,
                    SupplyName = selectedSupply.Name,
                    SupplyAmount = decimal.Parse(supplyAmount),
                    MeasurementUnit = supplyMeasurementUnit.MeasurementUnit1,
                    IdMeasurementUnit = selectedSupply.IdMeasurementUnit
                };
                availableSupplies.Remove(selectedSupply);
                currentSupplies.Add(newRecipeSupply);

                RefreshSelectedSupplies();
                RefreshAvailableSupplies();
                ResetAmountForm();
            }
        }

        private void ResetAmountForm()
        {
            SupplyAmountForm.Visibility = Visibility.Hidden;
            BtnSaveRecipe.IsEnabled = true;
            BtnCancelRegistration.IsEnabled = true;
            TxtSupplyAmount.Text = string.Empty;
            LblSupplyAmount.Content = string.Empty;
        }

        private void BtnCancelSupplyAmount_Click(object sender, RoutedEventArgs e)
        {
            ResetAmountForm();
        }

        private bool IsInputDecimal(string amount)
        {
            string pattern = @"^-?\d+(\.\d+)?$";
            return System.Text.RegularExpressions.Regex.IsMatch(amount, pattern);
        }

        private bool IsSupplyAlreadySelected(Supply newSelectedSupply)
        {
            bool isAlreadySelected = false;

            var idSelectedSupply = newSelectedSupply.IdSupply;

            foreach (var item in currentSupplies)
            {

                if (item.IdSupply == idSelectedSupply)
                {
                    isAlreadySelected = true;
                    break;
                }
            }

            return isAlreadySelected;
        }
    }
}

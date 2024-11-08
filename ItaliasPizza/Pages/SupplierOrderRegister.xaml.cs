using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;

namespace ItaliasPizza.Pages
{
    /// <summary>
    /// Interaction logic for SupplierOrderRegister.xaml
    /// </summary>
    public partial class SupplierOrderRegister : Page
    {
        private const int KILOGRAMS_ID = 1;
        private const int UNITS_ID = 2;
        private const int LITERS_ID = 3;

        public SupplierOrderRegister()
        {
            InitializeComponent();
            CbSupplier.ItemsSource = GetSuppliers();
        }

        private List<Supplier> GetSuppliers()
        {
            return SupplierOperations.GetAllSuppliers();
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(DtpOrder.Text)
                && !string.IsNullOrEmpty(DtpEstimatedArrival.Text)
                && !string.IsNullOrEmpty(CbSupplier.Text);
        }

        //private bool IsQuantityValid()
        //{
        //    string pattern = @"^\d+(\.\d+)?$";
        //    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        //    return regex.IsMatch(TxtAmount.Text);
        //}

        private bool IsDateValid()
        {
            return DtpOrder.SelectedDate < DtpEstimatedArrival.SelectedDate;
        }

        private void ResetForm()
        {
            DtpOrder.Text = string.Empty;
            DtpEstimatedArrival.Text = string.Empty;
            //TxtAmount.Text = string.Empty;
        }

        private void FillDtgAvailableSupplies(Guid supplierId)
        {
            var availableSupplies = SupplyOperations.GetSuppliesByCategoriesOfSupplier(supplierId);

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

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos deben contener información");
            }
            else if (!IsDateValid())
            {
                MessageBox.Show("Las fecha estimada de llegada debe ser posterior a la fecha del pedido");
            }
            else
            {
                Supplier supplier = (Supplier)CbSupplier.SelectedItem;
                //Supply supply = (Supply)CbSupply.SelectedItem;
                //MeasurementUnit measurementUnit = (MeasurementUnit)CbMeasurementUnit.SelectedItem;

                //SupplierOrder supplierOrder = new SupplierOrder
                //{
                //    IdSupplierOrder = Guid.NewGuid(),
                //    IdSupplier = supplier.IdSupplier,
                //    IdSupply = supply.IdSupply,
                //    OrderDate = (DateTime)DtpOrder.SelectedDate,
                //    ExpectedDate = (DateTime)DtpEstimatedArrival.SelectedDate,
                //    ArrivalDate = (DateTime)DtpEstimatedArrival.SelectedDate,
                //    IdOrderStatus = 1
                //};

                //int result = SupplierOrderOperations.SaveSupplierOrder(supplierOrder);

                //if (result == 0)
                //{
                //    MessageBox.Show("No se pudo registrar el pedido a proveedor, inténtalo de nuevo más tarde");
                //    return;
                //}
                //else
                //{
                //    MessageBox.Show("Pedido a proveedor registrado exitosamente");
                //    ResetForm();
                //}
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }

        private void CbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Supplier supplier = (Supplier)CbSupplier.SelectedItem;

            FillDtgAvailableSupplies(supplier.IdSupplier);
        }

        private void CbSupply_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Supply supply = (Supply)CbSupply.SelectedItem;

            //switch (supply?.IdSupplyCategory)
            //{
            //    case 2:
            //        CbMeasurementUnit.SelectedIndex = LITERS_ID - 1;
            //        break;
            //    case 8:
            //        CbMeasurementUnit.SelectedIndex = LITERS_ID - 1;
            //        break;
            //    case 9:
            //        CbMeasurementUnit.SelectedIndex = UNITS_ID - 1;
            //        break;
            //    default:
            //        CbMeasurementUnit.SelectedIndex = KILOGRAMS_ID - 1;
            //        break;
            //}
        }

        private void BtnAddRecipeSupply_Click(object sender, RoutedEventArgs e)
        {
            //Button button = sender as Button;
            //selectedSupply = button.DataContext as Supply;

            //if (!IsSupplyAlreadySelected(selectedSupply))
            //{
            //    BtnSaveRecipe.IsEnabled = false;
            //    BtnCancelRegistration.IsEnabled = false;
            //    SupplyAmountForm.Visibility = Visibility.Visible;
            //    LblSupplyAmount.Content = SupplyOperations.GetMeasurementUnitById(selectedSupply.IdMeasurementUnit).MeasurementUnit1;

            //}
            //else
            //{
            //    MessageBox.Show("El ingrediente ya ha sido seleccionado.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
        }

        private void BtnRemoveRecipeSupply_Click(object sender, RoutedEventArgs e)
        {
            //Button button = sender as Button;
            //var selectedSupplyDetails = button.DataContext as RecipeSupplyDetails;

            //recipeSuppliesDetails.Remove(selectedSupplyDetails);
            //FillDtgSelectedSupplies();
        }
    }
}

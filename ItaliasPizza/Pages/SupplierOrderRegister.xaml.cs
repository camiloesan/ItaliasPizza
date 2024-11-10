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
        private List<OrderedSupplyDetails> suppliesDetails = new List<OrderedSupplyDetails>();
        private Supply selectedSupply;

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

        private bool IsDateValid()
        {
            return DtpOrder.SelectedDate < DtpEstimatedArrival.SelectedDate;
        }

        private void ResetForm()
        {
            DtpOrder.Text = string.Empty;
            DtpEstimatedArrival.Text = string.Empty;
            suppliesDetails = new List<OrderedSupplyDetails>();
            FillDtgSelectedSupplies();
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
        private void FillDtgSelectedSupplies()
        {
            var items = new ObservableCollection<OrderedSupplyDetails>(suppliesDetails);
            DtgSelectedSupplies.ItemsSource = items;
        }

        private bool IsInputDecimal(string amount)
        {
            string pattern = @"^-?\d+(\.\d+)?$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(amount);
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
                var newOrderedSupply = new OrderedSupplyDetails
                {
                    IdSupply = selectedSupply.IdSupply,
                    SupplyName = selectedSupply.Name,
                    SupplyAmount = decimal.Parse(supplyAmount),
                    MeasurementUnit = supplyMeasurementUnit.MeasurementUnit1,
                    IdMeasurementUnit = selectedSupply.IdMeasurementUnit
                };
                suppliesDetails.Add(newOrderedSupply);
                FillDtgSelectedSupplies();
                ResetAmountForm();
            }
        }

        private void BtnCancelSupplyAmount_Click(object sender, RoutedEventArgs e)
        {
            ResetAmountForm();
        }

        private void ResetAmountForm()
        {
            SupplyAmountForm.Visibility = Visibility.Hidden;
            BtnSave.IsEnabled = true;
            BtnCancel.IsEnabled = true;
            TxtSupplyAmount.Text = string.Empty;
            LblSupplyAmount.Content = string.Empty;
        }
        private bool IsSupplyAlreadySelected(Supply newSelectedSupply)
        {
            bool isAlreadySelected = false;

            var idSelectedSupply = newSelectedSupply.IdSupply;

            foreach (var item in suppliesDetails)
            {

                if (item.IdSupply == idSelectedSupply)
                {
                    isAlreadySelected = true;
                    break;
                }
            }

            return isAlreadySelected;
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
            } else if (suppliesDetails.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione al menos un insumo.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Supplier supplier = (Supplier)CbSupplier.SelectedItem;
                List<SupplierOrder> orders = new List<SupplierOrder>();
                List<OrderedSupply> orderedSupplies = new List<OrderedSupply>();
                var orderIdentifier = Guid.NewGuid();
                int result = 0;

                foreach (var item in suppliesDetails)
                {
                    orders.Add(new SupplierOrder
                    {
                        IdSupplierOrder = Guid.NewGuid(),
                        IdSupplier = supplier.IdSupplier,
                        IdSupply = item.IdSupply,
                        OrderDate = (DateTime)DtpOrder.SelectedDate,
                        ExpectedDate = (DateTime)DtpEstimatedArrival.SelectedDate,
                        ArrivalDate = (DateTime)DtpEstimatedArrival.SelectedDate,
                        IdOrderStatus = 1
                    });
                }

                foreach (var item in orders)
                {
                    result += SupplierOrderOperations.SaveSupplierOrder(item);
                    foreach (var supplyDetails in suppliesDetails)
                    {
                        if (item.IdSupply == supplyDetails.IdSupply)
                        {
                            orderedSupplies.Add(new OrderedSupply
                            {
                                IdSupply = item.IdSupply,
                                IdSupplierOrder = item.IdSupplierOrder,
                                OrderIdentifier = orderIdentifier,
                                Quantity = supplyDetails.SupplyAmount,
                                IdMeasurementUnit = supplyDetails.IdMeasurementUnit
                            });
                        }
                    }
                }

                if (result == 0)
                {
                    MessageBox.Show("No se pudo registrar el pedido a proveedor, inténtalo de nuevo más tarde");
                    return;
                }
                else
                {
                    MessageBox.Show("Pedido a proveedor registrado exitosamente");
                    SupplierOrderOperations.SaveOrderedSupply(orderedSupplies);
                    ResetForm();
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SupplierOrders();
        }

        private void CbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Supplier supplier = (Supplier)CbSupplier.SelectedItem;

            FillDtgAvailableSupplies(supplier.IdSupplier);
        }

        private void BtnAddSupply_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            selectedSupply = button.DataContext as Supply;

            if (!IsSupplyAlreadySelected(selectedSupply))
            {
                BtnSave.IsEnabled = false;
                BtnCancel.IsEnabled = false;
                SupplyAmountForm.Visibility = Visibility.Visible;
                LblSupplyAmount.Content = SupplyOperations.GetMeasurementUnitById(selectedSupply.IdMeasurementUnit).MeasurementUnit1;

            }
            else
            {
                MessageBox.Show("El ingrediente ya ha sido seleccionado.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRemoveSupply_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var selectedSupplyDetails = button.DataContext as OrderedSupplyDetails;

            suppliesDetails.Remove(selectedSupplyDetails);
            FillDtgSelectedSupplies();
        }
    }
}

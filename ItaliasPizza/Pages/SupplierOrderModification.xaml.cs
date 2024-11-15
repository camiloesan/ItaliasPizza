using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
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

namespace ItaliasPizza.Pages
{
    /// <summary>
    /// Interaction logic for SupplierOrderModification.xaml
    /// </summary>
    public partial class SupplierOrderModification : Page
    {
        private const int STATUS_ORDERED = 1;
        private const int STATUS_DELIVERED = 2;
        private const int STATUS_CANCELED = 4;
        private readonly SupplierOrderDetails supplierOrderDetails = new SupplierOrderDetails();
        private List<OrderedSupplyDetails> suppliesDetails = new List<OrderedSupplyDetails>();
        private Supply selectedSupply;
        public SupplierOrderModification(SupplierOrderDetails supplierOrderDetails)
        {
            InitializeComponent();
            this.supplierOrderDetails = supplierOrderDetails;
            suppliesDetails = SupplierOrderOperations.GetOrderedSupplyDetailsByOrderIdentifier(this.supplierOrderDetails.OrderIdentifier);
            ShowSupplierOrderInfo();
        }

        private void ShowSupplierOrderInfo()
        {
            DtpOrder.Text = supplierOrderDetails.OrderDate;
            DtpEstimatedArrival.Text = supplierOrderDetails.ExpectedDate;
            DtgSelectedSupplies.ItemsSource = suppliesDetails;
            DtgAvailableSupplies.ItemsSource = SupplyOperations.GetSuppliesByCategoriesOfSupplier(supplierOrderDetails.IdSupplier);
            if (supplierOrderDetails.Status.ToLower().Contains("cancelado") || supplierOrderDetails.Status.ToLower().Contains("entregado"))
            {
                BtnSave.IsEnabled = false;
                BtnCancelOrder.IsEnabled = false;
            }
        }

        private void FillDtgSelectedSupplies()
        {
            var items = new ObservableCollection<OrderedSupplyDetails>(suppliesDetails);
            DtgSelectedSupplies.ItemsSource = items;
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

        private bool AreFieldsFilled()
        {
            return DtpOrder.SelectedDate != null
                && DtpEstimatedArrival.SelectedDate != null;
        }

        private bool IsDateValid()
        {
            bool result;

            if (DtpArrival.SelectedDate == null)
            {
                result = DtpOrder.SelectedDate < DtpEstimatedArrival.SelectedDate;
            } else
            {
                result = DtpOrder.SelectedDate < DtpEstimatedArrival.SelectedDate
                    && DtpOrder.SelectedDate < DtpArrival.SelectedDate;
            }

            return result;
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

        private void BtnRemoveSupply_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var selectedSupplyDetails = button.DataContext as OrderedSupplyDetails;

            suppliesDetails.Remove(selectedSupplyDetails);
            FillDtgSelectedSupplies();
        }

        private void UpdateOrders(int orderStatus)
        {
            List<SupplierOrder> orders = new List<SupplierOrder>();
            List<OrderedSupply> orderedSupplies = new List<OrderedSupply>();
            var orderIdentifier = Guid.NewGuid();
            int result = 0;

            foreach (var item in suppliesDetails)
            {
                orders.Add(new SupplierOrder
                {
                    IdSupplierOrder = Guid.NewGuid(),
                    IdSupplier = supplierOrderDetails.IdSupplier,
                    IdSupply = item.IdSupply,
                    OrderDate = (DateTime)DtpOrder.SelectedDate,
                    ExpectedDate = (DateTime)DtpEstimatedArrival.SelectedDate,
                    ArrivalDate = (DateTime)DtpEstimatedArrival.SelectedDate,
                    IdOrderStatus = orderStatus
                });
            }

            result += SupplierOrderOperations.DeleteSupplierOrdersAndOrderedSupplies(supplierOrderDetails.OrderIdentifier);

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
                MessageBox.Show("No se pudo actualizar el pedido a proveedor, inténtalo de nuevo más tarde");
                return;
            }
            else
            {
                MessageBox.Show("Pedido a proveedor actualizado exitosamente");
                SupplierOrderOperations.SaveOrderedSupply(orderedSupplies);
                Application.Current.MainWindow.Content = new SupplierOrders();
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
                MessageBox.Show("Ninguna fecha puede ser inferior a la fecha del pedido");
            }
            else if (suppliesDetails.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione al menos un insumo.", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrEmpty(DtpArrival.Text) && DtpArrival.SelectedDate == null)
            {
                UpdateOrders(STATUS_ORDERED);
            } else
            {
                MessageBoxResult response = MessageBox.Show("¿Está seguro que desea guardar este pedido? (Al tener una fecha de entrega no podrá modificarlo nuevamente)",
                                                     "Advertencia",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Warning);
                if (response == MessageBoxResult.Yes)
                {
                    UpdateOrders(STATUS_DELIVERED);
                }
            }
        }

        private void Btn_CancelOrder(object sender, RoutedEventArgs e)
        {
            MessageBoxResult response = MessageBox.Show("¿Está seguro que desea cancela este pedido? (Podrá ver los detalles del pedido pero no podrá modificarlo nuevamente)",
                                                     "Advertencia",
                                                     MessageBoxButton.YesNo,
                                                     MessageBoxImage.Warning);
            if (response == MessageBoxResult.Yes)
            {
                int result = 0;

                result += SupplierOrderOperations.UpdateSupplierOrdersStatus(supplierOrderDetails.OrderIdentifier, STATUS_CANCELED);

                if (result == 0)
                {
                    MessageBox.Show("No se pudo cancelar el pedido a proveedor, inténtalo de nuevo más tarde");
                    return;
                }
                else
                {
                    MessageBox.Show("Pedido a proveedor cancelado exitosamente");
                    Application.Current.MainWindow.Content = new SupplierOrders();
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SupplierOrders();
        }
    }
}

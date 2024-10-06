using Database;
using ItaliasPizza.DataAccessLayer;
using System;
using System.Collections.Generic;
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
            CbMeasurementUnit.ItemsSource = GetMeasurementUnits();
            Supplier supplier = (Supplier)CbSupplier.SelectedItem;
            CbSupply.ItemsSource = SupplyOperations.GetSuppliesByCategory(supplier.IdSupplierCategory);
        }

        private List<Supplier> GetSuppliers()
        {
            return SupplierOperations.GetAllSuppliers();
        }

        private List<MeasurementUnit> GetMeasurementUnits()
        {
            return SupplyOperations.GetMeasurementUnits();
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(DtpOrder.Text)
                && !string.IsNullOrEmpty(DtpEstimatedArrival.Text)
                && !string.IsNullOrEmpty(CbSupplier.Text)
                && !string.IsNullOrEmpty(CbSupply.Text)
                && !string.IsNullOrEmpty(TxtAmount.Text)
                && !string.IsNullOrEmpty(CbMeasurementUnit.Text);
        }

        private bool IsQuantityValid()
        {
            string pattern = @"^\d+(\.\d+)?$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(TxtAmount.Text);
        }

        private bool IsDateValid()
        {
            return DtpOrder.SelectedDate < DtpEstimatedArrival.SelectedDate;
        }

        private void ResetForm()
        {
            DtpOrder.Text = string.Empty;
            DtpEstimatedArrival.Text = string.Empty;
            TxtAmount.Text = string.Empty;
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos deben contener información");
            } else if (!IsDateValid())
            {
                MessageBox.Show("Las fecha estimada de llegada debe ser posterior a la fecha del pedido");
            } else if (!IsQuantityValid())
            {
                MessageBox.Show("La cantidad solo debe contener números (con decimales si lo desea)");
            } else
            {
                Supplier supplier = (Supplier)CbSupplier.SelectedItem;
                Supply supply = (Supply)CbSupply.SelectedItem;
                MeasurementUnit measurementUnit = (MeasurementUnit)CbMeasurementUnit.SelectedItem;

                SupplierOrder supplierOrder = new SupplierOrder
                {
                    IdSupplierOrder = Guid.NewGuid(),
                    IdSupplier = supplier.IdSupplier,
                    IdSupply = supply.IdSupply,
                    OrderDate = (DateTime)DtpOrder.SelectedDate,
                    ExpectedDate = (DateTime)DtpEstimatedArrival.SelectedDate,
                    ArrivalDate = (DateTime)DtpEstimatedArrival.SelectedDate,
                    IdOrderStatus = 1
                };

                int result = SupplierOrderOperations.SaveSupplierOrder(supplierOrder);

                if (result == 0)
                {
                    MessageBox.Show("No se pudo registrar el pedido a proveedor, inténtalo de nuevo más tarde");
                    return;
                }
                else
                {
                    MessageBox.Show("Pedido a proveedor registrado exitosamente");
                    ResetForm();
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }

        private void CbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Supplier supplier = (Supplier)CbSupplier.SelectedItem;
            CbSupply.ItemsSource = SupplyOperations.GetSuppliesByCategory(supplier.IdSupplierCategory);

            switch (supplier.IdSupplierCategory)
            {
                case 2:
                    CbMeasurementUnit.SelectedIndex = LITERS_ID - 1;
                    break;
                case 8:
                    CbMeasurementUnit.SelectedIndex = LITERS_ID - 1;
                    break;
                case 9:
                    CbMeasurementUnit.SelectedIndex = UNITS_ID - 1;
                    break;
                default:
                    CbMeasurementUnit.SelectedIndex = KILOGRAMS_ID - 1;
                    break;
            }
        }
    }
}

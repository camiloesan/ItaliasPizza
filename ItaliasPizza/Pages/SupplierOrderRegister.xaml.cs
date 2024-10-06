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
        public SupplierOrderRegister()
        {
            InitializeComponent();
            CbSupplier.ItemsSource = GetSuppliers();
            CbSupply.ItemsSource = GetSupplies();
            CbMeasurementUnit.ItemsSource = GetMeasurementUnits();
        }

        private List<Supplier> GetSuppliers()
        {
            return SupplierOperations.GetAllSuppliers();
        }

        private List<Supply> GetSupplies()
        {
            return SupplyOperations.GetAllSupplies();
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
                && !string.IsNullOrEmpty(TxtAmount.Text);
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

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos deben contener información");
            } else if (IsDateValid())
            {
                MessageBox.Show("Las fecha estimada de llegada debe ser posterior a la fecha del pedido");
            } else if (!IsQuantityValid())
            {
                MessageBox.Show("La cantidad solo debe contener números (con decimales si lo desea)");
            } else
            {

            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}

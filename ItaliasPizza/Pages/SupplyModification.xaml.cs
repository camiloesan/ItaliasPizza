using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
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

namespace ItaliasPizza.Pages
{
    /// <summary>
    /// Interaction logic for SupplyModification.xaml
    /// </summary>
    public partial class SupplyModification : Page
    {
        private const int KILOGRAMS_ID = 1;
        private const int UNITS_ID = 2;
        private const int LITERS_ID = 3;

        public SupplyModification()
        {
            InitializeComponent();
            CbCategory.ItemsSource = GetCategories();
            CbMeasurementUnit.ItemsSource = GetMeasurementUnits();
            FillFields();
        }

        private List<SupplyCategory> GetCategories()
        {
            return SupplyOperations.GetSupplyCategories();
        }

        private List<MeasurementUnit> GetMeasurementUnits()
        {
            return SupplyOperations.GetMeasurementUnits();
        }

        private void FillFields()
        {
            Supply supply = SupplyOperations.GetSupplyById(Guid.Parse("5df88935-c789-4bca-a80a-68c1758d766f"));

            SupplyDetails.IdSupply = supply.IdSupply;
            SupplyDetails.Name = supply.Name;
            SupplyDetails.Amount = supply.Quantity.ToString();
            SupplyDetails.ExpirationDate = supply.ExpirationDate.ToString();

            TxtName.Text = SupplyDetails.Name;
            CbCategory.SelectedIndex = supply.IdSupplyCategory - 1;
            TxtAmount.Text = SupplyDetails.Amount;
            DtpExpiration.Text = SupplyDetails.ExpirationDate;
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtName.Text)
                && !string.IsNullOrEmpty(TxtAmount.Text)
                && !string.IsNullOrEmpty(DtpExpiration.Text)
                && !string.IsNullOrEmpty(CbCategory.Text);
        }

        private bool IsQuantityValid()
        {
            string pattern = @"^\d+(\.\d+)?$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(TxtAmount.Text);
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos deben contener información");
            }
            else if (!IsQuantityValid())
            {
                MessageBox.Show("La cantidad solo debe contener números (con decimales si lo desea)");
            }
            else
            {
                SupplyCategory supplyCategory = (SupplyCategory)CbCategory.SelectedItem;
                MeasurementUnit measurementUnit = (MeasurementUnit)CbMeasurementUnit.SelectedItem;

                Supply supply = new Supply
                {
                    IdSupply = SupplyDetails.IdSupply,
                    Name = TxtName.Text,
                    Quantity = decimal.Parse(TxtAmount.Text),
                    IdSupplyCategory = supplyCategory.IdSupplyCategory,
                    IdMeasurementUnit = measurementUnit.IdMeasurementUnit,
                    ExpirationDate = DtpExpiration.SelectedDate.Value,
                };

                bool result = SupplyOperations.UpdateSupplyInfo(supply);

                if (result)
                {
                    MessageBox.Show("Insumo modificado exitosamente");
                    FillFields();
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el insumo, inténtalo de nuevo más tarde");
                }
            }
        }

        private void Btn_Eliminate(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea eliminar este insumo?",
                                                      "Advertencia",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (SupplyOperations.UpdateSupplyStatus(SupplyDetails.IdSupply, false))
                {
                    MessageBox.Show("Insumo eliminado exitosamente");
                    Application.Current.MainWindow.Content = new Login();
                } else
                {
                    MessageBox.Show("No se pudo eliminar el insumo, inténtalo de nuevo más tarde");
                    Application.Current.MainWindow.Content = new Login();
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }

        private void CbMeasurementUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SupplyCategory supplyCategory = (SupplyCategory)CbCategory.SelectedItem;

            switch (supplyCategory.IdSupplyCategory)
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

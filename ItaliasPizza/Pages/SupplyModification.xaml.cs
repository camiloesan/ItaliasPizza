using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizza.Pages
{
    public partial class SupplyModification : Page
    {
        private const int KILOGRAMS_ID = 1;
        private const int UNITS_ID = 2;
        private const int LITERS_ID = 3;
        private readonly SupplyDetailsX supply;

        public SupplyModification(SupplyDetailsX supply)
        {
            this.supply = supply;
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
            TxtName.Text = supply.Name;
            CbCategory.SelectedIndex = supply.IdSupplyCategory - 1;
            TxtAmount.Text = supply.Quantity.Split(' ')[0] ;
            DtpExpiration.Text = supply.ExpirationDate;
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

                Supply supplyObj = new Supply
                {
                    IdSupply = supply.IdSupply,
                    Name = TxtName.Text,
                    Quantity = decimal.Parse(TxtAmount.Text),
                    IdSupplyCategory = supplyCategory.IdSupplyCategory,
                    IdMeasurementUnit = measurementUnit.IdMeasurementUnit,
                    ExpirationDate = DtpExpiration.SelectedDate.Value,
                };

                bool result = SupplyOperations.UpdateSupplyInfo(supplyObj);

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
                if (SupplyOperations.UpdateSupplyStatus(supply.IdSupply, false))
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
            Application.Current.MainWindow.Content = new Inventory();
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

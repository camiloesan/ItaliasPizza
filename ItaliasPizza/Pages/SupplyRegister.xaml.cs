﻿using Database;
using ItaliasPizza.DataAccessLayer;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Interaction logic for SupplyRegister.xaml
    /// </summary>
    public partial class SupplyRegister : Page
    {
        private const int KILOGRAMS_ID = 1;
        private const int UNITS_ID = 2;
        private const int LITERS_ID = 3;

        public SupplyRegister()
        {
            InitializeComponent();
            CbCategory.ItemsSource = GetCategories();
            CbMeasurementUnit.ItemsSource = GetMeasurementUnits();
        }

        private List<SupplyCategory> GetCategories()
        {
            return SupplyOperations.GetSupplyCategories();
        }

        private List<MeasurementUnit> GetMeasurementUnits()
        {
            return SupplyOperations.GetMeasurementUnits();
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

        private void ResetForm()
        {
            TxtName.Text = string.Empty;
            TxtAmount.Text = string.Empty;
            DtpExpiration.Text = string.Empty;
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
                    IdSupply = Guid.NewGuid(),
                    Name = TxtName.Text,
                    Quantity = decimal.Parse(TxtAmount.Text),
                    IdSupplyCategory = supplyCategory.IdSupplyCategory,
                    IdMeasurementUnit = measurementUnit.IdMeasurementUnit,
                    ExpirationDate = DtpExpiration.SelectedDate.Value,
                    Status = true,
                };

                int result = SupplyOperations.SaveSupply(supply);

                if (result == 0)
                {
                    MessageBox.Show("No se pudo registrar el insumo, inténtalo de nuevo más tarde");
                    return;
                }
                else
                {
                    MessageBox.Show("Insumo registrado exitosamente");
                    ResetForm();
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Inventory();
        }

        private void CbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

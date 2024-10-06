using Database;
using ItaliasPizza.DataAccessLayer;
using System;
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
        public SupplyRegister()
        {
            InitializeComponent();
            CbCategory.ItemsSource = GetCategories();
        }

        private List<SupplyCategory> GetCategories()
        {
            return SupplyOperations.GetSupplyCategoriesNames();
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
            CbCategory.Text = string.Empty;
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos deben contener información");
            }
            else if (!IsQuantityValid())
            {
                MessageBox.Show("El número solo debe contener 10 números");
            }
            else
            {
                var supplyId = Guid.NewGuid();
                string name = TxtName.Text;
                decimal amount = decimal.Parse(TxtAmount.Text);
                SupplyCategory supplyCategory = (SupplyCategory)CbCategory.SelectedItem;

                Supply supply = new Supply
                {
                    IdSupply = supplyId,
                    Name = name,
                    Quantity = amount,
                    IdSupplyCategory = supplyCategory.IdSupplyCategory,
                    //MeasurementUnit = DtpExpiration.Text,
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

        }
    }
}

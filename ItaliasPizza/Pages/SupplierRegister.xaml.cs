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

namespace ItaliasPizza.Pages
{
    /// <summary>
    /// Interaction logic for SupplierRegister.xaml
    /// </summary>
    public partial class SupplierRegister : Page
    {
        public SupplierRegister()
        {
            InitializeComponent();
        }

        private List<SupplyCategory> GetCategories()
        {
            return SupplyOperations.GetSupplyCategories();
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtName.Text)
                && !string.IsNullOrEmpty(TxtPhone.Text);
        }

        private bool IsPhoneValid()
        {
            string pattern = @"^\d{10}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(TxtPhone.Text);
        }

        private bool IsNumOfCategoriesValid()
        {
            string pattern = @"^[1-9]$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(TxtCategoryCount.Text);
        }

        private void ResetForm()
        {
            TxtName.Text = string.Empty;
            TxtPhone.Text = string.Empty;
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos deben contener información");
            }
            else if (!IsPhoneValid())
            {
                MessageBox.Show("El número solo debe contener 10 números");
            }else if (!IsNumOfCategoriesValid())
            {
                MessageBox.Show("La cantidad de proveedores solo deben ser números del 1 al 9");
            }
            else
            {
                Supplier supplier = new Supplier
                {
                    IdSupplier = Guid.NewGuid(),
                    Name = TxtName.Text,
                    Phone = TxtPhone.Text,
                };

                int result = SupplierOperations.SaveSupplier(supplier);

                if (result == 0)
                {
                    MessageBox.Show("No se pudo registrar el proveedor, inténtalo de nuevo más tarde");
                    return;
                }
                else
                {
                    MessageBox.Show("Proveedor registrado exitosamente");
                    ResetForm();
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }

        private void TxtCategoryCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            CategoryPanel.Children.Clear();

            if (int.TryParse(TxtCategoryCount.Text, out int categoryCount) && categoryCount > 0)
            {
                LblCategories.Content = "Categorías(*)";

                for (int i = 0; i < categoryCount; i++)
                {
                    ComboBox cb = new ComboBox
                    {
                        Name = $"CbCategory_{i + 1}",
                        DisplayMemberPath = "SupplyCategory1",
                        SelectedValuePath = "IdSupplyCategory",
                        IsEditable = false,
                        Width = 200,
                        Margin = new Thickness(5)
                    };

                    cb.ItemsSource = SupplyOperations.GetSupplyCategories();
                    CategoryPanel.Children.Add(cb);
                }
            } else
            {
                LblCategories.Content = "Categorías(*) Selecciona un número de proveedores";
            }
        }
    }
}
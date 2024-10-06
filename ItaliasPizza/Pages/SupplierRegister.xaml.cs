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
            ShowCategories();
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

        private void ResetForm()
        {
            TxtName.Text = string.Empty;
            TxtPhone.Text = string.Empty;
            TxtCategoryCount.Text = "1";
            ShowCategories();
        }

        private void ShowCategories()
        {
            CategoryPanel?.Children.Clear();
            if (int.TryParse(TxtCategoryCount.Text, out int categoryCount) && categoryCount > 0)
            {

                for (int i = 0; i < categoryCount; i++)
                {
                    ComboBox cb = new ComboBox
                    {
                        Name = $"CbCategory_{i}",
                        DisplayMemberPath = "SupplyCategory1",
                        SelectedValuePath = "IdSupplyCategory",
                        IsEditable = false,
                        Width = 200,
                        Margin = new Thickness(5)
                    };

                    cb.ItemsSource = GetCategories();
                    CategoryPanel?.Children.Add(cb);
                    cb.SelectedIndex = 0;
                }
            }
        }

        private void SaveSupplierCategories(Guid supplierId)
        {
            foreach (var child in CategoryPanel.Children)
            {
                if (child is ComboBox cb && cb.SelectedValue != null)
                {
                    SupplierSupplyCategory supplierSupplyCategory = new SupplierSupplyCategory
                    {
                        IdSupplier = supplierId,
                        IdSupplyCategory = (int)cb.SelectedValue
                    };

                    int result = SupplierOperations.SaveSupplierSuppliCategory(supplierSupplyCategory);

                    if (result == 0)
                    {
                        MessageBox.Show("No se pudo registrar la categoría, inténtalo de nuevo más tarde");
                    }
                }
            }
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
                    SaveSupplierCategories(supplier.IdSupplier);
                    ResetForm();
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SuppliersList();
        }

        private void TxtCategoryCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowCategories();
        }

        private void Btn_IncreaseCategory(object sender, RoutedEventArgs e)
        {
            int currentValue = int.Parse(TxtCategoryCount.Text);

            if (currentValue < 9)
            {
                TxtCategoryCount.Text = (currentValue + 1).ToString();
            } else
            {
                MessageBox.Show("Solo su puede un máximo de 9 categorías");
            }
        }

        private void Btn_DecreaseCategory(object sender, RoutedEventArgs e)
        {
            int currentValue = int.Parse(TxtCategoryCount.Text);

            if (currentValue > 1)
            {
                TxtCategoryCount.Text = (currentValue - 1).ToString();
            }
            else
            {
                MessageBox.Show("Debe haber al menos 1 categoría");
            }
        }
    }
}
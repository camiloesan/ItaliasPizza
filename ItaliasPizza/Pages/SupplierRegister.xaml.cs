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
            ShowCategories();
        }

        private void ShowCategories()
        {
            CbCategories.ItemsSource = GetCategories();
        }

        private int SaveSupplierCategories(Guid supplierId)
        {
            int result = 0;

            foreach (var item in CbCategories.Items)
            {
                ComboBoxItem comboBoxItem = (ComboBoxItem)CbCategories.ItemContainerGenerator.ContainerFromItem(item);

                if (comboBoxItem != null)
                {
                    CheckBox checkBox = FindVisualChild<CheckBox>(comboBoxItem);

                    if (checkBox != null && checkBox.IsChecked == true)
                    {
                        var supplyCategory = item as SupplyCategory;
                        SupplierSupplyCategory supplierCategory = new SupplierSupplyCategory
                        {
                            IdSupplier = supplierId,
                            IdSupplyCategory = supplyCategory.IdSupplyCategory
                        };
                        SupplierOperations.SaveSupplierSuppliCategory(supplierCategory);
                    }
                }
            }

            return result;
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
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
    }
}
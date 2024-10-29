using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        private List<SupplierCategory> supplierCategories;
        public SupplierRegister()
        {
            InitializeComponent();
            supplierCategories = SupplierOperations.GetAllSupplierCategories();
            ShowCategories();
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
            supplierCategories = SupplierOperations.GetAllSupplierCategories();
            ShowCategories();
        }

        private void ShowCategories()
        {
            LbCategories.ItemsSource = supplierCategories;
        }

        private int GetCheckedCategoriesCount()
        {
            int checkedCount = 0;

            foreach (var supplierCategory in supplierCategories)
            {
                if (supplierCategory != null && supplierCategory.IsSelected == true)
                {
                    checkedCount++;
                }
            }

            return checkedCount;
        }

        private void SaveSupplierCategories(Guid supplierId)
        {
            foreach (var supplierCategory in supplierCategories)
            {
                if (supplierCategory != null && supplierCategory.IsSelected == true)
                {
                    SupplierSupplyCategory supplierSupplyCategory = new SupplierSupplyCategory
                    {
                        IdSupplier = supplierId,
                        IdSupplyCategory = supplierCategory.Id
                    };
                    SupplierOperations.SaveSupplierSuppliCategory(supplierSupplyCategory);
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
            else if (GetCheckedCategoriesCount() == 0){
                MessageBox.Show("Selecciona al menos una categoría");
            } else
            {
                Supplier supplier = new Supplier
                {
                    IdSupplier = Guid.NewGuid(),
                    Name = TxtName.Text,
                    Phone = TxtPhone.Text,
                    Status = true
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
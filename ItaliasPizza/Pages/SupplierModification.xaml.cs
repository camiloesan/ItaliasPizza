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
    /// Interaction logic for SupplierModification.xaml
    /// </summary>
    public partial class SupplierModification : Page
    {
        private readonly Supplier supplier;
        private List<SupplierCategory> supplierCategories;
        public SupplierModification(Guid supplierId)
        {
            InitializeComponent();
            supplier = SupplierOperations.GetSupplierById(supplierId);
            supplierCategories = SupplierOperations.GetSupplierDetailsWithCategoriesBySpplierId(supplier.IdSupplier);
            ShowCategories();
            ShowSupplierInfo();
        }

        private void ShowSupplierInfo()
        {
            TxtName.Text = supplier.Name;
            TxtPhone.Text = supplier.Phone;

            if (supplier.Status)
            {
                BtnStatus.Content = "Desactivar";
            } else
            {
                BtnStatus.Content = "Activar";
            }
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

        private void ShowCategories()
        {
            LbCategories.ItemsSource = supplierCategories;
        }

        private void SaveSupplierCategories(Guid supplierId)
        {
            var supplierCategoryIds = SupplierOperations.GetCategoryIdsBySupplierId(supplierId);

            foreach (var supplierCategory in supplierCategories)
            {
                if (supplierCategory.IsSelected)
                {
                    if(!supplierCategoryIds.Contains(supplierCategory.Id))
                    {
                        SupplierSupplyCategory supplierSupplyCategory = new SupplierSupplyCategory
                        {
                            IdSupplier = supplierId,
                            IdSupplyCategory = supplierCategory.Id
                        };
                        SupplierOperations.SaveSupplierSuppliCategory(supplierSupplyCategory);
                    }               
                } else
                {
                    if (supplierCategoryIds.Contains(supplierCategory.Id))
                    {
                        SupplierSupplyCategory supplierSupplyCategory = new SupplierSupplyCategory
                        {
                            IdSupplier = supplierId,
                            IdSupplyCategory = supplierCategory.Id
                        };
                        SupplierOperations.DeleteSupplierSuppliCategory(supplierSupplyCategory);
                    }
                }
            }
        }

        private bool AreSupplierCategoriesEqual(Guid supplierId)
        {
            var supplierCategoryIds = SupplierOperations.GetCategoryIdsBySupplierId(supplierId);

            var selectedCategoryIds = supplierCategories
                .Where(category => category.IsSelected)
                .Select(category => category.Id)
                .ToList();

            return supplierCategoryIds.Count == selectedCategoryIds.Count
                   && !supplierCategoryIds.Except(selectedCategoryIds).Any();
        }

        private void ChangeSupplierStatus()
        {
            if(supplier.Status)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea desactivar este proveedor?",
                                                      "Advertencia",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    bool resultStatus = SupplierOperations.UpdateSupplierStatus(supplier.IdSupplier, false);
                    if (resultStatus)
                    {
                        MessageBox.Show("Proveedor desactivado exitosamente");
                        Application.Current.MainWindow.Content = new SuppliersList();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo desactivar el proveedor, inténtalo de nuevo más tarde");
                        Application.Current.MainWindow.Content = new SuppliersList();
                    }
                }
            } else
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro que desea activar este proveedor?",
                                                      "Advertencia",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    bool resultStatus = SupplierOperations.UpdateSupplierStatus(supplier.IdSupplier, true);
                    if (resultStatus)
                    {
                        MessageBox.Show("Proveedor activado exitosamente");
                        Application.Current.MainWindow.Content = new SuppliersList();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo activar el proveedor, inténtalo de nuevo más tarde");
                        Application.Current.MainWindow.Content = new SuppliersList();
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
            else if (GetCheckedCategoriesCount() == 0)
            {
                MessageBox.Show("Selecciona al menos una categoría");
            }
            else
            {
                Supplier supplierToUpdate = new Supplier
                {
                    IdSupplier = supplier.IdSupplier,
                    Name = TxtName.Text,
                    Phone = TxtPhone.Text,
                };

                bool result = SupplierOperations.UpdateSupplyInfo(supplierToUpdate);

                if (result || !AreSupplierCategoriesEqual(supplierToUpdate.IdSupplier))
                {
                    if (!AreSupplierCategoriesEqual(supplierToUpdate.IdSupplier))
                    {
                        SaveSupplierCategories(supplier.IdSupplier);
                    }
                    MessageBox.Show("Proveedor actualizadoado exitosamente");
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el proveedor, inténtalo de nuevo más tarde");
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SuppliersList();
        }
        private void Btn_Deactivate(object sender, RoutedEventArgs e)
        {
            ChangeSupplierStatus();
        }
    }
}

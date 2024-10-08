using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for SuppliersList.xaml
    /// </summary>
    public partial class SuppliersList : Page
    {
        public SuppliersList()
        {
            InitializeComponent();
            ShowSuppliers();
        }

        private void ShowSuppliers()
        {
            DtgSuppliers.ItemsSource = SupplierOperations.GetAllSuppliersWithCategories();
        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_RegisterNewSupplier(object sender, RoutedEventArgs e)
        {

        }
    }
}

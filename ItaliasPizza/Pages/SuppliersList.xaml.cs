using Database;
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

            var items = new ObservableCollection<Supplier>
            {
                new Supplier { Name = "Bryam", IdSupplierCategory = 1, Phone = "2282739074" },
            };

            DtgSuppliers.ItemsSource = items;
        }

        private void Btn_Filter(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_RegisterNewSupplier(object sender, RoutedEventArgs e)
        {

        }
    }
}

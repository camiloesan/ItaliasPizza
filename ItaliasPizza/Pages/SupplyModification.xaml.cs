using Database;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SupplyModification.xaml
    /// </summary>
    public partial class SupplyModification : Page
    {
        public SupplyModification()
        {
            InitializeComponent();
            CbCategory.ItemsSource = GetCategories();
        }

        private List<String> GetCategories()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.SupplyCategory.Select(c => c.SupplyCategory1).ToList();
            }
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtName.Text)
                && !string.IsNullOrEmpty(TxtAmount.Text)
                && !string.IsNullOrEmpty(DtpExpiration.Text)
                && !string.IsNullOrEmpty(CbCategory.Text);
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos deben contener información");
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

            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}

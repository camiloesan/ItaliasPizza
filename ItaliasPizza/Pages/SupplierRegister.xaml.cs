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
            CbCategory.ItemsSource = GetCategories();
        }

        private List<SupplyCategory> GetCategories()
        {
            return SupplyOperations.GetSupplyCategoriesNames();
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtName.Text)
                && !string.IsNullOrEmpty(CbCategory.Text)
                && !string.IsNullOrEmpty(TxtPhone.Text);
        }

        private bool IsPhoneValid()
        {
            string pattern = @"^\d{10}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(TxtPhone.Text);
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
                
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}

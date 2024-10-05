using Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizza.Pages
{
    public partial class ProductRegister : Page
    {
        public ProductRegister()
        {
            InitializeComponent();
            CbProductType.ItemsSource = GetProductTypes();
        }

        public List<ProducType> GetProductTypes()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.ProducType.ToList();
            }
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}

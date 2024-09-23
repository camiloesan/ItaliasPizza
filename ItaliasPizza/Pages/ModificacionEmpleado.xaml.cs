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
    /// Interaction logic for ModificacionEmpleado.xaml
    /// </summary>
    public partial class ModificacionEmpleado : Page
    {
        public ModificacionEmpleado()
        {
            InitializeComponent();
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            // todo return previous page
        }

        private void FillFields()
        {
            // todo fill fields with data from db
        }
    }
}

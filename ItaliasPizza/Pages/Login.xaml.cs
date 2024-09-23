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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void EnterKeyEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                //Application.Current.MainWindow.Content = new MainMenu();
                //todo
            }
        }

        private void BtnLoginEvent(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new RegistroEmpleado();
        }

        private void BtnCancelEvent(object sender, RoutedEventArgs e)
        {

        }

        private void LblResetPasswordEvent(object sender, MouseEventArgs e)
        {
            //todo
        }
    }
}

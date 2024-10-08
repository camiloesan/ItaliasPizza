using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ItaliasPizza.Pages
{
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
                LogIn();
            }
        }

        private bool AreFieldsEmpty()
        {
            return string.IsNullOrEmpty(TBoxEmail.Text) || string.IsNullOrEmpty(TBoxPassword.Password);
        }

        private void LogIn()
        {
            if (AreFieldsEmpty())
            {
                MessageBox.Show("Por favor, ingrese un usuario válido.");
                return;
            }

            string email = TBoxEmail.Text;
            bool isUserValid = AccessAccountOperations.AreCredentialsValid(email, TBoxPassword.Password);
            if (isUserValid) 
            {
                var account = AccessAccountOperations.GetAccessAccountByEmail(email);
                SessionDetails.UserId = account.IdAccessAccount;
                SessionDetails.UserType = AccessAccountOperations.GetEmployeeCharge(email);
                SessionDetails.IdEmployee = AccessAccountOperations.GetEmployeeId(email);
                ChangePageByUser(SessionDetails.UserType);
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }

        private void ChangePageByUser(string userType)
        {
            switch (userType) {
                case "Gerente":
                    Application.Current.MainWindow.Content = new EmployeeModification(SessionDetails.IdEmployee);
                    break;
                case "Cajero":
                    //Application.Current.MainWindow.Content = new MainMenu();
                    break;
                case "Cocinero":
                    //Application.Current.MainWindow.Content = new MainMenu();
                    break;
                case "Repartidor":
                    //Application.Current.MainWindow.Content = new MainMenu();
                    break;
                case "Mesero":
                    //Application.Current.MainWindow.Content = new MainMenu();
                    break;
                default:
                    MessageBox.Show("Usuario no válido.");
                    break;
            }
        }

        private void BtnLoginEvent(object sender, RoutedEventArgs e)
        {
            //LogIn();
            Application.Current.MainWindow.Content = new SuppliersList();
        }
    }
}

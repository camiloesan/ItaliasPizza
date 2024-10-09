using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizza.Pages
{
    public partial class Employees : Page
    {
        public Employees()
        {
            InitializeComponent();
            ShowEmployees();
        }

        public void ShowEmployees()
        {
            DtgEmployees.ItemsSource = EmployeeOperations.GetEmployeeInfo();
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button.DataContext is EmployeeDetails selectedEmployee)
            {
                Application.Current.MainWindow.Content = new EmployeeModification(selectedEmployee.IdEmployee);
            }
        }


        private void Btn_Filter(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_RegisterNewEmployee(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new EmployeeRegister();
        }
    }
}

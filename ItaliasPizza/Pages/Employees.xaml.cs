using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Pages.InventoryReport;
using ItaliasPizza.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliasPizza.Pages
{
    public partial class Employees : Page
    {
        private List<EmployeeDetails> employeeDetailsList = EmployeeOperations.GetEmployeeInfo();

        public Employees()
        {
            InitializeComponent();
            ShowEmployees();
            FillCbFilter();
        }

        public void ShowEmployees()
        {
            DtgEmployees.ItemsSource = employeeDetailsList;
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button.DataContext is EmployeeDetails selectedEmployee)
            {
                Application.Current.MainWindow.Content = new EmployeeModification(selectedEmployee.IdEmployee);
            }
        }


        private const string EMPLOYEE_NAME_ATTRIBUTE = "Nombre";
        private const string EMPLOYEE_LASTNAME_ATTRIBUTE = "Apellido";
        private const string EMPLOYEE_EMAIL_ATTRIBUTE = "Correo";
        private const string EMPLOYEE_PHONE_ATTRIBUTE = "Teléfono";
        private void FillCbFilter()
        {
            List<string> filters = new List<string>
            {
                EMPLOYEE_NAME_ATTRIBUTE,
                EMPLOYEE_LASTNAME_ATTRIBUTE,
                EMPLOYEE_EMAIL_ATTRIBUTE,
                EMPLOYEE_PHONE_ATTRIBUTE
            };

            CbFilter.ItemsSource = filters;
            CbFilter.SelectedIndex = 0;
        }


        private void Btn_Filter(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Search(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearcher.Text.ToLower();
            List<EmployeeDetails> filteredList = new List<EmployeeDetails>();

            switch (CbFilter.Text)
            {
                case EMPLOYEE_NAME_ATTRIBUTE:
                    filteredList = employeeDetailsList
                        .Where(s => s.FirstName.ToLower().Contains(searchText))
                        .ToList();
                    break;
                case EMPLOYEE_LASTNAME_ATTRIBUTE:
                    filteredList = employeeDetailsList
                        .Where(s => s.LastName.ToLower().Contains(searchText))
                        .ToList();
                    break;
                case EMPLOYEE_EMAIL_ATTRIBUTE:
                    filteredList = employeeDetailsList
                        .Where(s => s.Email.ToLower().Contains(searchText))
                        .ToList();
                    break;
                case EMPLOYEE_PHONE_ATTRIBUTE:
                    filteredList = employeeDetailsList
                        .Where(s => s.Phone.Contains(searchText))
                        .ToList();
                    break;
            }

            if (employeeDetailsList != filteredList && filteredList.Count > 0)
            {
                DtgEmployees.ItemsSource = filteredList;
            }
            else
            {
                MessageBox.Show("No existen coincidencias");
                DtgEmployees.ItemsSource = employeeDetailsList;
            }
        }

        private void Btn_RegisterNewEmployee(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new EmployeeRegister();
        }

        private void Btn_Employees(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Employees();
        }

        private void Btn_Supplies(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Inventory();
        }

        private void Btn_Orders(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Suppliers(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SuppliersList();
        }

        private void Btn_Reports(object sender, RoutedEventArgs e)
        {
			Application.Current.MainWindow.Content = new FinishInventoryReport();
		}

        private void Btn_Products(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Products();
        }
        private void Btn_SupplierOrders(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SupplierOrders();
        }

        private void Btn_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Login();
        }
    }
}

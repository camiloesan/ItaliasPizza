using Database;
using ItaliasPizza.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItaliasPizza.Pages
{
    public partial class ModificacionEmpleado : Page
    {
        public ModificacionEmpleado(Guid employeeId)
        {
            InitializeComponent();
            Employee employee = EmployeeOperations.GetEmployeeById(employeeId);
            FillFields(employee);
            CbCharge.ItemsSource = GetCharges();
        }

        private void UpdateEmployee(Guid employeeId)
        {
            HighlightInvalidFields();
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Por favor llene todos los campos");
                return;
            }
            else if (EmployeeOperations.IsPhoneRegistered(TxtPhone.Text))
            {
                MessageBox.Show("El número de teléfono ya está registrado, ingrese uno diferente");
                return;
            }
            else if (EmployeeOperations.IsEmailRegistered(TxtEmail.Text))
            {
                MessageBox.Show("El correo electrónico ya está registrado, ingrese uno diferente");
                return;
            }
            else if (!IsEmailValid(TxtEmail.Text))
            {
                MessageBox.Show("El correo electrónico no es válido, ingrese un formato válido");
                return;
            }
            else if (CbStatus.SelectedIndex == 0)
            {
                MessageBox.Show("Por favor seleccione un estado");
                return;
            }
            else
            {
                string name = TxtName.Text;
                string lastName = TxtLastName.Text;
                string phone = TxtPhone.Text;
                Charge charge = (Charge)CbCharge.SelectedItem;
                string email = TxtEmail.Text;
                string password = TxtPassword.Password;
                bool status = false;
                if (CbStatus.Text.Equals("Activo"))
                {
                    status = true;
                }
                else if (CbStatus.Text.Equals("Inactivo"))
                {
                    status = false;
                }

                Employee employee = new Employee
                {
                    IdEmployee = employeeId,
                    FirstName = name,
                    LastName = lastName,
                    Phone = phone,
                    Status = status,
                    IdCharge = charge.IdCharge
                };

                int resultEmployeeUpdate = EmployeeOperations.UpdateEmployee(employee);
                int resultAccountUpdate = EmployeeOperations.UpdateEmployeePasswordAndEmail(employeeId, password, email);

                if (resultEmployeeUpdate == 0 || resultAccountUpdate == 0)
                {
                    MessageBox.Show("No se pudo modificar al empleado, inténtalo de nuevo más tarde");
                    return;
                }
                else
                {
                    MessageBox.Show("Empleado modificado exitosamente");
                    ResetTextFormBorders();
                }
            }
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            // todo return previous page
        }

        private List<Charge> GetCharges()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Charge.ToList();
            }
        }

        private void FillFields(Employee employee)
        {
            string employeeEmail = EmployeeOperations.GetEmployeeEmail(employee.IdEmployee);
            TxtName.Text = employee.FirstName;
            TxtLastName.Text = employee.LastName;
            TxtPhone.Text = employee.Phone;
            TxtEmail.Text = employeeEmail;
            CbCharge.SelectedIndex = employee.IdCharge;
            TxtPassword.Password = AccessAccountOperations.GetEmployeePassword(employeeEmail);
            if (employee.Status)
            {
                CbStatus.SelectedIndex = 0;
            }
            else
            {
                CbStatus.SelectedIndex = 1;
            }
        }
        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtName.Text)
                && !string.IsNullOrEmpty(TxtLastName.Text)
                && !string.IsNullOrEmpty(TxtPhone.Text)
                && !string.IsNullOrEmpty(CbCharge.Text)
                && !string.IsNullOrEmpty(TxtEmail.Text)
                && !string.IsNullOrEmpty(TxtPassword.Password)
                && CbStatus.SelectedIndex != 0;
        }

        public bool IsEmailValid(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        private void HighlightInvalidFields()
        {
            string name = TxtName.Text.Trim();
            string lastName = TxtLastName.Text.Trim();
            string phone = TxtPhone.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string password = TxtPassword.Password.Trim();
            ResetTextFormBorders();

            bool isPhoneRegistered = EmployeeOperations.IsPhoneRegistered(phone);
            bool isEmailRegistered = EmployeeOperations.IsEmailRegistered(email);
            bool isEmailValid = IsEmailValid(email);

            if (string.IsNullOrEmpty(name))
            {
                TxtName.BorderBrush = Brushes.Red;
                TxtName.BorderThickness = new Thickness(2);
            }

            if (string.IsNullOrEmpty(lastName))
            {
                TxtLastName.BorderBrush = Brushes.Red;
                TxtLastName.BorderThickness = new Thickness(2);
            }

            if (string.IsNullOrEmpty(phone) || isPhoneRegistered)
            {
                TxtPhone.BorderBrush = Brushes.Red;
                TxtPhone.BorderThickness = new Thickness(2);
            }

            if (string.IsNullOrEmpty(email) || !isEmailValid || !isEmailRegistered)
            {
                TxtEmail.BorderBrush = Brushes.Red;
                TxtEmail.BorderThickness = new Thickness(2);
            }

            if (string.IsNullOrEmpty(password))
            {
                TxtPassword.BorderBrush = Brushes.Red;
                TxtPassword.BorderThickness = new Thickness(2);
            }
        }

        private void ResetTextFormBorders()
        {
            TxtName.BorderBrush = Brushes.Black;
            TxtName.BorderThickness = new Thickness(1);
            TxtLastName.BorderBrush = Brushes.Black;
            TxtLastName.BorderThickness = new Thickness(1);
            TxtPhone.BorderBrush = Brushes.Black;
            TxtPhone.BorderThickness = new Thickness(1);
            TxtEmail.BorderBrush = Brushes.Black;
            TxtEmail.BorderThickness = new Thickness(1);
            TxtPassword.BorderBrush = Brushes.Black;
            TxtPassword.BorderThickness = new Thickness(1);
            CbStatus.BorderBrush = Brushes.Black;
            CbStatus.BorderThickness = new Thickness(1);
            CbCharge.BorderBrush = Brushes.Black;
            CbCharge.BorderThickness = new Thickness(1);
        }
    }
}

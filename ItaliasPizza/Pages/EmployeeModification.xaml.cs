using Database;
using ItaliasPizza.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItaliasPizza.Pages
{
    public partial class EmployeeModification : Page
    {
        Guid guid = Guid.Parse("ce3b85b5-6c17-45ea-8bbb-e39e7cd1a662");
        readonly List<Charge> charges = new List<Charge>();
        readonly Employee employee;
        readonly string employeeEmail;

        public EmployeeModification(Guid employeeId)
        {
            InitializeComponent();
            employee = EmployeeOperations.GetEmployeeById(employeeId);
            employeeEmail = EmployeeOperations.GetEmployeeEmail(employeeId);
            this.guid = employeeId;
            charges = ChargesOperations.GetCharges();
            CbCharge.ItemsSource = charges;
            FillFields(employee);
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

        private void UpdateEmployee(Guid employeeId)
        {
            HighlightInvalidFields();
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Por favor llene todos los campos");
                return;
            }
            else if (!employee.Phone.Equals(TxtPhone.Text))
            {
                if (EmployeeOperations.IsPhoneRegistered(TxtPhone.Text))
                {
                    MessageBox.Show("El número de teléfono ya está registrado, ingrese uno diferente");
                    return;
                }
            }
            else if (!employeeEmail.Equals(TxtEmail.Text))
            {
                if (EmployeeOperations.IsEmailRegistered(TxtEmail.Text))
                {
                    MessageBox.Show("El correo electrónico ya está registrado, ingrese uno diferente");
                    return;
                }
            }
            else if (CbStatus.SelectedIndex == 0)
            {
                MessageBox.Show("Por favor seleccione un estado");
                return;
            }
            else
            {
                bool confirmSave = ConfirmSave();

                if (!confirmSave)
                {
                    return;
                }

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

                try
                {
                    EmployeeOperations.UpdateEmployee(employee);
                    EmployeeOperations.UpdateEmployeePasswordAndEmail(employeeId, password, email);
                } catch
                {
                    MessageBox.Show("No se pudo modificar al empleado en este momento, inténtelo de nuevo más tarde");
                }

                MessageBox.Show("Empleado modificado exitosamente");
                ResetTextFormBorders();
            }
        }

        private bool ConfirmSave()
        {
            MessageBoxResult result = MessageBox.Show(
                "¿Está seguro de que desea modificar al usuario?",
                "Confirmación",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Cancel)
            {
                return false;
            }

            return true;
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            UpdateEmployee(guid);
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Employees();
        }

        private void FillFields(Employee employee)
        {
            TxtName.Text = employee.FirstName;
            TxtLastName.Text = employee.LastName;
            TxtPhone.Text = employee.Phone;
            TxtEmail.Text = employeeEmail;

            int indexCharge = charges.IndexOf(charges.Find(a => a.IdCharge == employee.IdCharge));
            CbCharge.SelectedIndex = indexCharge;

            TxtPassword.Password = AccessAccountOperations.GetEmployeePassword(employeeEmail);
            if (employee.Status)
            {
                CbStatus.SelectedIndex = 1;
            }
            else
            {
                CbStatus.SelectedIndex = 2;
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

            if (string.IsNullOrEmpty(phone) || (!employee.Phone.Equals(phone) && isPhoneRegistered))
            {
                TxtPhone.BorderBrush = Brushes.Red;
                TxtPhone.BorderThickness = new Thickness(2);
            }

            if (string.IsNullOrEmpty(email) || !isEmailValid || (!employeeEmail.Equals(email) && isEmailRegistered))
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
    }
}

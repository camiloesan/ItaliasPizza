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
    public partial class RegistroEmpleado : Page
    {
        public RegistroEmpleado()
        {
            InitializeComponent();
            CbCharge.ItemsSource = GetCharges();
        }

        public RegistroEmpleado(bool _) {}

        public bool IsPasswordMatch(string password, string confirmedPassword)
        {
            return password == confirmedPassword;
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtName.Text)
                && !string.IsNullOrEmpty(TxtLastName.Text)
                && !string.IsNullOrEmpty(TxtPhone.Text)
                && !string.IsNullOrEmpty(CbCharge.Text)
                && !string.IsNullOrEmpty(TxtEmail.Text)
                && !string.IsNullOrEmpty(TxtPassword.Password)
                && !string.IsNullOrEmpty(TxtConfirmPassword.Password)
                && CbStatus.SelectedIndex != 0;
        }

        

        public bool IsEmailValid(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        private bool HighlightInvalidFields()
        {
            string name = TxtName.Text.Trim();
            string lastName = TxtLastName.Text.Trim();
            string phone = TxtPhone.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string password = TxtPassword.Password.Trim();
            string confirmPassword = TxtConfirmPassword.Password.Trim();
            ResetTextFormBorders();

            bool isPhoneRegistered = EmployeeOperations.IsPhoneRegistered(phone);
            bool isEmailRegistered = EmployeeOperations.IsEmailRegistered(email);
            bool isEmailValid = IsEmailValid(email);

            bool isValid = false;
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

            if (string.IsNullOrEmpty(password) || !password.Equals(confirmPassword))
            {
                TxtPassword.BorderBrush = Brushes.Red;
                TxtPassword.BorderThickness = new Thickness(2);
            }

            if (string.IsNullOrEmpty(confirmPassword) || !password.Equals(confirmPassword))
            {
                TxtConfirmPassword.BorderBrush = Brushes.Red;
                TxtConfirmPassword.BorderThickness = new Thickness(2);
            }

            return isValid;
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
            TxtConfirmPassword.BorderBrush = Brushes.Black;
            TxtConfirmPassword.BorderThickness = new Thickness(1);
            CbStatus.BorderBrush = Brushes.Black;
            CbStatus.BorderThickness = new Thickness(1);
            CbCharge.BorderBrush = Brushes.Black;
            CbCharge.BorderThickness = new Thickness(1);
        }

        

        public List<Charge> GetCharges()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Charge.ToList();
            }
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            HighlightInvalidFields();
            if (!IsPasswordMatch(TxtPassword.Password, TxtConfirmPassword.Password))
            {
                MessageBox.Show("Las contraseñas no coinciden");
                return;
            } 
            else if (!AreFieldsFilled())
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
                var employeeId = Guid.NewGuid();
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
                } else if (CbStatus.Text.Equals("Inactivo"))
                {
                    status = false;
                }

                Employee employee = new Employee { 
                    IdEmployee = employeeId, 
                    FirstName = name, 
                    LastName = lastName, 
                    Phone = phone, 
                    Status = status, 
                    IdCharge = charge.IdCharge
                };

                AccessAccount accessAccount = new AccessAccount
                {
                    UserName = name + lastName,
                    Password = password,
                    IdEmployee = employeeId,
                    Email = email,
                    Status = status
                };

                int result = EmployeeOperations.SaveEmployee(employee, accessAccount);
                if (result == 0)
                {
                    MessageBox.Show("No se pudo registrar al empleado, inténtalo de nuevo más tarde");
                    return;
                }
                else
                {
                    MessageBox.Show("Empleado registrado exitosamente");
                    ResetTextFormBorders();
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            // todo return to previous page or main menu
        }
    }
}

using Database;
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
    /// Interaction logic for RegistroEmpleado.xaml
    /// </summary>
    public partial class RegistroEmpleado : Page
    {
        public RegistroEmpleado()
        {
            InitializeComponent();
            CbCharge.ItemsSource = GetCharges();
        }

        public RegistroEmpleado(bool test) {}

        private bool IsPasswordMatch()
        {
            return TxtPassword.Password == TxtConfirmPassword.Password;
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtName.Text) 
                && !string.IsNullOrEmpty(TxtLastName.Text) 
                && !string.IsNullOrEmpty(TxtPhone.Text) 
                && !string.IsNullOrEmpty(TxtEmail.Text) 
                && !string.IsNullOrEmpty(TxtPassword.Password)
                && !string.IsNullOrEmpty(CbCharge.Text)
                && !string.IsNullOrEmpty(TxtConfirmPassword.Password);
        }

        private void SaveEmployee(Employee employee, AccessAccount accessAccount)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Add(employee);
                db.AccessAccount.Add(accessAccount);
                db.SaveChanges();
            }
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
            if (!IsPasswordMatch())
            {
                MessageBox.Show("Las contraseñas no coinciden");
                return;
            } 
            else if (!AreFieldsFilled())
            {
                MessageBox.Show("Por favor llene todos los campos");
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
                    Charge = charge 
                };

                AccessAccount accessAccount = new AccessAccount
                {
                    UserName = name + lastName,
                    Password = password,
                    IdEmployee = employeeId,
                    Email = email,
                    Status = status
                };

                SaveEmployee(employee, accessAccount);

                MessageBox.Show("Empleado registrado exitosamente");
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            // todo return to previous page or main menu
        }
    }
}

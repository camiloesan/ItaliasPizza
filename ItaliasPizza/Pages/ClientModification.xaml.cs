using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Pages.Clients;
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
    public partial class ClientModification : Page
    {
        private Client Client { get; set; }

        public ClientModification(Client client)
        {
            Client = client;
            FillFields(client);
            InitializeComponent();
        }

        private void FillFields(Client client)
        {
            TxtName.Text = client.FirstName;
            TxtLastName.Text = client.LastName;
            TxtPhone.Text = client.Phone;
        }

        private bool Confirmation()
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas guardar los cambios?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private void BtnCancelSaveClient_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new ViewClients();
        }

        private void BtnSaveClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientOperations.IsPhoneRegistered(TxtPhone.Text))
            {
                MessageBox.Show("El número de teléfono ya está registrado, intente otro número", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Confirmation())
            {
                var result = ClientOperations.UpdateClient(Client);
                if (result == 1)
                {
                    MessageBox.Show("Cliente actualizado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.MainWindow.Content = new ViewClients();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsInputNumber(string input)
        {
            string pattern = @"^\d+$";
            return System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
        }

        private void TxtNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsInputNumber(e.Text);
        }
    }
}

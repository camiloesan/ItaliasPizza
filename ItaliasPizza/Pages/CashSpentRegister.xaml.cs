using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CashSpentRegister.xaml
    /// </summary>
    public partial class CashSpentRegister : Page
    {
        private const int TRANSACTION_TYPE_SPENT = 2;

        public CashSpentRegister()
        {
            InitializeComponent();
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtDescription.Text)
                && !string.IsNullOrEmpty(TxtAmount.Text);
        }
        private bool IsInputDecimal(string amount)
        {
            string pattern = @"^-?\d+(\.\d+)?$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(amount);
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos obligatorios deben contener información", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (!IsInputDecimal(TxtAmount.Text))
            {
                MessageBox.Show("Ingrese una cantidad valida (Número y decimales)", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                int result = 0;

                Transaction transaction = new Transaction
                {
                    IdTransaction = Guid.NewGuid(),
                    IdTransactionType = TRANSACTION_TYPE_SPENT,
                    Date = DateTime.Now,
                    Amount = int.Parse(TxtAmount.Text),
                    Description = TxtDescription.Text,
                    RegisteredBy = SessionDetails.IdEmployee
                };

                result += TransactionOperations.SaveTransaction(transaction);

                if (result == 0)
                {
                    MessageBox.Show("No se pudo guardar la salida de dinero, inténtalo de nuevo más tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Salida de dinero guardada exitosamente", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                    TxtDescription.Text = string.Empty;
                    TxtAmount.Text = string.Empty;
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            //TODO: change page to Register spent button
        }
    }
}

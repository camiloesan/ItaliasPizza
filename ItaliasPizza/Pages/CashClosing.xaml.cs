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
    /// Interaction logic for CashClosing.xaml
    /// </summary>
    public partial class CashClosing : Page
    {
        private CashClosingDetails cashClosingDetails;
        public CashClosing()
        {
            InitializeComponent();
            cashClosingDetails = CashReconciliationOperations.GetCashClosingDetails();
            ShowCashInfo();
        }

        private void ShowCashInfo()
        {
            LblPreviousDayCash.Content += cashClosingDetails.PreviousDayCash.ToString();
            LblTotalSalesCash.Content += cashClosingDetails.TotalSalesCash.ToString();
            LblTotalSpentCash.Content += cashClosingDetails.TotalSpentCash.ToString();
            LblTotalCash.Content += ((cashClosingDetails.PreviousDayCash + cashClosingDetails.TotalSalesCash)-(cashClosingDetails.TotalSpentCash)).ToString();
        }

        private bool AreFieldsFilled()
        {
            return !string.IsNullOrEmpty(TxtTotalAmount.Text)
                && !string.IsNullOrEmpty(TxtNextDayCash.Text);
        }
        private bool IsInputDecimal(string amount)
        {
            string pattern = @"^-?\d+(\.\d+)?$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(amount);
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {
            bool cashClosingIsAlredyRegistered = CashReconciliationOperations.IsCashReconciliationRegisteredToday();

            if (!AreFieldsFilled())
            {
                MessageBox.Show("Todos los campos obligatorios deben contener información", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } else if(!IsInputDecimal(TxtTotalAmount.Text) || !IsInputDecimal(TxtNextDayCash.Text))
            {
                MessageBox.Show("Ingrese una cantidad valida en los campos obligatorios (Número y decimales)", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (cashClosingIsAlredyRegistered)
            {
                MessageBox.Show("Ya se ha registrado el corte de caja de hoy", "Alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
            MessageBoxResult response = MessageBox.Show("¿Está seguro que desea realizar el corte de caja? (No podrá modificarlo)",
                                                    "Advertencia",
                                                    MessageBoxButton.YesNo,
                                                    MessageBoxImage.Warning);
                if (response == MessageBoxResult.Yes)
                {
                    string observations;
                    int result = 0;

                    if (string.IsNullOrEmpty(TxtObservations.Text))
                    {
                        observations = "Sin observaciones";
                    }
                    else
                    {
                        observations = TxtObservations.Text;
                    }

                    CashReconciliation cashReconciliation = new CashReconciliation
                    {
                        IdCashReconciliation = Guid.NewGuid(),
                        OpeningDate = DateTime.Today,
                        ClosingDate = DateTime.Now,
                        StartingAmount = cashClosingDetails.PreviousDayCash,
                        FinishingAmount = decimal.Parse(TxtTotalAmount.Text),
                        SalesAmount = cashClosingDetails.TotalSalesCash,
                        SpendingsAmount = cashClosingDetails.TotalSpentCash,
                        CashDifference = decimal.Parse(TxtNextDayCash.Text),
                        Observations = observations,
                        RegisteredBy = SessionDetails.IdEmployee
                    };

                    result += CashReconciliationOperations.SaveCashReconciliation(cashReconciliation);

                    if (result == 0)
                    {
                        MessageBox.Show("No se pudo guardar el corte de caja, inténtalo de nuevo más tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Corte de caja guardado exitosamente", "Alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                        //TODO: change page to CashReconciliation button
                    }
                }
            }
        }

        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            //TODO: change page to CashReconciliation button
        }
    }
}

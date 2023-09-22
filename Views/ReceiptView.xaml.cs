using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPFApp.Models;
using WPFApp.Repositories;
using WPFApp.ViewModels;

namespace WPFApp.Views
{
    public partial class ReceiptView : UserControl
    {
        private ReceiptViewModel receiptViewModel;
        private ObservableCollection<ReceiptModel> filteredReceipts;

        public ReceiptView()
        {
            InitializeComponent();
            receiptViewModel = new ReceiptViewModel();
            filteredReceipts = new ObservableCollection<ReceiptModel>(receiptViewModel.Receipts);
            ReceiptGrid.ItemsSource = filteredReceipts;
        }

        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            FilterReceipts();
        }

        private void FilterReceipts()
        {
            filteredReceipts.Clear();

            string bloodType = BloodTypeCombo.Text;
            DateTime fromDate = FromDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime toDate = ToDatePicker.SelectedDate ?? DateTime.MaxValue;
            string process = ReceiptProcessCombo.Text;

            ReceiptRepository receiptRepository = new ReceiptRepository();

            // Retrieve receipts based on blood type
            var matchingReceiptsByBloodType = receiptRepository.GetReceiptsByBloodType(bloodType);

            foreach (ReceiptModel receipt in matchingReceiptsByBloodType)
            {
                // Filter receipts by date range and process
                if ((receipt.ReceiptDate >= fromDate && receipt.ReceiptDate <= toDate) &&
                    (receipt.Process == process || process == "All Receipts" || process == "--Select Process--"))
                {
                    filteredReceipts.Add(receipt);
                }
            }
        }

    }
}

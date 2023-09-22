using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.ViewModels
{
    public class ReceiptViewModel : ViewModelBase
    {
        private ObservableCollection<ReceiptModel> receipts;
        private ReceiptRepository receiptRepository;

        public ObservableCollection<ReceiptModel> Receipts
        {
            get { return receipts; }
            set
            {
                receipts = value;
            }
        }

        public ReceiptViewModel()
        {
            receiptRepository = new ReceiptRepository();
            LoadReceipts();
        }

        public void LoadReceipts()
        {
            Receipts = receiptRepository.GetReceipts();
        }
    }
}

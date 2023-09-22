using System.Windows;
using System.Windows.Controls;
using WPFApp.ViewModels;

namespace WPFApp.Views
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        public StockView()
        {
            InitializeComponent();
        }

        public void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected bag health
            string selectedBagHealth = HealthTypeCombo.Text;

            if (selectedBagHealth == "--Bag Health--")
                return;

            // Call the function with the selected bag health
            StockViewModel viewModel = (StockViewModel)DataContext;
            viewModel.GetStockItems(selectedBagHealth);
        }

    }
}

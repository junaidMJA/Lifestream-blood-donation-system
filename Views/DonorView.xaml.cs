using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPFApp.Models;
using WPFApp.Repositories;
using WPFApp.ViewModels;

namespace WPFApp.Views
{
    public partial class DonorView : UserControl
    {
        DonorViewModel donorViewModel = new DonorViewModel();

        public DonorView()
        {
            InitializeComponent();
            DonorGrid.ItemsSource = donorViewModel.Donor;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text?.Trim();

            if (searchText == "Search...")
            {
                searchText = string.Empty; // Exclude the "Search..." text from the filter
            }

            if (BloodTypeCombo.Text != "All Blood Types" && BloodTypeCombo.Text != null && BloodTypeCombo.Text != "--Select Type--")
            {
                DonorGrid.ItemsSource = donorViewModel.Donor
                    .Where(donor => donor.BloodType == BloodTypeCombo.Text &&
                        (string.IsNullOrEmpty(searchText) || donor.Name.ToLower().Contains(searchText.ToLower())))
                    .ToList();
            }
            else
            {
                DonorGrid.ItemsSource = donorViewModel.Donor
                    .Where(donor => string.IsNullOrEmpty(searchText) || donor.Name.ToLower().Contains(searchText.ToLower()))
                    .ToList();
            }
        }
      
    }

}

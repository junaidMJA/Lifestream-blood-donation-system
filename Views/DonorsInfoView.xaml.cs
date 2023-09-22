using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFApp.ViewModels;
using WPFApp.Repositories;

namespace WPFApp.Views
{
    public partial class DonorsInfoView : UserControl
    {
        private readonly DonorsInfoViewModel viewModel;
        private readonly DonorRepository donorRepository;

        public DonorsInfoView()
        {
            InitializeComponent();
            viewModel = (DonorsInfoViewModel)DataContext;
            donorRepository = new DonorRepository();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameTextBox.Text;
                DateTime dateOfBirth = DOBDatePicker.SelectedDate ?? DateTime.MinValue;
                string gender = ((ComboBoxItem)GenderCombo.SelectedItem)?.Content?.ToString();
                string bloodType = ((ComboBoxItem)BloodTypeCombo.SelectedItem)?.Content?.ToString();
                string contact = ContactTextBox.Text;
                string address = AddressTextBox.Text;
                string frequency = ((ComboBoxItem)FreqCombo.SelectedItem)?.Content?.ToString();
                DateTime lastDonated = LastDonatedDatePicker.SelectedDate ?? DateTime.MinValue;

                viewModel.AddDonor(name, dateOfBirth, gender, bloodType, contact, address, frequency, lastDonated);

                // Call the repository method to add the donor to the database
                donorRepository.AddDonor(name, dateOfBirth, gender, bloodType, contact, address, frequency, lastDonated);

                // Refresh the donor grid
                viewModel.RefreshDonors();

                ResetForm();
                CancelButton.IsEnabled = false;

                StatusTextBlock.Text = "Donor added successfully.";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#33ff00"));
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Failed to add donor. Error: {ex.Message}";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd152"));
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int donorId = int.Parse(DonorIdTextBox.Text);
                string name = NameTextBox.Text;
                DateTime dateOfBirth = DOBDatePicker.SelectedDate ?? DateTime.MinValue;
                string gender = ((ComboBoxItem)GenderCombo.SelectedItem)?.Content?.ToString();
                string bloodType = ((ComboBoxItem)BloodTypeCombo.SelectedItem)?.Content?.ToString();
                string contact = ContactTextBox.Text;
                string address = AddressTextBox.Text;
                string frequency = ((ComboBoxItem)FreqCombo.SelectedItem)?.Content?.ToString();
                DateTime lastDonated = LastDonatedDatePicker.SelectedDate ?? DateTime.MinValue;

                viewModel.UpdateDonor(donorId, name, dateOfBirth, gender, bloodType, contact, address, frequency, lastDonated);

                // Call the repository method to update the donor in the database
                donorRepository.UpdateDonor(donorId, name, dateOfBirth, gender, bloodType, contact, address, frequency, lastDonated);

                // Refresh the donor grid
                viewModel.RefreshDonors();

                StatusTextBlock.Text = "Donor updated successfully.";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#33ff00"));
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Failed to update donor. Error: {ex.Message}";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd152"));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int donorId = int.Parse(DonorIdTextBox.Text);
                viewModel.DeleteDonor(donorId);

                // Call the repository method to delete the donor from the database
                donorRepository.DeleteDonor(donorId);

                // Refresh the donor grid
                viewModel.RefreshDonors();

                StatusTextBlock.Text = "Donor deleted successfully.";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#33ff00"));
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Failed to delete donor. Error: {ex.Message}";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd152"));
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
            CancelButton.IsEnabled = false;
            StatusTextBlock.Text = "";
        }

        private void ResetForm()
        {
            NameTextBox.Text = "";
            DOBDatePicker.SelectedDate = null;
            GenderCombo.SelectedIndex = -1;
            BloodTypeCombo.SelectedIndex = -1;
            ContactTextBox.Text = "";
            AddressTextBox.Text = "";
            FreqCombo.SelectedIndex = -1;
            LastDonatedDatePicker.SelectedDate = null;
            DonorIdTextBox.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check if any of the TextBox controls have a non-empty value
            bool isAnyTextBoxEmpty = string.IsNullOrEmpty(NameTextBox.Text) ||
                                     string.IsNullOrEmpty(ContactTextBox.Text) ||
                                     string.IsNullOrEmpty(AddressTextBox.Text) ||
                                     string.IsNullOrEmpty(DonorIdTextBox.Text);

            // Enable or disable the Cancel button based on the condition
            CancelButton.IsEnabled = isAnyTextBoxEmpty;
            StatusTextBlock.Text = "";
        }
    }
}

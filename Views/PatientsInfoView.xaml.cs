using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFApp.ViewModels;
using WPFApp.Repositories;

namespace WPFApp.Views
{
    public partial class PatientsInfoView : UserControl
    {
        private readonly PatientsInfoViewModel viewModel;
        private readonly PatientRepository patientRepository;

        public PatientsInfoView()
        {
            InitializeComponent();
            viewModel = (PatientsInfoViewModel)DataContext;
            patientRepository = new PatientRepository();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameTextBox.Text;
                DateTime dateOfBirth = DOBDatePicker.SelectedDate ?? DateTime.MinValue;
                string bloodType = ((ComboBoxItem)BloodTypeCombo.SelectedItem)?.Content?.ToString();
                string gender = ((ComboBoxItem)GenderCombo.SelectedItem)?.Content?.ToString();
                string contact = ContactTextBox.Text;
                string frequency = ((ComboBoxItem)FreqCombo.SelectedItem)?.Content?.ToString();
                string address = AddressTextBox.Text;

                viewModel.AddPatient(name, dateOfBirth, bloodType, gender, contact, frequency, address);

                // Call the repository method to add the patient to the database
                patientRepository.AddPatient(name, dateOfBirth, bloodType, gender, contact, frequency, address);

                // Refresh the patient grid
                viewModel.RefreshPatients();

                ResetForm();
                CancelButton.IsEnabled = false;

                StatusTextBlock.Text = "Patient added successfully.";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#33ff00"));
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Failed to add patient. Error: {ex.Message}";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd152"));
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int patientId = int.Parse(PatientIdTextBox.Text);
                string name = NameTextBox.Text;
                DateTime dateOfBirth = DOBDatePicker.SelectedDate ?? DateTime.MinValue;
                string bloodType = ((ComboBoxItem)BloodTypeCombo.SelectedItem)?.Content?.ToString();
                string gender = ((ComboBoxItem)GenderCombo.SelectedItem)?.Content?.ToString();
                string contact = ContactTextBox.Text;
                string frequency = ((ComboBoxItem)FreqCombo.SelectedItem)?.Content?.ToString();
                string address = AddressTextBox.Text;

                viewModel.UpdatePatient(patientId, name, dateOfBirth, bloodType, gender, contact, frequency, address);

                // Call the repository method to update the patient in the database
                patientRepository.UpdatePatient(patientId, name, dateOfBirth, bloodType, gender, contact, frequency, address);

                // Refresh the patient grid
                viewModel.RefreshPatients();

                StatusTextBlock.Text = "Patient updated successfully.";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#33ff00"));
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Failed to update patient. Error: {ex.Message}";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd152"));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int patientId = int.Parse(PatientIdTextBox.Text);
                viewModel.DeletePatient(patientId);

                // Call the repository method to delete the patient from the database
                patientRepository.DeletePatient(patientId);

                // Refresh the patient grid
                viewModel.RefreshPatients();

                StatusTextBlock.Text = "Patient deleted successfully.";
                StatusTextBlock.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#33ff00"));
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Failed to delete patient. Error: {ex.Message}";
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
            BloodTypeCombo.SelectedIndex = -1;
            GenderCombo.SelectedIndex = -1;
            ContactTextBox.Text = "";
            FreqCombo.SelectedIndex = -1;
            AddressTextBox.Text = "";
            PatientIdTextBox.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check if any of the TextBox controls have a non-empty value
            bool isAnyTextBoxEmpty = string.IsNullOrEmpty(NameTextBox.Text) ||
                                     string.IsNullOrEmpty(ContactTextBox.Text) ||
                                     string.IsNullOrEmpty(AddressTextBox.Text) ||
                                     string.IsNullOrEmpty(PatientIdTextBox.Text);

            // Enable or disable the Cancel button based on the condition
            CancelButton.IsEnabled = isAnyTextBoxEmpty;
            StatusTextBlock.Text = "";
        }

    }
}

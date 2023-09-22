using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFApp.Models;
using WPFApp.Repositories;
using WPFApp.ViewModels;

namespace WPFApp.Views
{
    public partial class AppointmentsView : UserControl
    {
        private AppointmentsViewModel appointmentsViewModel;
        private ObservableCollection<AppointmentModel> filteredAppointments;

        public AppointmentsView()
        {
            InitializeComponent();
            appointmentsViewModel = new AppointmentsViewModel();
            filteredAppointments = new ObservableCollection<AppointmentModel>(appointmentsViewModel.Appointments);
            AppointmentsGrid.ItemsSource = filteredAppointments;
        }

        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            FilterAppointments();
        }

        private void FilterAppointments()
        {
            filteredAppointments.Clear();

            string searchQuery = SearchTextBox.Text.ToLower();
            string bloodType = BloodTypeCombo.Text;
            string appointmentStatus = AppointmentTypeCombo.Text;

            DonorRepository donorRepository = new DonorRepository();

            // Retrieve donors based on blood type and name
            var matchingDonors = donorRepository.GetDonorsByNameAndBloodType(searchQuery, bloodType);

            foreach (AppointmentModel appointment in appointmentsViewModel.Appointments)
            {
                bool matchesAppointmentStatus = string.IsNullOrEmpty(appointmentStatus) ||
                    appointmentStatus == "All Appointments" ||
                    appointmentStatus == "--Select Status--" ||
                    appointment.Status == appointmentStatus;

                bool isWithinDateRange = (!FromDatePicker.SelectedDate.HasValue || appointment.AppointmentDate.Date >= FromDatePicker.SelectedDate.Value.Date) &&
                    (!ToDatePicker.SelectedDate.HasValue || appointment.AppointmentDate.Date <= ToDatePicker.SelectedDate.Value.Date);

                bool isADonor = false;

                foreach (DonorModel donor in matchingDonors) 
                {
                    if (donor.ID == appointment.ID)
                    {
                        isADonor = true; break;
                    }
                }

                if (matchesAppointmentStatus && isWithinDateRange && isADonor)
                {
                    filteredAppointments.Add(appointment);
                }
            }
        }

    }
}

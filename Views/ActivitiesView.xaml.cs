using System;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.Views
{
    public partial class ActivitiesView : UserControl
    {
        public RepositoryBase repositoryBase;
        public AppointmentRepository appointmentRepository;
        public RequestRepository requestRepository;
        public ReceiptRepository receiptRepository;
        public UserRepository userRepository;

        public ActivitiesView()
        {
            InitializeComponent();
            repositoryBase = new RepositoryBase();
            appointmentRepository = new AppointmentRepository();
            requestRepository = new RequestRepository();
            receiptRepository = new ReceiptRepository();
            userRepository = new UserRepository();
        }

        private void CreateAppButton_Click(object sender, RoutedEventArgs e)
        {
            // Get input values
            int donorId;
            if (!int.TryParse(DonorIDTextBox.Text, out donorId))
            {
                SetStatusMessage("Please enter a valid Donor ID.", isError: true);
                return;
            }

            DateTime appointmentDate = (AppDatePicker.SelectedDate ?? DateTime.MinValue).Date; // Get the selected date and set the time to midnight
            string appointmentTime = AppTimeComboBox.Text;

            // Extract hours and minutes from the appointmentTime string

            string[] timeParts = appointmentTime.Split(':', ' ');
            int hours = int.Parse(timeParts[0]);
            int minutes = int.Parse(timeParts[1]);
            if (timeParts[2].Equals("PM", StringComparison.OrdinalIgnoreCase))
            {
                hours += 12;
            }

            // Create a new TimeSpan object with the extracted hours and minutes
            TimeSpan appointmentTimeSpan = new TimeSpan(hours, minutes, 0);

            // Combine the date and time components
            DateTime appointmentDateTime = appointmentDate.Add(appointmentTimeSpan);


            // Validate input
            if (donorId <= 0 || appointmentDate == DateTime.MinValue || string.IsNullOrEmpty(appointmentTime))
            {
                SetStatusMessage("Please enter valid appointment details.", isError: true);
                return;
            }

            // Add the appointment to the repository
            string errorMessage;
            if (appointmentRepository.AddAppointment("Appointment", DateTime.Now, appointmentDateTime, "Pending", donorId, out errorMessage))
            {
                SetStatusMessage("Appointment created successfully!", false);
                ResetInputFields();
            }
            else
            {
                SetStatusMessage($"Error: {errorMessage}", true);
            }

        }

        // Edit Appointment button click event handler
        private void EditAppButton_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId;

            if (int.TryParse(AppIDTextBox.Text, out appointmentId))
            {

            }
            else
            {
                SetStatusMessage($"Error: Enter Valid ID", isError: true);
                ResetInputFields();
                return;
            }

            DateTime appointmentDate = AppDatePicker.SelectedDate ?? DateTime.MinValue;

            // Edit the appointment in the repository
            var (success, errorMessage) = appointmentRepository.EditAppointment(appointmentId, appointmentDate);

            if (success)
            {
                SetStatusMessage("Appointment edited successfully!", isError: false);
                ResetInputFields();
            }
            else
            {
                SetStatusMessage($"Error: {errorMessage}", isError: true);
            }
        }

        // Delete Appointment button click event handler
        private void DeleteAppButton_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId;

            if (int.TryParse(AppIDTextBox.Text, out appointmentId))
            {
                
            }
            else
            {
                SetStatusMessage($"Error: Enter Valid ID", isError: true);
                ResetInputFields();
                return;
            }

            // Delete the appointment from the repository
            var (success, errorMessage) = appointmentRepository.DeleteAppointment(appointmentId);

            if (success)
            {
                SetStatusMessage("Appointment deleted successfully!", isError: false);
                ResetInputFields();
            }
            else
            {
                SetStatusMessage($"Error: {errorMessage}", isError: true);
            }
        }

        private void CreateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            // Get input values
            int patientId;
            if (!int.TryParse(PatientIDTextBox.Text, out patientId))
            {
                SetStatusMessage("Please enter a valid Patient ID.", isError: true);
                return;
            }

            DateTime requestDate = (RequestDatePicker.SelectedDate ?? DateTime.MinValue).Date; // Get the selected date and set the time to midnight
            int requestQuantity;
            if (!int.TryParse(RequestQuantityTextBox.Text, out requestQuantity))
            {
                SetStatusMessage("Please enter a valid Request Quantity.", isError: true);
                return;
            }

            // Validate input
            if (patientId <= 0 || requestDate == DateTime.MinValue || requestQuantity <= 0)
            {
                SetStatusMessage("Please enter valid request details.", isError: true);
                return;
            }

            // Add the request to the repository
            string errorMessage;
            if (requestRepository.AddRequest(patientId, requestDate, requestQuantity, out errorMessage))
            {
                SetStatusMessage("Request created successfully!", isError: false);
                ResetInputFields();
            }
            else
            {
                SetStatusMessage($"Error: {errorMessage}", isError: true);
            }
        }

        private void EditRequestButton_Click(object sender, RoutedEventArgs e)
        {
            int requestId;
            int requestQuantity;

            if (!int.TryParse(RequestIDTextBox.Text, out requestId))
            {
                SetStatusMessage($"Error: Enter Valid ID", isError: true);
                ResetInputFields();
                return;
            }

            if (!int.TryParse(RequestQuantityTextBox.Text, out requestQuantity))
            {
                SetStatusMessage($"Error: Enter Valid Quantity", isError: true);
                ResetInputFields();
                return;
            }

            DateTime requestDate = RequestDatePicker.SelectedDate ?? DateTime.MinValue;

            // Edit the request in the repository
            string errorMessage;
            bool success = requestRepository.UpdateRequest(requestId, requestQuantity, requestDate, out errorMessage);

            if (success)
            {
                SetStatusMessage("Request edited successfully!", isError: false);
                ResetInputFields();
            }
            else
            {
                SetStatusMessage($"Error: {errorMessage}", isError: true);
            }
        }

        private void DeleteRequestButton_Click(object sender, RoutedEventArgs e)
        {
            int requestId;

            if (int.TryParse(RequestIDTextBox.Text, out requestId))
            {

            }
            else
            {
                SetStatusMessage($"Error: Enter Valid ID", isError: true);
                ResetInputFields();
                return;
            }

            // Delete the request from the repository
            var (success, errorMessage) = requestRepository.DeleteRequest(requestId);

            if (success)
            {
                SetStatusMessage("Request deleted successfully!", isError: false);
                ResetInputFields();
            }
            else
            {
                SetStatusMessage($"Error: {errorMessage}", isError: true);
            }
        }

        private void GenerateDonationReceipt_Click(object sender, RoutedEventArgs e)
        {
            int appointmentId;
            int quantity;

            if (!int.TryParse(AppIDDonationTextBox.Text, out appointmentId))
            {
                SetStatusMessage($"Error: Enter a valid Appointment ID", isError: true);
                ResetInputFields();
                return;
            }

            if (!int.TryParse(QuantityDonationTextBox.Text, out quantity))
            {
                SetStatusMessage($"Error: Enter a valid Quantity", isError: true);
                ResetInputFields();
                return;
            }

            // Retrieve the user's staff ID
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            int staffId;
            int.TryParse(user.Id, out staffId);

            // Retrieve the patient's blood type from the Appointments table
            string bloodType;

            using (SqlConnection connection = repositoryBase.GetConnection())
            {
                string bloodTypeQuery = "SELECT [Donor_BloodType] FROM [Donor] WHERE [Donor_ID] =  (SELECT [Donor_ID] FROM [Appointment] WHERE [ID] = @AppointmentId)";
                using (SqlCommand command = new SqlCommand(bloodTypeQuery, connection))
                {
                    command.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    connection.Open();
                    bloodType = command.ExecuteScalar()?.ToString();
                }
            }

            // Generate the receipt
            bool success;
            string errorMessage;
            success = receiptRepository.AddReceipt(DateTime.Now, bloodType, quantity, "Donation", staffId, appointmentId, out errorMessage);


            if (success)
            {
                SetStatusMessage("Receipt generated successfully!", isError: false);
                ResetInputFields();
            }
            else
            {
                SetStatusMessage($"Error: {errorMessage}", isError: true);
            }
        }

        private void GenerateReceivingReceipt_Click(object sender, RoutedEventArgs e)
        {
            int requestId;

            if (!int.TryParse(RequestIDReceivingTextBox.Text, out requestId))
            {
                SetStatusMessage($"Error: Enter a valid Request ID", isError: true);
                ResetInputFields();
                return;
            }

            // Retrieve the user's staff ID
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            int staffId;
            int.TryParse(user.Id, out staffId);

            // Retrieve the patient's blood type and quantity from the Request table
            string bloodType;
            int quantity;

            using (SqlConnection connection = repositoryBase.GetConnection())
            {
                string query = "SELECT [Patient_ID], [Request_Quantity] FROM [Request] WHERE [Request_ID] = @RequestId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RequestId", requestId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int patientId = reader.GetInt32(0);
                            quantity = reader.GetInt32(1);

                            // Close the first data reader
                            reader.Close();

                            // Retrieve the patient's blood type from the Patient table
                            string bloodTypeQuery = "SELECT [Patient_BloodType] FROM [Patient] WHERE [Patient_ID] = @PatientId";
                            using (SqlCommand bloodTypeCommand = new SqlCommand(bloodTypeQuery, connection))
                            {
                                bloodTypeCommand.Parameters.AddWithValue("@PatientId", patientId);
                                bloodType = bloodTypeCommand.ExecuteScalar()?.ToString();
                            }
                        }
                        else
                        {
                            SetStatusMessage($"Error: Request with ID {requestId} not found", isError: true);
                            ResetInputFields();
                            return;
                        }
                    }
                }
            }

            // Generate the receipt
            string errorMessage;
            if (receiptRepository.AddReceipt(DateTime.Now, bloodType, quantity, "Receiving", staffId, requestId, out errorMessage))
            {
                SetStatusMessage("Receiving receipt generated successfully!", isError: false);
                ResetInputFields();
            }
            else
            {
                SetStatusMessage($"Error: {errorMessage}", isError: true);
            }
        }



        private void SetStatusMessage(string message, bool isError)
        {
            StatusMessageTextBlock.Text = message;

            // Set the color based on the isError parameter
            Brush brush = isError ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD152")) // Error color
                                  : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#33FF00")); // Success color

            StatusMessageTextBlock.Foreground = brush;
        }

        private void ResetInputFields()
        {
            DonorIDTextBox.Text = string.Empty;
            AppDatePicker.SelectedDate = null;
            AppTimeComboBox.SelectedItem = null;
            AppIDTextBox.Text = string.Empty;
            PatientIDTextBox.Text = string.Empty;
            RequestDatePicker.SelectedDate = null;
            RequestQuantityTextBox.Text = string.Empty;
            RequestIDTextBox.Text = string.Empty;
            RequestIDReceivingTextBox.Text = string.Empty;
            QuantityDonationTextBox.Text = string.Empty;
            AppIDDonationTextBox.Text = string.Empty;
        }
    }
}

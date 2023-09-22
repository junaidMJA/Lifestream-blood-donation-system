using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPFApp.Models;
using WPFApp.Repositories;
using WPFApp.ViewModels;

namespace WPFApp.Views
{
    public partial class RequestView : UserControl
    {
        private RequestViewModel requestViewModel;
        private ObservableCollection<RequestModel> filteredRequests;

        public RequestView()
        {
            InitializeComponent();
            requestViewModel = new RequestViewModel();
            filteredRequests = new ObservableCollection<RequestModel>(requestViewModel.Requests);
            RequestGrid.ItemsSource = filteredRequests;
        }

        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            FilterRequests();
        }

        private void FilterRequests()
        {
            filteredRequests.Clear();

            string searchQuery = SearchTextBox.Text.ToLower();
            string bloodType = BloodTypeCombo.Text;
            DateTime fromDate = FromDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime toDate = ToDatePicker.SelectedDate ?? DateTime.MaxValue;
            string requestStatus = RequestTypeCombo.Text;

            RequestRepository requestRepository = new RequestRepository();
            PatientRepository patientRepository = new PatientRepository();

            // Retrieve requests based on blood type
            var matchingRequestsByBloodType = requestRepository.GetRequestsByBloodType(bloodType);

            // Retrieve other requests based on patient name
            var matchingRequestsByPatientName = requestRepository.GetRequestsByPatientName(searchQuery);

            foreach (RequestModel request in matchingRequestsByBloodType)
            {
                // Filter requests by date range, status, and patient name
                if ((request.RequiredDate >= fromDate && request.RequiredDate <= toDate) &&
                    (request.Status == requestStatus || requestStatus == "All Requests" || requestStatus == "--Select Status--") &&
                    (string.IsNullOrEmpty(searchQuery) || patientRepository.GetPatientByID(request.PatientID)?.Name.ToLower().Contains(searchQuery) == true))
                {
                    filteredRequests.Add(request);
                }
            }

            foreach (RequestModel request in matchingRequestsByPatientName)
            {
                // Filter requests by date range, status, and blood type
                if ((request.RequiredDate >= fromDate && request.RequiredDate <= toDate) &&
                    (request.Status == requestStatus || requestStatus == "All Requests" || requestStatus == "--Select Status--") &&
                    (string.IsNullOrEmpty(bloodType) || request.BloodType == bloodType || bloodType == "All Blood Types" || bloodType == "--Select Type--"))
                {
                    bool flag = false;
                    foreach (RequestModel requestModel in filteredRequests)
                    {
                        if (requestModel.RequestID == request.RequestID)
                        {
                            flag = true; break;
                        }
                    }
                    if (flag == false)
                    {
                        filteredRequests.Add(request);
                    }
                }
            }
        }
    }
}

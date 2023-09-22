using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private HomeRepository homeRepository;

        public HomeViewModel()
        {
            homeRepository = new HomeRepository();

            SeriesCollection = new SeriesCollection();
            Labels = new List<string>();
            BloodTypeCount = new ObservableCollection<BloodTypeCount>();
            GenderSeriesCollection = new SeriesCollection();

            DateTime startDate = new DateTime(2023, 01, 01);
            DateTime endDate = new DateTime(2023, 12, 31);
            int interval = 30;

            // Call the method in HomeRepository to get the blood type graph data
            List<BloodTypeGraphData> bloodTypeGraphData = homeRepository.GetBloodTypeGraphData(startDate, endDate, interval).ToList();

            // Extract the distinct blood types from the graph data
            List<string> bloodTypes = bloodTypeGraphData.Select(d => d.BloodType).Distinct().ToList();

            // Create LineSeries for each blood type
            foreach (string bloodType in bloodTypes)
            {
                List<double> donationCounts = bloodTypeGraphData
                    .Where(d => d.BloodType == bloodType)
                    .OrderBy(d => d.GraphDate)
                    .Select(d => (double)d.DonationCount)
                    .ToList();

                SeriesCollection.Add(new LineSeries
                {
                    Title = bloodType,
                    Values = new ChartValues<double>(donationCounts),
                    PointGeometrySize = 5,
                    LineSmoothness = 0.5,
                });
            }

            // Get the distinct graph dates
            List<DateTime> graphDates = bloodTypeGraphData.Select(d => d.GraphDate.Date).Distinct().ToList();

            // Create labels for each graph date
            Labels = graphDates.Select(date => date.ToString("dd/MM/yyyy")).ToList();

            YFormatter = value => value.ToString("N0");

            // Call the method in HomeRepository to get the blood type count data
            List<BloodTypeCount> bloodTypeCounts = homeRepository.CountBlood(startDate, endDate);
            BloodTypeCount = new ObservableCollection<BloodTypeCount>(bloodTypeCounts);

            // Call the method in HomeRepository to get the gender count data
            var genderCounts = homeRepository.GetGenderCounts();
            if (genderCounts.Rows.Count > 0)
            {
                MaleDonorsCount = new ChartValues<int> { Convert.ToInt32(genderCounts.Rows[0]["MaleDonors"]) };
                FemaleDonorsCount = new ChartValues<int> { Convert.ToInt32(genderCounts.Rows[0]["FemaleDonors"]) };
                MalePatientsCount = new ChartValues<int> { Convert.ToInt32(genderCounts.Rows[0]["MalePatients"]) };
                FemalePatientsCount = new ChartValues<int> { Convert.ToInt32(genderCounts.Rows[0]["FemalePatients"]) };
            }

            MaleDonorsLabelPoint = point => $"Male Donors: {point.Y}";
            FemaleDonorsLabelPoint = point => $"Female Donors: {point.Y}";
            MalePatientsLabelPoint = point => $"Male Patients: {point.Y}";
            FemalePatientsLabelPoint = point => $"Female Patients: {point.Y}";

            GenderSeriesCollection.AddRange(new SeriesCollection
            {
                new PieSeries { Title = "Male Donors", Values = MaleDonorsCount, DataLabels = true, LabelPoint = MaleDonorsLabelPoint, PushOut = 10 },
                new PieSeries { Title = "Female Donors", Values = FemaleDonorsCount, DataLabels = true, LabelPoint = FemaleDonorsLabelPoint, PushOut = 10 },
                new PieSeries { Title = "Male Patients", Values = MalePatientsCount, DataLabels = true, LabelPoint = MalePatientsLabelPoint, PushOut = 10 },
                new PieSeries { Title = "Female Patients", Values = FemalePatientsCount, DataLabels = true, LabelPoint = FemalePatientsLabelPoint, PushOut = 10 }
            });


            // Call the method in HomeRepository to get the count of pending appointments
            int pendingAppointmentCount = homeRepository.CountPendingAppointments(startDate, endDate);

            // Call the method in HomeRepository to get the count of pending requests
            int pendingRequestCount = homeRepository.CountPendingRequests(startDate, endDate);

            // Update the text block bindings with the counts
            PendingAppointmentCount = pendingAppointmentCount.ToString();
            PendingRequestCount = pendingRequestCount.ToString();

        }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public ObservableCollection<BloodTypeCount> BloodTypeCount { get; set; }

        public SeriesCollection GenderSeriesCollection { get; set; }
        public ChartValues<int> MaleDonorsCount { get; set; }
        public ChartValues<int> FemaleDonorsCount { get; set; }
        public ChartValues<int> MalePatientsCount { get; set; }
        public ChartValues<int> FemalePatientsCount { get; set; }
        public Func<ChartPoint, string> MaleDonorsLabelPoint { get; set; }
        public Func<ChartPoint, string> FemaleDonorsLabelPoint { get; set; }
        public Func<ChartPoint, string> MalePatientsLabelPoint { get; set; }
        public Func<ChartPoint, string> FemalePatientsLabelPoint { get; set; }

        public string PendingAppointmentCount { get; set; }
        public string PendingRequestCount { get; set; }
    }
}

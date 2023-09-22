using System;

namespace WPFApp.Models
{
    public class AppointmentModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime PlacementDate { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public int DonorID { get; set; }
    }
}

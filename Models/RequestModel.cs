using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Models
{
    public class RequestModel
    {
        public int RequestID { get; set; }
        public string BloodType { get; set; }
        public int Quantity { get; set; }
        public DateTime PlaceDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string Status { get; set; }
        public int PatientID { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Models
{
    public class ReceiptModel
    {
        public int ReceiptID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string BloodType { get; set; }
        public int Quantity { get; set; }
        public string BagHealth { get; set; }
        public string Process { get; set; }
        public int? StaffID { get; set; }
        public int? RequestID { get; set; }
        public int? AppointmentID { get; set; }
    }

}

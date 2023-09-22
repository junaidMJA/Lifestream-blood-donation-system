using System;

namespace WPFApp.Models
{
    public class UserModel
    {
        public string Id { get; set; } // Staff ID
        public string StaffName { get; set; } // Staff Name
        public string Gender { get; set; }
        public string Designation { get; set; }
        public int Salary { get; set; }
        public DateTime DOB { get; set; }
        public string Contact { get; set; }
        public string StaffShift { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] Photo { get; set; }

        // Additional properties as needed
    }
}

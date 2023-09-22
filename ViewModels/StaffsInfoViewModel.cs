using System;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Reflection;
using WPFApp.Repositories;
using WPFApp.Models;
using System.IO;

namespace WPFApp.ViewModels
{
    public class StaffsInfoViewModel : ViewModelBase
    {
        private readonly UserRepository userRepository;
        private string staffId;
        private string name;
        private DateTime dateOfBirth = DateTime.Now;
        private string shift = "Shift";
        private string gender = "Gender";
        private string contact;
        private string designation = "Designation";
        private int salary;
        private string username;
        private string password;
        private BitmapImage selectedImage;
        private string statusTextBlock;
        private string statusTextColor;

        public string StaffId
        {
            get { return staffId; }
            set
            {
                staffId = value;
                OnPropertyChanged(nameof(StaffId));
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        public string Shift
        {
            get { return shift; }
            set
            {
                shift = value;
                OnPropertyChanged(nameof(Shift));
            }
        }

        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }

        public string Designation
        {
            get { return designation; }
            set
            {
                designation = value;
                OnPropertyChanged(nameof(Designation));
            }
        }

        public int Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                OnPropertyChanged(nameof(Salary));
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public BitmapImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        public string StatusTextBlock
        {
            get { return statusTextBlock; }
            set
            {
                statusTextBlock = value;
                OnPropertyChanged(nameof(StatusTextBlock));
            }
        }

        public string StatusTextColor
        {
            get { return statusTextColor; }
            set
            {
                statusTextColor = value;
                OnPropertyChanged(nameof(StatusTextColor));
            }
        }

        public ViewModelCommand AddStaffCommand { get; }
        public ViewModelCommand UpdateStaffCommand { get; }
        public ViewModelCommand DeleteStaffCommand { get; }
        public ViewModelCommand SelectImageCommand { get; }
        public ViewModelCommand CancelCommand { get; }

        public StaffsInfoViewModel()
        {
            userRepository = new UserRepository();
            SelectImageCommand = new ViewModelCommand(SelectImage);
            SetDefaultImage();

            AddStaffCommand = new ViewModelCommand(AddStaff, CanAddStaff);
            UpdateStaffCommand = new ViewModelCommand(UpdateStaff, CanUpdateStaff);
            DeleteStaffCommand = new ViewModelCommand(DeleteStaff, CanDeleteStaff);
            CancelCommand = new ViewModelCommand(Cancel, CanCancel);
        }

        private bool CanCancel(object parameter)
        {
            // Implement your validation logic here
            return true;
        }

        private void Cancel(object parameter)
        {
            ClearFields();
        }

        private bool CanAddStaff(object parameter)
        {
            // Implement your validation logic here
            return true;
        }

        private void AddStaff(object parameter)
        {
            try
            {
                // Validate input fields
                if (!ValidateFields())
                {
                    return;
                }

                // Create a new UserModel object
                var staff = new UserModel
                {
                    Id = StaffId,
                    StaffName = Name,
                    DOB = DateOfBirth,
                    StaffShift = Shift,
                    Gender = Gender,
                    Contact = Contact,
                    Designation = Designation,
                    Salary = Salary,
                    UserName = Username,
                    Password = Password,
                    Photo = SelectedImage == GetDefaultImage() ? null : ConvertBitmapImageToByteArray(SelectedImage)
                };

                // Call the repository method to add the staff
                userRepository.AddStaff(staff);

                // Clear the input fields
                ClearFields();

                // Display success message
                StatusTextBlock = "Staff added successfully.";
                StatusTextColor = "#00FF00"; // Green color
            }
            catch (Exception ex)
            {
                // Handle any exceptions and display error message
                StatusTextBlock = "An error occurred while adding the staff: " + ex.Message;
                StatusTextColor = "#FFFF00"; // Yellow color
            }
        }

        private bool CanUpdateStaff(object parameter)
        {
            // Implement your validation logic here
            return true;
        }

        private void UpdateStaff(object parameter)
        {
            if (string.IsNullOrWhiteSpace(StaffId))
            {
                StatusTextBlock = "Please enter a valid Staff ID.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return;
            }

            if (!int.TryParse(StaffId, out int staffId))
            {
                StatusTextBlock = "Please enter a valid Staff ID.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return;
            }

            try
            {
                // Retrieve the existing staff details
                var existingStaff = userRepository.GetById(staffId);
                if (existingStaff == null)
                {
                    StatusTextBlock = "Staff not found.";
                    StatusTextColor = "#FFFF00"; // Yellow color
                    return;
                }

                // Update the staff details
                if (!string.IsNullOrWhiteSpace(Name) && Name != existingStaff.StaffName)
                {
                    existingStaff.StaffName = Name;
                }

                if (DateOfBirth != existingStaff.DOB)
                {
                    existingStaff.DOB = DateOfBirth;
                }

                if (!string.IsNullOrWhiteSpace(Shift) && Shift != "Shift" && Shift != existingStaff.StaffShift)
                {
                    existingStaff.StaffShift = Shift;
                }

                if (!string.IsNullOrWhiteSpace(Gender) && Gender != "Gender" && Gender != existingStaff.Gender)
                {
                    existingStaff.Gender = Gender;
                }

                if (!string.IsNullOrWhiteSpace(Contact) && Contact != existingStaff.Contact)
                {
                    existingStaff.Contact = Contact;
                }

                if (!string.IsNullOrWhiteSpace(Designation) && Designation != "Designation" && Designation != existingStaff.Designation)
                {
                    existingStaff.Designation = Designation;
                }

                if (Salary != existingStaff.Salary)
                {
                    existingStaff.Salary = Salary;
                }

                if (!string.IsNullOrWhiteSpace(Username) && Username != existingStaff.UserName)
                {
                    existingStaff.UserName = Username;
                }

                if (!string.IsNullOrWhiteSpace(Password) && Password != existingStaff.Password)
                {
                    existingStaff.Password = Password;
                }

                if (SelectedImage != GetDefaultImage())
                {
                    existingStaff.Photo = ConvertBitmapImageToByteArray(SelectedImage);
                }
                else
                {
                    existingStaff.Photo = null;
                }

                // Call the repository method to update the staff
                userRepository.UpdateStaff(existingStaff);

                // Clear the input fields
                ClearFields();

                // Display success message
                StatusTextBlock = "Staff updated successfully.";
                StatusTextColor = "#00FF00"; // Green color
            }
            catch (Exception ex)
            {
                // Handle any exceptions and display error message
                StatusTextBlock = "An error occurred while updating the staff: " + ex.Message;
                StatusTextColor = "#FFFF00"; // Yellow color
            }
        }


        private BitmapImage GetDefaultImage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = "WPFApp.Images.myprofile.jpg"; // Replace "WPFApp" with your project namespace
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    var defaultImage = new BitmapImage();
                    defaultImage.BeginInit();
                    defaultImage.StreamSource = stream;
                    defaultImage.CacheOption = BitmapCacheOption.OnLoad;
                    defaultImage.EndInit();
                    defaultImage.Freeze(); // Freeze the image for better performance
                    return defaultImage;
                }
            }
            return null;
        }

        private bool CanDeleteStaff(object parameter)
        {
            // Implement your validation logic here
            return true;
        }

        private void DeleteStaff(object parameter)
        {
            try
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(StaffId))
                {
                    StatusTextBlock = "Please enter a valid Staff ID.";
                    StatusTextColor = "#FFFF00"; // Yellow color
                    return;
                }

                if (!int.TryParse(StaffId, out int staffId))
                {
                    StatusTextBlock = "Please enter a valid Staff ID.";
                    StatusTextColor = "#FFFF00"; // Yellow color
                    return;
                }

                // Call the repository method to delete the staff
                userRepository.DeleteStaff(staffId);

                // Clear the input fields
                ClearFields();

                // Display success message
                StatusTextBlock = "Staff deleted successfully.";
                StatusTextColor = "#00FF00"; // Green color
            }
            catch (Exception ex)
            {
                // Handle any exceptions and display error message
                StatusTextBlock = "An error occurred while deleting the staff: " + ex.Message;
                StatusTextColor = "#FFFF00"; // Yellow color
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name == "Name")
            {
                StatusTextBlock = "Please enter a valid name.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            if (DateOfBirth == DateTime.Now)
            {
                StatusTextBlock = "Please enter a valid date of birth.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            if (string.IsNullOrWhiteSpace(Shift) || Shift == "Shift")
            {
                StatusTextBlock = "Please enter a valid shift.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            if (string.IsNullOrWhiteSpace(Gender) || Gender == "Gender")
            {
                StatusTextBlock = "Please enter a valid gender.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            if (string.IsNullOrWhiteSpace(Contact) || Contact == "Contact")
            {
                StatusTextBlock = "Please enter a valid contact.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            if (string.IsNullOrWhiteSpace(Designation) || Designation == "Designation")
            {
                StatusTextBlock = "Please enter a valid designation.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            if (Salary <= 0)
            {
                StatusTextBlock = "Please enter a valid salary.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            if (string.IsNullOrWhiteSpace(Username) || Username == "Username")
            {
                StatusTextBlock = "Please enter a valid username.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password) || Password == "Password")
            {
                StatusTextBlock = "Please enter a valid password.";
                StatusTextColor = "#FFFF00"; // Yellow color
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            // Clear all input fields
            StaffId = string.Empty;
            Name = string.Empty;
            DateOfBirth = DateTime.Now;
            Shift = "Shift";
            Gender = "Gender";
            Contact = string.Empty;
            Designation = "Designation";
            Salary = 0;
            Username = string.Empty;
            Password = string.Empty;
            SetDefaultImage();
        }

        private void SetDefaultImage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = "WPFApp.Images.myprofile.jpg"; // Replace "WPFApp" with your project namespace
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    var defaultImage = new BitmapImage();
                    defaultImage.BeginInit();
                    defaultImage.StreamSource = stream;
                    defaultImage.CacheOption = BitmapCacheOption.OnLoad;
                    defaultImage.EndInit();
                    defaultImage.Freeze(); // Freeze the image for better performance
                    SelectedImage = defaultImage;
                }
            }
        }

        private void SelectImage(object parameter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp|All Files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // Create a new BitmapImage and set it as the source
                SelectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private byte[] ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] byteArray = null;
            if (bitmapImage != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int qualityLevel = 100;
                    while (qualityLevel > 0 && (byteArray == null || byteArray.Length > 50000))
                    {
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.QualityLevel = qualityLevel;
                        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

                        memoryStream.SetLength(0);
                        encoder.Save(memoryStream);
                        byteArray = memoryStream.ToArray();

                        qualityLevel -= 5;
                    }
                }
            }
            return byteArray;
        }



    }
}

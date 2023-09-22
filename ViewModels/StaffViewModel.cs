using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.ViewModels
{
    public class StaffViewModel : ViewModelBase
    {
        private readonly UserRepository userRepository;
        private ObservableCollection<UserModel> users;
        private SearchParameters searchParameters;

        public ObservableCollection<UserModel> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public SearchParameters SearchParameters
        {
            get { return searchParameters; }
            set
            {
                searchParameters = value;
                OnPropertyChanged(nameof(SearchParameters));
            }
        }

        public ICommand SearchCommand { get; }

        public StaffViewModel()
        {
            userRepository = new UserRepository();

            // Retrieve all users from the repository
            var allUsers = userRepository.GetByAll();

            // Set the default profile picture for users with null photo
            foreach (var user in allUsers)
            {
                if (user.Photo == null)
                {
                    user.Photo = LoadDefaultProfilePicture();
                }
            }

            // Populate the Users collection with the retrieved users
            Users = new ObservableCollection<UserModel>(allUsers);

            // Initialize the search command
            SearchCommand = new RelayCommand(Search, CanSearch);

            // Initialize the search parameters
            SearchParameters = new SearchParameters();
        }

        private void Search(object parameter)
        {
            try
            {
                // Retrieve all users from the repository
                var allUsers = userRepository.GetByAll();

                // Filter the users based on the search parameters
                var filteredUsers = allUsers;

                if (int.TryParse(SearchParameters.MinSalary, out int minSalary))
                {
                    filteredUsers = filteredUsers.Where(u => u.Salary >= minSalary);
                }

                if (int.TryParse(SearchParameters.MaxSalary, out int maxSalary))
                {
                    filteredUsers = filteredUsers.Where(u => u.Salary <= maxSalary);
                }

                if (!string.IsNullOrWhiteSpace(SearchParameters.Designation) && SearchParameters.Designation != "--Select Designation--" && SearchParameters.Designation != "All Staff")
                {
                    filteredUsers = filteredUsers.Where(u => u.Designation == SearchParameters.Designation);
                }

                if (!string.IsNullOrWhiteSpace(SearchParameters.Shift) && SearchParameters.Shift != "--Select Shift--" && SearchParameters.Shift != "All Shifts")
                {
                    filteredUsers = filteredUsers.Where(u => u.StaffShift == SearchParameters.Shift);
                }

                if (!string.IsNullOrWhiteSpace(SearchParameters.Name))
                {
                    filteredUsers = filteredUsers.Where(u => u.StaffName.IndexOf(SearchParameters.Name, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                // Set the default profile picture for filtered users with null photo
                foreach (var user in filteredUsers)
                {
                    if (user.Photo == null)
                    {
                        user.Photo = LoadDefaultProfilePicture();
                    }
                }

                // Update the Users collection with the filtered users
                Users = new ObservableCollection<UserModel>(filteredUsers);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and display an error message
                Console.WriteLine("An error occurred while searching for staff: " + ex.Message);
            }
        }


        private bool CanSearch(object parameter)
        {
            // You can implement additional logic here if needed
            return true;
        }

        private byte[] LoadDefaultProfilePicture()
        {
            // Load the default profile picture from the resource
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourcePath = "WPFApp.Images.myprofile.jpg"; // Replace "WPFApp" with your project namespace
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                var defaultImageBytes = new byte[stream.Length];
                stream.Read(defaultImageBytes, 0, defaultImageBytes.Length);
                return defaultImageBytes;
            }
        }
    }

    public class SearchParameters : ViewModelBase
    {
        private string minSalary;
        private string maxSalary;
        private string designation = "All Staff";
        private string shift = "All Shifts";
        private string name;

        public string MinSalary
        {
            get { return minSalary; }
            set
            {
                minSalary = value;
                OnPropertyChanged(nameof(MinSalary));
            }
        }

        public string MaxSalary
        {
            get { return maxSalary; }
            set
            {
                maxSalary = value;
                OnPropertyChanged(nameof(MaxSalary));
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

        public string Shift
        {
            get { return shift; }
            set
            {
                shift = value;
                OnPropertyChanged(nameof(Shift));
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
    }
}

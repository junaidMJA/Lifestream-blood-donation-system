using FontAwesome.Sharp;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WPFApp.Models;
using WPFApp.Repositories;
using System.Reflection;
using WPFApp.Views;

namespace WPFApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Fields
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        private IUserRepository userRepository;

        //Properties
        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }

            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public string Caption
        {
            get
            {
                return _caption;
            }

            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public IconChar Icon
        {
            get
            {
                return _icon;
            }

            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        //--> Commands
        public ICommand LogoutCommand { get; }
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowDonorViewCommand { get; }
        public ICommand ShowPatientViewCommand { get; }
        public ICommand ShowDonorsInfoViewCommand { get; }
        public ICommand ShowPatientsInfoViewCommand { get; }
        public ICommand ShowActivitiesViewCommand { get; }
        public ICommand ShowAppointmentsViewCommand { get; }
        public ICommand ShowRequestViewCommand { get; }
        public ICommand ShowReceiptViewCommand { get; }
        public ICommand ShowStockViewCommand { get; }
        public ICommand ShowStaffViewCommand { get; }
        public ICommand ShowStaffsInfoViewCommand { get; }
        public ICommand ShowProfileViewCommand { get; }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);

            //Initialize commands
            LogoutCommand = new ViewModelCommand(ExecuteLogoutCommand);

            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowDonorViewCommand = new ViewModelCommand(ExecuteShowDonorViewCommand);
            ShowPatientViewCommand = new ViewModelCommand(ExecuteShowPatientViewCommand);
            ShowDonorsInfoViewCommand = new ViewModelCommand(ExecuteShowDonorsInfoViewCommand);
            ShowPatientsInfoViewCommand = new ViewModelCommand(ExecuteShowPatientsInfoViewCommand);
            ShowActivitiesViewCommand = new ViewModelCommand(ExecuteShowActivitiesViewCommand);
            ShowAppointmentsViewCommand = new ViewModelCommand(ExecuteShowAppointmentsViewCommand);
            ShowRequestViewCommand = new ViewModelCommand(ExecuteShowRequestViewCommand);
            ShowReceiptViewCommand = new ViewModelCommand(ExecuteShowReceiptViewCommand);
            ShowStockViewCommand = new ViewModelCommand(ExecuteShowStockViewCommand);
            if (user.Designation == "Admin")
            {
                ShowStaffViewCommand = new ViewModelCommand(ExecuteShowStaffViewCommand);
                ShowStaffsInfoViewCommand = new ViewModelCommand(ExecuteShowStaffsInfoViewCommand);
            }
            else 
            {
                ShowStaffViewCommand = new ViewModelCommand(ExecuteShowAccessDeniedViewCommand);
                ShowStaffsInfoViewCommand = new ViewModelCommand(ExecuteShowAccessDeniedViewCommand);
            }
            ShowProfileViewCommand = new ViewModelCommand(ExecuteShowProfileViewCommand);

            //Default view
            ExecuteShowHomeViewCommand(null);

            LoadCurrentUserData();
        }
        private void ExecuteLogoutCommand(object obj)
        {
            var loginView = new LoginView();
            loginView.Show();
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }
            Application.Current.MainWindow = loginView;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }
        private void ExecuteShowDonorViewCommand(object obj)
        {
            CurrentChildView = new DonorViewModel();
            Caption = "Donors";
            Icon = IconChar.UserGroup;
        }
        private void ExecuteShowPatientViewCommand(object obj)
        {
            CurrentChildView = new PatientViewModel();
            Caption = "Patients";
            Icon = IconChar.BedPulse;
        }
        private void ExecuteShowDonorsInfoViewCommand(object obj)
        {
            CurrentChildView = new DonorsInfoViewModel();
            Caption = "Donor's Information";
            Icon = IconChar.List;
        }
        private void ExecuteShowPatientsInfoViewCommand(object obj)
        {
            CurrentChildView = new PatientsInfoViewModel();
            Caption = "Patient's Information";
            Icon = IconChar.List;
        }
        private void ExecuteShowActivitiesViewCommand(object obj)
        {
            CurrentChildView = new ActivitiesViewModel();
            Caption = "Manage Operations";
            Icon = IconChar.CalendarCheck;
        }
        private void ExecuteShowAppointmentsViewCommand(object obj)
        {
            CurrentChildView = new AppointmentsViewModel();
            Caption = "Manage Appointments";
            Icon = IconChar.PenToSquare;
        }
        private void ExecuteShowRequestViewCommand(object obj)
        {
            CurrentChildView = new RequestViewModel();
            Caption = "Manage Requests";
            Icon = IconChar.HandHoldingMedical;
        }
        private void ExecuteShowReceiptViewCommand(object obj)
        {
            CurrentChildView = new ReceiptViewModel();
            Caption = "Manage Receipts";
            Icon = IconChar.Receipt;
        }
        private void ExecuteShowStockViewCommand(object obj)
        {
            CurrentChildView = new StockViewModel();
            Caption = "Current Stock";
            Icon = IconChar.BoxesStacked;
        }
        private void ExecuteShowStaffViewCommand(object obj)
        {
            CurrentChildView = new StaffViewModel();
            Caption = "View Staff";
            Icon = IconChar.ClipboardUser;
        }
        private void ExecuteShowStaffsInfoViewCommand(object obj)
        {
            CurrentChildView = new StaffsInfoViewModel();
            Caption = "Staff's Information";
            Icon = IconChar.List;
        }
        private void ExecuteShowProfileViewCommand(object obj)
        {
            CurrentChildView = new ProfileViewModel();
            Caption = "Your Profile";
            Icon = IconChar.User;
        }
        private void ExecuteShowAccessDeniedViewCommand(object obj)
        {
            CurrentChildView = new AccessDeniedViewModel();
            Caption = "No Access";
            Icon = IconChar.Xmark;
        }
        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.UserName;
                CurrentUserAccount.DisplayName = $"Welcome {user.StaffName}";

                if (user.Photo != null)
                {
                    try
                    {
                        // Convert the byte array to a BitmapImage
                        var imageSource = new BitmapImage();
                        using (var stream = new MemoryStream(user.Photo))
                        {
                            imageSource.BeginInit();
                            imageSource.StreamSource = stream;
                            imageSource.CacheOption = BitmapCacheOption.OnLoad;
                            imageSource.EndInit();
                        }

                        CurrentUserAccount.ProfilePicture = imageSource;
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (e.g., log or display an error message)
                        Console.WriteLine($"Error loading profile picture: {ex.Message}");
                    }
                }
                else
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourcePath = "WPFApp.Images.myprofile.jpg";
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
                            CurrentUserAccount.ProfilePicture = defaultImage;
                        }
                    }
                }
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
                //Hide child views.
            }
        }


    }
}

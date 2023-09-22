using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;
using WPFApp.Models;
using WPFApp.Repositories;
using System.Windows.Media;
using System.Threading;
using Microsoft.Win32;
using WPFApp.Views;
using System.Windows;

namespace WPFApp.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private UserRepository userRepository;
        private UserModel currentUserAccount;
        private BitmapImage selectedImage;
        private BitmapImage defaultImage;
        private string originalPassword;
        private string statusTextBlock;
        private SolidColorBrush statusTextColor;

        public ProfileViewModel()
        {
            userRepository = new UserRepository();
            defaultImage = LoadDefaultImage(); // Load the default image
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
            UploadPhotoCommand = new RelayCommand(UploadPhoto);
            LogoutCommand = new RelayCommand(Logout);
            CurrentUserAccount = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (currentUserAccount.Photo != null)
            {
                selectedImage = ConvertByteArrayToBitmapImage(CurrentUserAccount.Photo);
            }
            else
            {
                selectedImage = defaultImage; // Set the default image initially
            }
        }

        public UserModel CurrentUserAccount
        {
            get { return currentUserAccount; }
            set
            {
                currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
                // Set the original password for comparison
                originalPassword = currentUserAccount.Password;
            }
        }

        public BitmapImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
                SaveCommand.RaiseCanExecuteChanged();
                CancelCommand.RaiseCanExecuteChanged();
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

        public SolidColorBrush StatusTextColor
        {
            get { return statusTextColor; }
            set
            {
                statusTextColor = value;
                OnPropertyChanged(nameof(StatusTextColor));
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand UploadPhotoCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }

        private bool CanSave(object parameter)
        {
            // Enable save button if the image or password has changed
            return (selectedImage != defaultImage) || currentUserAccount.Password != originalPassword || currentUserAccount.Password != null;
        }

        private void Save(object parameter)
        {
            // Update the user's image and password if they are changed
            if (selectedImage != defaultImage)
            {
                currentUserAccount.Photo = ConvertBitmapImageToByteArray(selectedImage);
            }

            if (currentUserAccount.Password != originalPassword)
            {
                // Validate the new password
                if (string.IsNullOrWhiteSpace(currentUserAccount.Password) || currentUserAccount.Password.Contains(" "))
                {
                    // Display error message
                    StatusTextBlock = "Invalid password. Password should not be empty or contain spaces.";
                    StatusTextColor = Brushes.Yellow;
                    return;
                }
            }

            // Update the user in the database
            bool isSuccess = userRepository.UpdateUser(currentUserAccount);

            if (isSuccess)
            {
                // Display success message
                StatusTextBlock = "Profile updated successfully.";
                StatusTextColor = Brushes.LightGreen;
            }
            else
            {
                // Display error message
                StatusTextBlock = "Failed to update profile.";
                StatusTextColor = Brushes.Yellow;
            }
        }

        private bool CanCancel(object parameter)
        {
            // Enable cancel button if the image or password has changed
            return selectedImage != defaultImage || currentUserAccount.Password != originalPassword || currentUserAccount.Password != null;
        }

        private void Cancel(object parameter)
        {
            // Reset the password to the original value
            currentUserAccount.Password = originalPassword;

            // Set the image to the default image
            if (currentUserAccount.Photo != null)
            {
                selectedImage = ConvertByteArrayToBitmapImage(CurrentUserAccount.Photo);
            }
            else
            {
                selectedImage = defaultImage; // Set the default image initially
            }
            OnPropertyChanged(nameof(SelectedImage));

            // Reset the status message
            StatusTextBlock = "";
            StatusTextColor = Brushes.Black;
        }

        private void UploadPhoto(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                selectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        private void Logout(object parameter)
        {
            var loginView = new LoginView();
            loginView.Show();
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }
            Application.Current.MainWindow = loginView;
        }

        private BitmapImage LoadDefaultImage()
        {
            try
            {
                // Get the assembly containing this code
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Get the full resource name
                string resourceName = "WPFApp.Images.myprofile.jpg";

                // Load the default image from the assembly
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = stream;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        image.Freeze(); // Freeze the image for better performance
                        return image;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null; // Return null or a fallback image if loading fails
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



        private BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            try
            {
                using (MemoryStream memoryStream = new MemoryStream(byteArray))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = memoryStream;
                    image.EndInit();
                    image.Freeze(); // Freeze the image for better performance
                    return image;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null; // Return null or handle the error in your application
        }

    }
}

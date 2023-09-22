using System.Windows.Media.Imaging;

namespace WPFApp.Models
{
    public class UserAccountModel
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public BitmapImage ProfilePicture { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFApp.Views;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {

        }

        private LoginView loginView;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create the LoginView and subscribe to its visibility changed event
            loginView = new LoginView();
            Application.Current.MainWindow = loginView;
            loginView.Show();
        }
        
    }
}

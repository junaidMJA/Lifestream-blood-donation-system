using System.Collections.ObjectModel;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.ViewModels
{
    public class AppointmentsViewModel : ViewModelBase
    {
        public ObservableCollection<AppointmentModel> Appointments { get; set; }

        public AppointmentsViewModel()
        {
            AppointmentRepository appointmentRepository = new AppointmentRepository();
            Appointments = appointmentRepository.GetAppointments();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.ViewModels
{
    public class PatientViewModel : ViewModelBase
    {
        public ObservableCollection<PatientModel> Patient;

        public PatientViewModel()
        {
            PatientRepository patientRepository = new PatientRepository();
            Patient = patientRepository.PatientGridBind();
        }
    }
}

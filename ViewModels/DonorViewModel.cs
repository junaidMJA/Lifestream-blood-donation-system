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
    public class DonorViewModel : ViewModelBase
    {
        public ObservableCollection<DonorModel> Donor;
        
        public DonorViewModel()
        {
            DonorRepository donorRepository= new DonorRepository();
            Donor = donorRepository.DonorGridBind();
        }
    }
}

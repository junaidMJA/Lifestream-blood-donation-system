using System.Collections.ObjectModel;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.ViewModels
{
    public class RequestViewModel : ViewModelBase
    {
        private ObservableCollection<RequestModel> requests;
        private RequestRepository requestRepository;

        public ObservableCollection<RequestModel> Requests
        {
            get { return requests; }
            set
            {
                requests = value;
            }
        }

        public RequestViewModel()
        {
            requestRepository = new RequestRepository();
            LoadRequests();
        }

        private void LoadRequests()
        {
            Requests = requestRepository.GetRequests();
        }
    }
}

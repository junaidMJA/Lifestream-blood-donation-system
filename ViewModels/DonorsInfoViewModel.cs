using System;
using System.Collections.ObjectModel;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.ViewModels
{
    public class DonorsInfoViewModel : ViewModelBase
    {
        private ObservableCollection<DonorModel> donors;
        private readonly DonorRepository donorRepository;

        public ObservableCollection<DonorModel> Donors
        {
            get { return donors; }
            set
            {
                donors = value;
                OnPropertyChanged(nameof(Donors));
            }
        }

        public DonorsInfoViewModel()
        {
            donorRepository = new DonorRepository();
            RefreshDonors();
        }

        public void AddDonor(string name, DateTime dateOfBirth, string gender, string bloodType, string contact, string address, string frequency, DateTime lastDonated)
        {
            // Add donor to the in-memory collection
            Donors.Add(new DonorModel()
            {
                ID = GetNextDonorId(),
                Name = name,
                DOB = dateOfBirth,
                Gender = gender,
                BloodType = bloodType,
                Contact = contact,
                Address = address,
                Frequency = frequency,
                LastDonated = lastDonated
            });
        }

        public void UpdateDonor(int donorId, string name, DateTime dateOfBirth, string gender, string bloodType, string contact, string address, string frequency, DateTime lastDonated)
        {
            // Update donor in the in-memory collection
            foreach (var donor in Donors)
            {
                if (donor.ID == donorId)
                {
                    donor.Name = name;
                    donor.DOB = dateOfBirth;
                    donor.Gender = gender;
                    donor.BloodType = bloodType;
                    donor.Contact = contact;
                    donor.Address = address;
                    donor.Frequency = frequency;
                    donor.LastDonated = lastDonated;
                    break;
                }
            }
        }

        public void DeleteDonor(int donorId)
        {
            // Remove donor from the in-memory collection
            foreach (var donor in Donors)
            {
                if (donor.ID == donorId)
                {
                    Donors.Remove(donor);
                    break;
                }
            }
        }

        public void RefreshDonors()
        {
            // Fetch donors from the database using the repository
            Donors = donorRepository.DonorGridBind();
        }

        private int GetNextDonorId()
        {
            // Generate a unique donor ID based on the existing donors' IDs
            int maxId = 0;
            foreach (var donor in Donors)
            {
                if (donor.ID > maxId)
                {
                    maxId = donor.ID;
                }
            }
            return maxId + 1;
        }
    }
}

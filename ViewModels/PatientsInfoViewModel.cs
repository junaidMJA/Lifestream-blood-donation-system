using System;
using System.Collections.ObjectModel;
using WPFApp.Models;
using WPFApp.Repositories;

namespace WPFApp.ViewModels
{
    public class PatientsInfoViewModel : ViewModelBase
    {
        private ObservableCollection<PatientModel> patients;
        private readonly PatientRepository patientRepository;

        public ObservableCollection<PatientModel> Patients
        {
            get { return patients; }
            set
            {
                patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }

        public PatientsInfoViewModel()
        {
            patientRepository = new PatientRepository();
            RefreshPatients();
        }

        public void AddPatient(string name, DateTime dateOfBirth, string bloodType, string gender, string contact, string frequency, string address)
        {
            // Add patient to the in-memory collection
            Patients.Add(new PatientModel()
            {
                ID = GetNextPatientId(),
                Name = name,
                DOB = dateOfBirth,
                BloodType = bloodType,
                Gender = gender,
                Contact = contact,
                Address = address,
                Frequency = frequency
            });
        }

        public void UpdatePatient(int patientId, string name, DateTime dateOfBirth, string bloodType, string gender, string contact, string frequency, string address)
        {
            // Update patient in the in-memory collection
            foreach (var patient in Patients)
            {
                if (patient.ID == patientId)
                {
                    patient.Name = name;
                    patient.DOB = dateOfBirth;
                    patient.BloodType = bloodType;
                    patient.Gender = gender;
                    patient.Contact = contact;
                    patient.Address = address;
                    patient.Frequency = frequency;
                    break;
                }
            }
        }

        public void DeletePatient(int patientId)
        {
            // Remove patient from the in-memory collection
            foreach (var patient in Patients)
            {
                if (patient.ID == patientId)
                {
                    Patients.Remove(patient);
                    break;
                }
            }
        }

        public void RefreshPatients()
        {
            // Fetch patients from the database using the repository
            Patients = patientRepository.PatientGridBind();
        }

        private int GetNextPatientId()
        {
            // Generate a unique patient ID based on the existing patients' IDs
            int maxId = 0;
            foreach (var patient in Patients)
            {
                if (patient.ID > maxId)
                {
                    maxId = patient.ID;
                }
            }
            return maxId + 1;
        }
    }
}

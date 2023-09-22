using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using WPFApp.Models;

namespace WPFApp.Repositories
{
    public class PatientRepository : RepositoryBase
    {
        public ObservableCollection<PatientModel> PatientGridBind()
        {
            ObservableCollection<PatientModel> patients = new ObservableCollection<PatientModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [BloodBank].[dbo].[Patient]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Patient_ID"));
                        string name = reader.GetString(reader.GetOrdinal("Patient_Name"));
                        DateTime dob = reader.GetDateTime(reader.GetOrdinal("Patient_DOB"));
                        string bloodType = reader.GetString(reader.GetOrdinal("Patient_BloodType"));
                        string gender = reader.GetString(reader.GetOrdinal("Patient_Gender"));
                        string contact = reader.GetString(reader.GetOrdinal("Patient_Contact"));
                        string address = reader.GetString(reader.GetOrdinal("Patient_Address"));
                        string frequency = reader.GetString(reader.GetOrdinal("Patient_Frequency"));

                        patients.Add(new PatientModel
                        {
                            ID = id,
                            Name = name,
                            DOB = dob,
                            BloodType = bloodType,
                            Gender = gender,
                            Contact = contact,
                            Address = address,
                            Frequency = frequency
                        });
                    }
                }

                connection.Close();
            }

            return patients;
        }

        public void AddPatient(string name, DateTime dateOfBirth, string bloodType, string gender, string contact, string frequency, string address)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [BloodBank].[dbo].[Patient] ([Patient_Name], [Patient_DOB], [Patient_BloodType], [Patient_Gender], [Patient_Contact], [Patient_Address], [Patient_Frequency]) " +
                                      "VALUES (@Name, @DOB, @BloodType, @Gender, @Contact, @Address, @Frequency)";

                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@DOB", dateOfBirth);
                command.Parameters.AddWithValue("@BloodType", bloodType);
                command.Parameters.AddWithValue("@Gender", gender);
                command.Parameters.AddWithValue("@Contact", contact);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@Frequency", frequency);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void UpdatePatient(int patientId, string name, DateTime? dateOfBirth, string bloodType, string gender, string contact, string frequency, string address)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [BloodBank].[dbo].[Patient] SET " +
                                      "[Patient_Name] = COALESCE(@Name, [Patient_Name]), " +
                                      "[Patient_BloodType] = COALESCE(@BloodType, [Patient_BloodType]), " +
                                      "[Patient_Gender] = COALESCE(@Gender, [Patient_Gender]), " +
                                      "[Patient_Contact] = COALESCE(@Contact, [Patient_Contact]), " +
                                      "[Patient_Address] = COALESCE(@Address, [Patient_Address]), " +
                                      "[Patient_Frequency] = COALESCE(@Frequency, [Patient_Frequency]) ";

                if (dateOfBirth.HasValue && dateOfBirth.Value != DateTime.MinValue)
                {
                    command.CommandText += ", [Patient_DOB] = @DOB ";
                    command.Parameters.AddWithValue("@DOB", dateOfBirth.Value);
                }

                command.CommandText += "WHERE [Patient_ID] = @PatientID";
                command.Parameters.AddWithValue("@PatientID", patientId);
                command.Parameters.AddWithValue("@Name", !string.IsNullOrEmpty(name) ? (object)name : DBNull.Value);
                command.Parameters.AddWithValue("@BloodType", !string.IsNullOrEmpty(bloodType) ? (object)bloodType : DBNull.Value);
                command.Parameters.AddWithValue("@Gender", !string.IsNullOrEmpty(gender) ? (object)gender : DBNull.Value);
                command.Parameters.AddWithValue("@Contact", !string.IsNullOrEmpty(contact) ? (object)contact : DBNull.Value);
                command.Parameters.AddWithValue("@Address", !string.IsNullOrEmpty(address) ? (object)address : DBNull.Value);
                command.Parameters.AddWithValue("@Frequency", !string.IsNullOrEmpty(frequency) ? (object)frequency : DBNull.Value);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeletePatient(int patientId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [BloodBank].[dbo].[Patient] WHERE [Patient_ID] = @PatientID";

                command.Parameters.AddWithValue("@PatientID", patientId);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public PatientModel GetPatientByID(int patientId)
        {
            PatientModel patient = null;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [BloodBank].[dbo].[Patient] WHERE [Patient_ID] = @PatientID";
                command.Parameters.AddWithValue("@PatientID", patientId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(reader.GetOrdinal("Patient_Name"));
                        DateTime dob = reader.GetDateTime(reader.GetOrdinal("Patient_DOB"));
                        string bloodType = reader.GetString(reader.GetOrdinal("Patient_BloodType"));
                        string gender = reader.GetString(reader.GetOrdinal("Patient_Gender"));
                        string contact = reader.GetString(reader.GetOrdinal("Patient_Contact"));
                        string address = reader.GetString(reader.GetOrdinal("Patient_Address"));
                        string frequency = reader.GetString(reader.GetOrdinal("Patient_Frequency"));

                        patient = new PatientModel
                        {
                            ID = patientId,
                            Name = name,
                            DOB = dob,
                            BloodType = bloodType,
                            Gender = gender,
                            Contact = contact,
                            Address = address,
                            Frequency = frequency
                        };
                    }
                }

                connection.Close();
            }

            return patient;
        }
    }
}

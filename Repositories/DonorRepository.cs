using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using WPFApp.Models;

namespace WPFApp.Repositories
{
    public class DonorRepository : RepositoryBase
    {
        public ObservableCollection<DonorModel> DonorGridBind()
        {
            ObservableCollection<DonorModel> donors = new ObservableCollection<DonorModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT [Donor_ID], [Donor_Name], [Donor_DOB], [Donor_Gender], [Donor_BloodType], [Donor_Contact], [Donor_Address], [Donor_Frequency], [Donor_LastDonated] FROM [BloodBank].[dbo].[Donor]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Donor_ID"));
                        string name = reader.GetString(reader.GetOrdinal("Donor_Name"));
                        DateTime dob = reader.GetDateTime(reader.GetOrdinal("Donor_DOB"));
                        string gender = reader.GetString(reader.GetOrdinal("Donor_Gender"));
                        string bloodType = reader.GetString(reader.GetOrdinal("Donor_BloodType"));
                        string contact = reader.GetString(reader.GetOrdinal("Donor_Contact"));
                        string address = reader.GetString(reader.GetOrdinal("Donor_Address"));
                        string frequency = reader.GetString(reader.GetOrdinal("Donor_Frequency"));
                        DateTime lastDonated = reader.GetDateTime(reader.GetOrdinal("Donor_LastDonated"));

                        donors.Add(new DonorModel
                        {
                            ID = id,
                            Name = name,
                            DOB = dob,
                            Gender = gender,
                            BloodType = bloodType,
                            Contact = contact,
                            Address = address,
                            Frequency = frequency,
                            LastDonated = lastDonated
                        });
                    }
                }

                connection.Close();
            }

            return donors;
        }

        public void AddDonor(string name, DateTime dateOfBirth, string gender, string bloodType, string contact, string address, string frequency, DateTime lastDonated)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [BloodBank].[dbo].[Donor] ([Donor_Name], [Donor_DOB], [Donor_Gender], [Donor_BloodType], [Donor_Contact], [Donor_Address], [Donor_Frequency], [Donor_LastDonated]) " +
                                      "VALUES (@Name, @DOB, @Gender, @BloodType, @Contact, @Address, @Frequency, @LastDonated)";

                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@DOB", dateOfBirth);
                command.Parameters.AddWithValue("@Gender", gender);
                command.Parameters.AddWithValue("@BloodType", bloodType);
                command.Parameters.AddWithValue("@Contact", contact);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@Frequency", frequency);
                command.Parameters.AddWithValue("@LastDonated", lastDonated);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void UpdateDonor(int donorId, string name, DateTime? dateOfBirth, string gender, string bloodType, string contact, string address, string frequency, DateTime lastDonated)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [BloodBank].[dbo].[Donor] SET " +
                                      "[Donor_Name] = COALESCE(@Name, [Donor_Name]), " +
                                      "[Donor_DOB] = COALESCE(@DOB, [Donor_DOB]), " +
                                      "[Donor_Gender] = COALESCE(@Gender, [Donor_Gender]), " +
                                      "[Donor_BloodType] = COALESCE(@BloodType, [Donor_BloodType]), " +
                                      "[Donor_Contact] = COALESCE(@Contact, [Donor_Contact]), " +
                                      "[Donor_Address] = COALESCE(@Address, [Donor_Address]), " +
                                      "[Donor_Frequency] = COALESCE(@Frequency, [Donor_Frequency]), " +
                                      "[Donor_LastDonated] = COALESCE(@LastDonated, [Donor_LastDonated]) " +
                                      "WHERE [Donor_ID] = @DonorID";

                command.Parameters.AddWithValue("@DonorID", donorId);
                command.Parameters.AddWithValue("@Name", !string.IsNullOrEmpty(name) ? (object)name : DBNull.Value);
                command.Parameters.AddWithValue("@DOB", dateOfBirth.HasValue && dateOfBirth.Value != DateTime.MinValue ? (object)dateOfBirth.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Gender", !string.IsNullOrEmpty(gender) ? (object)gender : DBNull.Value);
                command.Parameters.AddWithValue("@BloodType", !string.IsNullOrEmpty(bloodType) ? (object)bloodType : DBNull.Value);
                command.Parameters.AddWithValue("@Contact", !string.IsNullOrEmpty(contact) ? (object)contact : DBNull.Value);
                command.Parameters.AddWithValue("@Address", !string.IsNullOrEmpty(address) ? (object)address : DBNull.Value);
                command.Parameters.AddWithValue("@Frequency", !string.IsNullOrEmpty(frequency) ? (object)frequency : DBNull.Value);
                command.Parameters.AddWithValue("@LastDonated", lastDonated != DateTime.MinValue ? (object)lastDonated : DBNull.Value);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeleteDonor(int donorId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [BloodBank].[dbo].[Donor] WHERE [Donor_ID] = @DonorID";

                command.Parameters.AddWithValue("@DonorID", donorId);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<DonorModel> GetDonorsByNameAndBloodType(string name, string bloodType)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Build the query based on the provided name and blood type
                string query = "SELECT * FROM [BloodBank].[dbo].[Donor] WHERE 1 = 1";

                if (!string.IsNullOrEmpty(name) && name != "search...")
                {
                    query += " AND [Donor_Name] LIKE @Name";
                    command.Parameters.AddWithValue("@Name", $"%{name}%");
                }

                if (!string.IsNullOrEmpty(bloodType) && bloodType != "All Blood Types" && bloodType != "--Select Type--")
                {
                    query += " AND [Donor_BloodType] = @BloodType";
                    command.Parameters.AddWithValue("@BloodType", bloodType);
                }

                command.CommandText = query;

                var donors = new List<DonorModel>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var donor = new DonorModel
                        {
                            ID = Convert.ToInt32(reader["Donor_ID"]),
                            Name = reader["Donor_Name"].ToString(),
                            DOB = Convert.ToDateTime(reader["Donor_DOB"]),
                            Gender = reader["Donor_Gender"].ToString(),
                            BloodType = reader["Donor_BloodType"].ToString(),
                            Contact = reader["Donor_Contact"].ToString(),
                            Address = reader["Donor_Address"].ToString(),
                            Frequency = reader["Donor_Frequency"].ToString(),
                            LastDonated = Convert.ToDateTime(reader["Donor_LastDonated"])
                        };

                        donors.Add(donor);
                    }
                }

                connection.Close();
                return donors;
            }
        }

    }
}

using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using WPFApp.Models;

namespace WPFApp.Repositories
{
    public class RequestRepository : RepositoryBase
    {
        public ObservableCollection<RequestModel> GetRequests()
        {
            ObservableCollection<RequestModel> requests = new ObservableCollection<RequestModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT [Request_ID], [Request_BloodType], [Request_Quantity], [Request_PlaceDate], [Request_RequiredDate], [Request_Status], [Patient_ID] FROM [BloodBank].[dbo].[Request]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int requestId = reader.GetInt32(reader.GetOrdinal("Request_ID"));
                        string bloodType = reader.GetString(reader.GetOrdinal("Request_BloodType"));
                        int quantity = reader.GetInt32(reader.GetOrdinal("Request_Quantity"));
                        DateTime placeDate = reader.GetDateTime(reader.GetOrdinal("Request_PlaceDate"));
                        DateTime requiredDate = reader.GetDateTime(reader.GetOrdinal("Request_RequiredDate"));
                        string status = reader.GetString(reader.GetOrdinal("Request_Status"));
                        int patientId = reader.GetInt32(reader.GetOrdinal("Patient_ID"));

                        requests.Add(new RequestModel
                        {
                            RequestID = requestId,
                            BloodType = bloodType,
                            Quantity = quantity,
                            PlaceDate = placeDate,
                            RequiredDate = requiredDate,
                            Status = status,
                            PatientID = patientId
                        });
                    }
                }

                connection.Close();
            }

            return requests;
        }

        public bool AddRequest(int patientId, DateTime requestDate, int requestQuantity, out string errorMessage)
        {
            // Retrieve patient blood type
            string bloodType = GetPatientBloodType(patientId);

            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    // Prepare the SQL query
                    string query = "INSERT INTO [Request] ([Request_BloodType], [Request_Quantity], [Request_PlaceDate], [Request_RequiredDate], [Request_Status], [Patient_ID]) " +
                                   "VALUES (@BloodType, @RequestQuantity, GETDATE(), @RequestRequiredDate, 'Pending', @PatientId)";

                    // Create the SqlCommand object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the parameter values
                        command.Parameters.AddWithValue("@BloodType", bloodType);
                        command.Parameters.AddWithValue("@RequestQuantity", requestQuantity);
                        command.Parameters.AddWithValue("@RequestRequiredDate", requestDate);
                        command.Parameters.AddWithValue("@PatientId", patientId);

                        // Open the connection and execute the query
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool UpdateRequest(int requestId, int requestQuantity, DateTime requestDate, out string errorMessage)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    // Prepare the SQL query
                    string query = "UPDATE [Request] SET [Request_Quantity] = @RequestQuantity, [Request_RequiredDate] = @RequestRequiredDate WHERE [Request_ID] = @RequestId";

                    // Create the SqlCommand object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the parameter values
                        command.Parameters.AddWithValue("@RequestId", requestId);
                        command.Parameters.AddWithValue("@RequestQuantity", requestQuantity);
                        command.Parameters.AddWithValue("@RequestRequiredDate", requestDate);

                        // Open the connection and execute the query
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions here or log them
                errorMessage = ex.Message;
                return false;
            }
        }



        private string GetPatientBloodType(int patientId)
        {
            using (SqlConnection connection = GetConnection())
            {
                // Prepare the SQL query
                string query = "SELECT [Patient_BloodType] FROM [Patient] WHERE [Patient_ID] = @PatientId";

                // Create the SqlCommand object
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Set the parameter value
                    command.Parameters.AddWithValue("@PatientId", patientId);

                    // Open the connection and execute the query
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }


        public (bool, string) DeleteRequest(int requestId)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM [BloodBank].[dbo].[Request] WHERE [request_id] = @RequestId";

                    command.Parameters.AddWithValue("@RequestId", requestId);

                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        return (true, "");
                    }
                    else
                    {
                        return (false, "No request found with the specified ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


        public ObservableCollection<RequestModel> GetRequestsByBloodType(string bloodType)
        {
            ObservableCollection<RequestModel> requests = new ObservableCollection<RequestModel>();

            if (bloodType == "All Blood Types" || bloodType == "--Select Type--" || bloodType == null) {
                RequestRepository req = new RequestRepository();
                requests = req.GetRequests();
                return requests;
            }

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [BloodBank].[dbo].[Request] WHERE [Request_BloodType] = @BloodType";
                command.Parameters.AddWithValue("@BloodType", bloodType);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int requestId = reader.GetInt32(reader.GetOrdinal("Request_ID"));
                        string bbloodType = reader.GetString(reader.GetOrdinal("Request_BloodType"));
                        int quantity = reader.GetInt32(reader.GetOrdinal("Request_Quantity"));
                        DateTime placeDate = reader.GetDateTime(reader.GetOrdinal("Request_PlaceDate"));
                        DateTime requiredDate = reader.GetDateTime(reader.GetOrdinal("Request_RequiredDate"));
                        string status = reader.GetString(reader.GetOrdinal("Request_Status"));
                        int patientId = reader.GetInt32(reader.GetOrdinal("Patient_ID"));

                        requests.Add(new RequestModel
                        {
                            RequestID = requestId,
                            BloodType = bbloodType,
                            Quantity = quantity,
                            PlaceDate = placeDate,
                            RequiredDate = requiredDate,
                            Status = status,
                            PatientID = patientId
                        });
                    }
                }

                connection.Close();
            }

            return requests;
        }

        public ObservableCollection<RequestModel> GetRequestsByPatientName(string name)
        {
            ObservableCollection<RequestModel> requests = new ObservableCollection<RequestModel>();

            if (name == "Search..." || name == "search..." || name == null)
            {
                RequestRepository req = new RequestRepository();
                requests = req.GetRequests();
                return requests;
            }

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT R.* FROM [BloodBank].[dbo].[Request] R INNER JOIN [BloodBank].[dbo].[Patient] P ON R.[Patient_ID] = P.[Patient_ID] WHERE P.[Patient_Name] LIKE @Name";
                command.Parameters.AddWithValue("@Name", "%" + name + "%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int requestId = reader.GetInt32(reader.GetOrdinal("Request_ID"));
                        string bloodType = reader.GetString(reader.GetOrdinal("Request_BloodType"));
                        int quantity = reader.GetInt32(reader.GetOrdinal("Request_Quantity"));
                        DateTime placeDate = reader.GetDateTime(reader.GetOrdinal("Request_PlaceDate"));
                        DateTime requiredDate = reader.GetDateTime(reader.GetOrdinal("Request_RequiredDate"));
                        string status = reader.GetString(reader.GetOrdinal("Request_Status"));
                        int patientId = reader.GetInt32(reader.GetOrdinal("Patient_ID"));

                        requests.Add(new RequestModel
                        {
                            RequestID = requestId,
                            BloodType = bloodType,
                            Quantity = quantity,
                            PlaceDate = placeDate,
                            RequiredDate = requiredDate,
                            Status = status,
                            PatientID = patientId
                        });
                    }
                }

                connection.Close();
            }

            return requests;
        }
    }
}

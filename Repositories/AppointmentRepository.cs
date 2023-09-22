using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using WPFApp.Models;

namespace WPFApp.Repositories
{
    public class AppointmentRepository : RepositoryBase
    {
        public ObservableCollection<AppointmentModel> GetAppointments()
        {
            ObservableCollection<AppointmentModel> appointments = new ObservableCollection<AppointmentModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT [id], [appo_name], [appo_placedate], [appo_datetime], [appo_status], [donor_id] FROM [BloodBank].[dbo].[Appointment]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string name = reader.GetString(reader.GetOrdinal("appo_name"));
                        DateTime placementDate = reader.GetDateTime(reader.GetOrdinal("appo_placedate"));
                        DateTime appointmentDate = reader.GetDateTime(reader.GetOrdinal("appo_datetime"));
                        string status = reader.GetString(reader.GetOrdinal("appo_status"));
                        int donorId = reader.GetInt32(reader.GetOrdinal("donor_id"));

                        appointments.Add(new AppointmentModel
                        {
                            ID = id,
                            Name = name,
                            PlacementDate = placementDate,
                            AppointmentDate = appointmentDate,
                            Status = status,
                            DonorID = donorId
                        });
                    }
                }

                connection.Close();
            }

            return appointments;
        }

        public bool AddAppointment(string name, DateTime placementDate, DateTime appointmentDate, string status, int donorId, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO [BloodBank].[dbo].[Appointment] ([appo_name], [appo_placedate], [appo_datetime], [appo_status], [donor_id]) " +
                                          "VALUES (@Name, @PlacementDate, @AppointmentDate, @Status, @DonorID)";

                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@PlacementDate", placementDate);
                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@DonorID", donorId);

                    command.ExecuteNonQuery();
                    connection.Close();

                    return true; // Appointment added successfully
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false; // Error occurred while adding the appointment
            }
        }

        public (bool success, string errorMessage) EditAppointment(int appointmentId, DateTime appointmentDate)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE [BloodBank].[dbo].[Appointment] SET [appo_datetime] = @AppointmentDate WHERE [id] = @AppointmentId";

                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    command.Parameters.AddWithValue("@AppointmentId", appointmentId);

                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                        return (true, string.Empty);
                    else
                        return (false, "No appointment found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                Console.WriteLine($"Error: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }

        public (bool success, string errorMessage) DeleteAppointment(int appointmentId)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM [BloodBank].[dbo].[Appointment] WHERE [id] = @AppointmentId";

                    command.Parameters.AddWithValue("@AppointmentId", appointmentId);

                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                        return (true, string.Empty);
                    else
                        return (false, "No appointment found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                Console.WriteLine($"Error: {ex.Message}");
                return (false, $"Error: {ex.Message}");
            }
        }
    }
}

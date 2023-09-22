using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WPFApp.Models;

namespace WPFApp.Repositories
{
    public class HomeRepository : RepositoryBase
    {
        public IEnumerable<BloodTypeGraphData> GetBloodTypeGraphData(DateTime startDate, DateTime endDate, int intervalDays)
        {
            string query = @"SELECT GraphDate, BloodType, DonationCount
                             FROM dbo.GetBloodTypeGraphData(@StartDate, @EndDate, @IntervalDays)";

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@IntervalDays", intervalDays);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<BloodTypeGraphData> bloodTypeGraphData = new List<BloodTypeGraphData>();

                        while (reader.Read())
                        {
                            BloodTypeGraphData data = new BloodTypeGraphData
                            {
                                GraphDate = reader.GetDateTime(0),
                                BloodType = reader.GetString(1),
                                DonationCount = reader.GetInt32(2)
                            };

                            bloodTypeGraphData.Add(data);
                        }

                        return bloodTypeGraphData;
                    }
                }
            }
        }

        public List<BloodTypeCount> CountBlood(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = @"SELECT DonatedBlood, ReceivedBlood, Receipt_BloodType
                         FROM dbo.countblood(@startdate, @enddate)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@startdate", startDate);
                    command.Parameters.AddWithValue("@enddate", endDate);

                    List<BloodTypeCount> bloodTypeCounts = new List<BloodTypeCount>();

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        BloodTypeCount bloodTypeCount = new BloodTypeCount
                        {
                            DonatedBlood = reader.GetInt32(0),
                            ReceivedBlood = reader.GetInt32(1),
                            BloodType = reader.GetString(2)
                        };

                        bloodTypeCounts.Add(bloodTypeCount);
                    }

                    return bloodTypeCounts;
                }
            }
        }

        public DataTable GetGenderCounts()
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = @"SELECT * FROM dbo.GetGenderCounts()";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public int CountPendingAppointments(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT [dbo].[CountPendingAppointments](@startDate, @endDate)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);

                connection.Open();
                int count = (int)command.ExecuteScalar();

                return count;
            }
        }

        public int CountPendingRequests(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT [dbo].[CountPendingRequests](@startDate, @endDate)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);

                connection.Open();
                int count = (int)command.ExecuteScalar();

                return count;
            }
        }

    }
}

using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using WPFApp.Models;
using System.Linq;
using System.Collections.Generic;

namespace WPFApp.Repositories
{
    public class ReceiptRepository : RepositoryBase
    {
        public ObservableCollection<ReceiptModel> GetReceipts()
        {
            ObservableCollection<ReceiptModel> receipts = new ObservableCollection<ReceiptModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT [Receipt_ID], [Receipt_DateTime], [Receipt_BloodType], [Receipt_Quantity], [Receipt_BagHealth], [Receipt_Process], [Staff_ID], [Request_ID], [Appo_ID] FROM [BloodBank].[dbo].[Receipt]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int receiptId = reader.GetInt32(reader.GetOrdinal("Receipt_ID"));
                        DateTime receiptDate = reader.GetDateTime(reader.GetOrdinal("Receipt_DateTime"));
                        string bloodType = reader.GetString(reader.GetOrdinal("Receipt_BloodType"));
                        int quantity = reader.GetInt32(reader.GetOrdinal("Receipt_Quantity"));
                        string bagHealth = reader.GetString(reader.GetOrdinal("Receipt_BagHealth"));
                        string process = reader.GetString(reader.GetOrdinal("Receipt_Process"));
                        int? staffId = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("Staff_ID")))
                        {
                            staffId = reader.GetInt32(reader.GetOrdinal("Staff_ID"));
                        }

                        int? requestId = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("Request_ID")))
                        {
                            requestId = reader.GetInt32(reader.GetOrdinal("Request_ID"));
                        }

                        int? appointmentId = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("Appo_ID")))
                        {
                            appointmentId = reader.GetInt32(reader.GetOrdinal("Appo_ID"));
                        }


                        receipts.Add(new ReceiptModel
                        {
                            ReceiptID = receiptId,
                            ReceiptDate = receiptDate,
                            BloodType = bloodType,
                            Quantity = quantity,
                            BagHealth = bagHealth,
                            Process = process,
                            StaffID = staffId,
                            RequestID = requestId,
                            AppointmentID = appointmentId
                        });
                    }
                }

                connection.Close();
            }

            return receipts;
        }

        public bool AddReceipt(DateTime dateTime, string bloodType, int quantity, string process, int staffId, int appointmentId, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    string query = "";
                    if (process == "Donation")
                    {
                        query = "INSERT INTO [Receipt] ([Receipt_DateTime], [Receipt_BloodType], [Receipt_Quantity], [Receipt_BagHealth], [Receipt_Process], [Staff_ID], [Request_ID], [Appo_ID]) " +
                                   "VALUES (@DateTime, @BloodType, @Quantity, 'Fresh', @Process, @StaffId, NULL, @AppointmentId)";
                    }
                    else
                    {
                        query = "INSERT INTO [Receipt] ([Receipt_DateTime], [Receipt_BloodType], [Receipt_Quantity], [Receipt_BagHealth], [Receipt_Process], [Staff_ID], [Request_ID], [Appo_ID]) " +
                                   "VALUES (@DateTime, @BloodType, @Quantity, 'Fresh', @Process, @StaffId, @AppointmentId, NULL)";
                    }
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DateTime", dateTime);
                        command.Parameters.AddWithValue("@BloodType", bloodType);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Process", process);
                        command.Parameters.AddWithValue("@StaffId", staffId);
                        command.Parameters.AddWithValue("@AppointmentId", appointmentId);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public List<ReceiptModel> GetReceiptsByBloodType(string bloodType)
        {
            var receipts = GetReceipts();

            if (bloodType == "--Select Type--" || bloodType == "All Blood Types" || bloodType == null)
            {
                return receipts.ToList();
            }

            // Filter the receipts based on the specified blood type
            var matchingReceipts = receipts.Where(r => r.BloodType == bloodType).ToList();

            return matchingReceipts;
        }
    }
}

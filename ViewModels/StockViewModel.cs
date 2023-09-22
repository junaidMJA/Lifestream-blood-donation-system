using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WPFApp.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        private ObservableCollection<StockItem> stockItems;

        public ObservableCollection<StockItem> StockItems
        {
            get { return stockItems; }
            set
            {
                stockItems = value;
                OnPropertyChanged(nameof(StockItems));
            }
        }

        public StockViewModel() 
        {
            GetStockItems("All Bags");
        }

        public void GetStockItems(string bagHealth)
        {
            string connectionString = "Server=tcp:database-server-bds.database.windows.net,1433;Initial Catalog=BloodBank;Persist Security Info=False;User ID=maiznadeem;Password=SnC2ApayPpi48b7;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM dbo.BloodStock(@baghealth)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the parameter to the command
                    command.Parameters.AddWithValue("@baghealth", bagHealth);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        StockItems = new ObservableCollection<StockItem>();

                        while (reader.Read())
                        {
                            StockItems.Add(new StockItem
                            {
                                Stock = (int)reader["Stock"],
                                BagType = reader["Bag_Type"].ToString()
                            });
                        }
                    }
                }
            }
        }
    }

    public class StockItem
    {
        public int Stock { get; set; }
        public string BagType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using WPFApp.Models;

namespace WPFApp.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {

        public void AddStaff(UserModel user)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [Staff] ([staff_name], [gender], [designation], [salary], [DOB], [contact], [staff_shift], [username], [password], [photo]) " +
                    "VALUES (@staffName, @gender, @designation, @salary, @dob, @contact, @shift, @username, @password, @photo)";
                command.Parameters.Add("@staffName", SqlDbType.NVarChar).Value = user.StaffName;
                command.Parameters.Add("@gender", SqlDbType.NVarChar).Value = user.Gender;
                command.Parameters.Add("@designation", SqlDbType.NVarChar).Value = user.Designation;
                command.Parameters.Add("@salary", SqlDbType.Int).Value = user.Salary;
                command.Parameters.Add("@dob", SqlDbType.DateTime).Value = user.DOB;
                command.Parameters.Add("@contact", SqlDbType.NVarChar).Value = user.Contact;
                command.Parameters.Add("@shift", SqlDbType.NVarChar).Value = user.StaffShift;
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = user.Password;
                command.Parameters.Add("@photo", SqlDbType.VarBinary).Value = user.Photo;

                command.ExecuteNonQuery();
            }
        }

        public void UpdateStaff(UserModel user)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [Staff] SET [staff_name] = @staffName, [gender] = @gender, [designation] = @designation, [salary] = @salary, " +
                    "[DOB] = @dob, [contact] = @contact, [staff_shift] = @shift, [username] = @username, [password] = @password, [photo] = @photo " +
                    "WHERE [id] = @id";
                command.Parameters.Add("@staffName", SqlDbType.NVarChar).Value = user.StaffName;
                command.Parameters.Add("@gender", SqlDbType.NVarChar).Value = user.Gender;
                command.Parameters.Add("@designation", SqlDbType.NVarChar).Value = user.Designation;
                command.Parameters.Add("@salary", SqlDbType.Int).Value = user.Salary;
                command.Parameters.Add("@dob", SqlDbType.DateTime).Value = user.DOB;
                command.Parameters.Add("@contact", SqlDbType.NVarChar).Value = user.Contact;
                command.Parameters.Add("@shift", SqlDbType.NVarChar).Value = user.StaffShift;
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = user.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = user.Password;
                command.Parameters.Add("@photo", SqlDbType.VarBinary).Value = user.Photo;
                command.Parameters.Add("@id", SqlDbType.Int).Value = user.Id;

                command.ExecuteNonQuery();
            }
        }

        public void DeleteStaff(int id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [Staff] WHERE [id] = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                command.ExecuteNonQuery();
            }
        }


        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM [Staff] WHERE [username] = @username AND [password] = @password";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = (int)command.ExecuteScalar() > 0;
            }
            return validUser;
        }

        public IEnumerable<UserModel> GetByAll()
        {
            List<UserModel> users = new List<UserModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Staff]";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel()
                        {
                            Id = reader["id"].ToString(),
                            StaffName = reader.IsDBNull(reader.GetOrdinal("staff_name")) ? string.Empty : reader["staff_name"].ToString(),
                            Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? string.Empty : reader["gender"].ToString(),
                            Designation = reader.IsDBNull(reader.GetOrdinal("designation")) ? string.Empty : reader["designation"].ToString(),
                            Salary = reader.IsDBNull(reader.GetOrdinal("salary")) ? 0 : (int)reader["salary"],
                            DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? DateTime.MinValue : (DateTime)reader["DOB"],
                            Contact = reader.IsDBNull(reader.GetOrdinal("contact")) ? string.Empty : reader["contact"].ToString(),
                            StaffShift = reader.IsDBNull(reader.GetOrdinal("staff_shift")) ? string.Empty : reader["staff_shift"].ToString(),
                            UserName = reader.IsDBNull(reader.GetOrdinal("username")) ? string.Empty : reader["username"].ToString(),
                            Password = reader.IsDBNull(reader.GetOrdinal("password")) ? string.Empty : reader["password"].ToString(),
                            Photo = reader.IsDBNull(reader.GetOrdinal("photo")) ? null : (byte[])reader["photo"]
                        };

                        users.Add(user);
                    }
                }
            }
            return users;
        }

        public UserModel GetById(int id)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Staff] WHERE [id] = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader["id"].ToString(),
                            StaffName = reader.IsDBNull(reader.GetOrdinal("staff_name")) ? string.Empty : reader["staff_name"].ToString(),
                            Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? string.Empty : reader["gender"].ToString(),
                            Designation = reader.IsDBNull(reader.GetOrdinal("designation")) ? string.Empty : reader["designation"].ToString(),
                            Salary = reader.IsDBNull(reader.GetOrdinal("salary")) ? 0 : (int)reader["salary"],
                            DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? DateTime.MinValue : (DateTime)reader["DOB"],
                            Contact = reader.IsDBNull(reader.GetOrdinal("contact")) ? string.Empty : reader["contact"].ToString(),
                            StaffShift = reader.IsDBNull(reader.GetOrdinal("staff_shift")) ? string.Empty : reader["staff_shift"].ToString(),
                            UserName = reader.IsDBNull(reader.GetOrdinal("username")) ? string.Empty : reader["username"].ToString(),
                            Password = reader.IsDBNull(reader.GetOrdinal("password")) ? string.Empty : reader["password"].ToString(),
                            Photo = reader.IsDBNull(reader.GetOrdinal("photo")) ? null : (byte[])reader["photo"]
                        };
                    }
                }
            }
            return user;
        }


        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [Staff] WHERE [username] = @username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader["id"].ToString(),
                            StaffName = reader.IsDBNull(reader.GetOrdinal("staff_name")) ? string.Empty : reader["staff_name"].ToString(),
                            Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? string.Empty : reader["gender"].ToString(),
                            Designation = reader.IsDBNull(reader.GetOrdinal("designation")) ? string.Empty : reader["designation"].ToString(),
                            Salary = reader.IsDBNull(reader.GetOrdinal("salary")) ? 0 : (int)reader["salary"],
                            DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? DateTime.MinValue : (DateTime)reader["DOB"],
                            Contact = reader.IsDBNull(reader.GetOrdinal("contact")) ? string.Empty : reader["contact"].ToString(),
                            StaffShift = reader.IsDBNull(reader.GetOrdinal("staff_shift")) ? string.Empty : reader["staff_shift"].ToString(),
                            UserName = reader.IsDBNull(reader.GetOrdinal("username")) ? string.Empty : reader["username"].ToString(),
                            Password = reader.IsDBNull(reader.GetOrdinal("password")) ? string.Empty : reader["password"].ToString(),
                            Photo = reader.IsDBNull(reader.GetOrdinal("photo")) ? null : (byte[])reader["photo"]
                        };
                    }
                }
            }
            return user;
        }

        public void UpdateAllPhotosToDefault()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [Staff] SET [photo] = @photo WHERE [photo] = NULL";
                command.Parameters.Add("@photo", SqlDbType.Image).Value = GetDefaultPhotoBytes();
                command.ExecuteNonQuery();
            }
        }

        private byte[] GetDefaultPhotoBytes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = "WPFApp.Images.myprofile.jpg"; // Replace "WPFApp" with your project namespace
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            return null;
        }

        public bool UpdateUser(UserModel user)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();

                    // Prepare the SQL update statement
                    string query = "UPDATE [Staff] SET [staff_name] = @staffName, [gender] = @gender, [designation] = @designation, [salary] = @salary, " +
                    "[DOB] = @dob, [contact] = @contact, [staff_shift] = @shift, [username] = @username, [password] = @password, [photo] = @photo " +
                    "WHERE [id] = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the parameter values
                        command.Parameters.AddWithValue("@id", user.Id);
                        command.Parameters.AddWithValue("@staffName", user.StaffName);
                        command.Parameters.AddWithValue("@dob", user.DOB);
                        command.Parameters.AddWithValue("@gender", user.Gender);
                        command.Parameters.AddWithValue("@designation", user.Designation);
                        command.Parameters.AddWithValue("@shift", user.StaffShift);
                        command.Parameters.AddWithValue("@salary", user.Salary);
                        command.Parameters.AddWithValue("@contact", user.Contact);
                        command.Parameters.AddWithValue("@username", user.UserName);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@photo", user.Photo);

                        // Execute the update statement
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the database update
                // Log the exception for further investigation
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


    }
}
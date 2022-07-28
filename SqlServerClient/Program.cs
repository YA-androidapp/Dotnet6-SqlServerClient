using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace SqlServerClient
{
    internal class Program
    {
        /// <summary>
        /// Contains the application's configuration settings. 
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor. Loads the application configuration settings from a JSON file.
        /// </summary>
        Program()
        {
            // Load the app's configuration settings from the JSON file.
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        static void Main(string[] args)
        {
            Program app = new();

            var connectionString = app.Configuration.GetConnectionString("MyConn");

            try
            {
                // SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                // builder.DataSource = "<your_server>.database.windows.net";
                // builder.UserID = "<your_username>";
                // builder.Password = "<your_password>";
                // builder.InitialCatalog = "<your_database>";

                // using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    Console.WriteLine("\nProducts:");
                    Console.WriteLine("=========================================\n");

                    String sql = "SELECT ProductID , Name FROM [SalesLT].[Product]";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}

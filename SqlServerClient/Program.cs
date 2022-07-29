using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Collections.Generic;


// Generate models
// Scaffold-DbContext -connection "Data Source=..." -Provider "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir Models


namespace SqlServerClient
{
    internal class Program
    {
        private static string connectionString = "";

        const string SQL_SELECT_PRODUCTS = "SELECT * FROM [SalesLT].[Product]";

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

        static void GetRecords()
        {
            var models = new List<Models.Product>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(SQL_SELECT_PRODUCTS, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
                            var c = reader[nameof(Models.Product.SellEndDate)];
                            var cc = c == DBNull.Value;

                            models.Add(new Models.Product
                            {
                                ProductId = (int)reader[nameof(Models.Product.ProductId)],
                                Name = reader[nameof(Models.Product.Name)].ToString(),
                                ProductNumber = reader[nameof(Models.Product.ProductNumber)].ToString(),
                                Color = reader[nameof(Models.Product.Color)].ToString(),
                                StandardCost = (Decimal)reader[nameof(Models.Product.StandardCost)],
                                ListPrice = (Decimal)reader[nameof(Models.Product.ListPrice)],
                                Size = reader[nameof(Models.Product.Size)].ToString(),
                                Weight = reader[nameof(Models.Product.Weight)] == DBNull.Value ? null : (Decimal)reader[nameof(Models.Product.Weight)],
                                ProductCategoryId = reader[nameof(Models.Product.ProductCategoryId)] == DBNull.Value ? null : (int)reader[nameof(Models.Product.ProductCategoryId)],
                                ProductModelId = reader[nameof(Models.Product.ProductModelId)] == DBNull.Value ? null : (int)reader[nameof(Models.Product.ProductModelId)],
                                SellStartDate = (DateTime)reader[nameof(Models.Product.SellStartDate)],
                                SellEndDate = reader[nameof(Models.Product.SellEndDate)] == DBNull.Value ? null : (DateTime)reader[nameof(Models.Product.SellEndDate)],
                                DiscontinuedDate = reader[nameof(Models.Product.DiscontinuedDate)] == DBNull.Value ? null : (DateTime)reader[nameof(Models.Product.DiscontinuedDate)],
                                ThumbNailPhoto = (byte[])reader[nameof(Models.Product.ThumbNailPhoto)],
                                ThumbnailPhotoFileName = reader[nameof(Models.Product.ThumbnailPhotoFileName)].ToString(),
                                Rowguid = (Guid)reader[nameof(Models.Product.Rowguid)],
                                ModifiedDate = (DateTime)reader[nameof(Models.Product.ModifiedDate)]
                            });
                        }
                    }
                }
                Console.WriteLine(models.Count);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void GetRecordsPopulator()
        {
            var models = new List<Models.Product>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(SQL_SELECT_PRODUCTS, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        models = new GenericPopulator<Models.Product>().CreateList(reader);
                    }
                }
                Console.WriteLine(models.Count);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void Main(string[] args)
        {
            Program app = new();
            connectionString = app.Configuration.GetConnectionString("MyConn");

            GetRecords();
            GetRecordsPopulator();
        }
    }
}

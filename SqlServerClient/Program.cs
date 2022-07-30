using System;
using Microsoft.Extensions.Configuration;


// Generate models
// Scaffold-DbContext -connection "Data Source=..." -Provider "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir Models


namespace SqlServerClient
{
    internal class Program
    {
        private static string connectionString = "";


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
            connectionString = app.Configuration.GetConnectionString("MyConn");

            RawSqlUtil.GetRecords(connectionString);
            RawSqlUtil.GetRecordsPopulator(connectionString);
            RawSqlUtil.AddRecord(connectionString, "Contoso", "Contoso", 12345, 12345, DateTime.Today, Guid.NewGuid(), DateTime.Today);
            RawSqlUtil.ModifyRecordsByName(connectionString, "Contoso", "Contoso2");
            RawSqlUtil.RemoveRecordsByName(connectionString, "Contoso");
        }
    }
}

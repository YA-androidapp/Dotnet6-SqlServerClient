using System;
using Microsoft.Extensions.Configuration;


// Generate models
// Scaffold-DbContext -connection "Data Source=..." -Provider "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir Models


namespace SqlServerClient
{
    // Program => Program_RawSql
    internal class Program_RawSql
    {
        private static string connectionString = "";


        /// <summary>
        /// Contains the application's configuration settings. 
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor. Loads the application configuration settings from a JSON file.
        /// </summary>
        Program_RawSql()
        {
            // Load the app's configuration settings from the JSON file.
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        // Main => Main_RawSql
        static void Main_RawSql(string[] args)
        {
            Program_RawSql app = new();
            connectionString = app.Configuration.GetConnectionString("MyConn");

            RawSqlUtil.GetRecords(connectionString);
            RawSqlUtil.GetRecordsPopulator(connectionString);
            RawSqlUtil.AddRecord(connectionString, "Contoso", "Contoso", 12345, 12345, DateTime.Today, Guid.NewGuid(), DateTime.Today);
            RawSqlUtil.ModifyRecordsByName(connectionString, "Contoso", "Contoso2");
            RawSqlUtil.RemoveRecordsByName(connectionString, "Contoso");
        }
    }
}

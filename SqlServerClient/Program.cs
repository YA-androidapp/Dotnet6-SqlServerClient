using System;
using Microsoft.Extensions.Configuration;


// Generate models
// Scaffold-DbContext -connection "Data Source=..." -Provider "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir Models


namespace SqlServerClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new Models.mydbContext();
            foreach (var product in db.Products)
            {
                Console.WriteLine(product.Name);
            }
        }
    }
}

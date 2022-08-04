using System;
using System.Linq;
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

            foreach (var product in DbUtil.GetRecords(db))
            {
                Console.WriteLine(product.Name);
            }
            
            DbUtil.AddRecord(
                db,
                new Models.Product("Contoso", "Contoso", 12345, 12345, DateTime.Today, Guid.NewGuid(), DateTime.Today)
            );

            DbUtil.ModifyRecordsByName(db, "Contoso", "34567");

            DbUtil.RemoveRecordsByName(db, "Contoso");
        }
    }
}

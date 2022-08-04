using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerClient
{
    public class DbUtil
    {
        public static List<Models.Product> GetRecords(Models.mydbContext db)
        {
            var products = new List<Models.Product>();

            foreach (var product in db.Products)
            {
                products.Add(product);
            }

            Console.WriteLine(products.Count);

            return products;
        }
        
        public static void AddRecord(Models.mydbContext db, Models.Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            Console.WriteLine(db.Products.Count());
        }

        public static void ModifyRecordsByName(Models.mydbContext db, string Name, string ProductNumber)
        {
            var product = db.Products.Single(x => x.Name == Name);
            product.ProductNumber = ProductNumber;
            db.SaveChanges();
            Console.WriteLine(db.Products.Count());
        }

        public static void RemoveRecordsByName(Models.mydbContext db, string Name)
        {
            var product = db.Products.Single(x => x.Name == Name);
            db.Products.Remove(product);
            db.SaveChanges();
            Console.WriteLine(db.Products.Count());
        }
    }
}

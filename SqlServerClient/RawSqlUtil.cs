using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace SqlServerClient
{
    public class RawSqlUtil
    {
        const string SQL_DELETE_PRODUCTS = "DELETE FROM [SalesLT].[Product] WHERE ProductID = @ProductID;";
        const string SQL_DELETE_PRODUCTS_BY_NAME = "DELETE FROM [SalesLT].[Product] WHERE Name = @Name;";
        const string SQL_INSERT_PRODUCT = "INSERT INTO [SalesLT].[Product] ( Name , ProductNumber , StandardCost , ListPrice , SellStartDate , rowguid , ModifiedDate ) VALUES ( @Name , @ProductNumber , @StandardCost , @ListPrice , @SellStartDate , @rowguid , @ModifiedDate ) ;";
        const string SQL_SELECT_PRODUCTS = "SELECT * FROM [SalesLT].[Product] ;";

        const string SQL_UPDATE_PRODUCTS_BY_NAME = "UPDATE [SalesLT].[Product] SET ProductNumber = @ProductNumber WHERE Name = @Name;";


        public static void AddRecord(string connectionString, string Name, string ProductNumber, int StandardCost, int ListPrice, DateTime SellStartDate, Guid rowguid, DateTime ModifiedDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(SQL_INSERT_PRODUCT, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                    command.Parameters.Add("@ProductNumber", SqlDbType.NVarChar).Value = ProductNumber;
                    command.Parameters.Add("@StandardCost", SqlDbType.Money).Value = StandardCost;
                    command.Parameters.Add("@ListPrice", SqlDbType.Money).Value = ListPrice;
                    command.Parameters.Add("@SellStartDate", SqlDbType.DateTime).Value = SellStartDate;
                    command.Parameters.Add("@rowguid", SqlDbType.UniqueIdentifier).Value = rowguid;
                    command.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = ModifiedDate;
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void GetRecords(string connectionString)
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
                                ThumbNailPhoto = reader[nameof(Models.Product.ThumbNailPhoto)] == DBNull.Value ? null : (byte[])reader[nameof(Models.Product.ThumbNailPhoto)],
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

        public static void GetRecordsPopulator(string connectionString)
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

        public static void ModifyRecordsByName(string connectionString, string Name, string ProductNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(SQL_UPDATE_PRODUCTS_BY_NAME, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                    command.Parameters.Add("@ProductNumber", SqlDbType.NVarChar).Value = ProductNumber;
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void RemoveRecordsByName(string connectionString, string Name)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(SQL_DELETE_PRODUCTS_BY_NAME, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

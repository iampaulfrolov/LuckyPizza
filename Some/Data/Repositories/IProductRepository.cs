using System.Collections.Generic;
using System.Threading.Tasks;
using CourseProject.Data.Repositories;
using CourseProject.Models;
using CourseProject.Models.DataModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategory(int categoryId, int userId);
}

public class ProductRepository : AdoNetRepository<Product>, IRepository<Product>, IProductRepository
{
    public ProductRepository(IOptions<Settings> option) : base(option)
    {
    }

    public async Task<IEnumerable<Product>> GetByCategory(int categoryId, int userId)
    {
        var products = new List<Product>();
        if (categoryId == 1002)
        {
            var sqlExpression = @$"SELECT p.Description, p.Title, p.Price, p.Quantity, p.Image, p.Id FROM dbo.Product p JOIN dbo.OrderProduct op ON p.Id = op.product_id JOIN dbo.""Order"" o ON o.Id = op.order_id WHERE o.user_id = {userId}";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            Description = reader.GetString(0),
                            Title = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Quantity = reader.GetInt32(3),
                            Image = reader.GetString(4),
                            Id = reader.GetInt32(5)
                        };

                        products.Add(product);
                    }
                }

                reader.Close();
            }

            return products;
        }


        return await GetMany(p => p.Category.Id, categoryId);
    }
}
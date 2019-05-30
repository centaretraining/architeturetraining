using System;
using System.Configuration;
using System.Data.SqlClient;

namespace LegacyApp.Web.Application.Cart
{
    public class CartService : ICartService
    {
        public void AddItemToCart(int cartId, int productId)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                con.Open();

                int count;
                var sql = "SELECT COUNT(*) FROM dbo.CartItem WHERE CartId = @CartId AND ProductId = @ProductId";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (count == 0)
                {
                    sql = "INSERT INTO dbo.CartItem (CartId, ProductId, Quantity)" +
                      " VALUES (@CartId, @ProductId, @Quantity)";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@Quantity", 1);

                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        sql = "UPDATE dbo.CartItem SET Quantity = Quantity + 1 WHERE CartId = @CartId AND ProductId = @ProductId";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public int CreateCart()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                con.Open();

                var sql = "INSERT INTO dbo.Cart (CreateDate, LastUpdate) VALUES (GETDATE(), GETDATE());" +
                          " SELECT SCOPE_IDENTITY();";
                using (var cmd = new SqlCommand(sql, con))
                {
                    var cartId = Convert.ToInt32(cmd.ExecuteScalar());
                    return cartId;
                }
            }
        }
    }
}

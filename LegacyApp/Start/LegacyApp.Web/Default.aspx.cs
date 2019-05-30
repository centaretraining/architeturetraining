using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace LegacyApp.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadMenu();
                LoadCart();
            }
        }

        private void LoadCart()
        {
            if (Session["CartId"] == null)
            {
                CartButton.Visible = false;
                CartTotalLabel.Visible = false;
                return;
            }

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                var sql = "SELECT SUM(p.Price * i.Quantity)" +
                          " FROM dbo.CartItem i " +
                          " INNER JOIN dbo.Product p ON p.ProductId = i.ProductId" +
                          " WHERE CartId = @CartId";
                using (var cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@CartId", Session["CartId"]);
                    var total = cmd.ExecuteScalar();
                    if (total == DBNull.Value)
                    {
                        CartButton.Visible = false;
                        CartTotalLabel.Visible = false;
                    }
                    else
                    {
                        CartTotalLabel.Text = ((decimal)total).ToString("C");
                    }
                }
            }
        }

        protected void MenuRepeater_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var productId = int.Parse(e.CommandArgument.ToString());
            int cartId;
            if (Session["CartId"] == null)
            {
                // New cart
                cartId = CreateCart();
                Session["CartId"] = cartId;
            }
            else
            {
                cartId = Convert.ToInt32(Session["CartId"]);
            }
            AddItemToCart(cartId, productId);

            Response.Redirect("Cart.aspx");
        }

        protected void CartButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Cart.aspx");
        }

        private void LoadMenu()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                var sql = "SELECT ProductId, Name, Price FROM dbo.Product ORDER BY Name";
                using (var cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        MenuRepeater.DataSource = rdr;
                        MenuRepeater.DataBind();
                    }
                }
            }
        }

        private void AddItemToCart(int cartId, int productId)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                con.Open();

                string sql = "";

                // TODO: Increment quantity if adding an existing item
                //int count;
                //sql = "SELECT COUNT(*) FROM dbo.CartItem WHERE CartId = @CartId AND ProductId = @ProductId";
                //using (var cmd = new SqlCommand(sql, con))
                //{
                //    cmd.Parameters.AddWithValue("@CartId", cartId);
                //    cmd.Parameters.AddWithValue("@ProductId", productId);
                //    count = Convert.ToInt32(cmd.ExecuteScalar());
                //}

                //if (count == 0)
                //{
                sql = "INSERT INTO dbo.CartItem (CartId, ProductId, Quantity)" +
                      " VALUES (@CartId, @ProductId, @Quantity)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@Quantity", 1);

                    cmd.ExecuteNonQuery();
                }
                //}
                //else
                //{
                //    using (var cmd = new SqlCommand(sql, con))
                //    {
                //        sql = "UPDATE dbo.CartItem SET Quantity = Quantity + 1 WHERE CartId = @CartId AND ProductId = @ProductId";
                //        cmd.CommandText = sql;
                //        cmd.Parameters.AddWithValue("@CartId", cartId);
                //        cmd.Parameters.AddWithValue("@ProductId", productId);

                //        cmd.ExecuteNonQuery();
                //    }
                //}
            }
        }

        private int CreateCart()
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
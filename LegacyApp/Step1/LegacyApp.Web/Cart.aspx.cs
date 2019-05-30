using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace LegacyApp.Web
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCart();
            }
        }

        protected void CartRepeater_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["CartId"] != null)
            {
                var cartItemId = int.Parse(e.CommandArgument.ToString());
                RemoveItem(cartItemId);
            }

            LoadCart();
        }

        protected void MenuButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void CheckoutButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Checkout.aspx");
        }

        private void RemoveItem(int cartItemId)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                con.Open();

                var sql = "DELETE FROM dbo.CartItem WHERE CartId = @CartId AND CartItemId = @CartItemId";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@CartId", Session["CartId"]);
                    cmd.Parameters.AddWithValue("@CartItemId", cartItemId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadCart()
        {
            if (Session["CartId"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                var sql = "SELECT i.CartItemId, p.ProductId, p.Name, p.Price, i.Quantity " +
                          " FROM dbo.CartItem i" +
                          " INNER JOIN dbo.Product p ON p.ProductId = i.ProductId" +
                          " WHERE i.CartId = @CartId" +
                          " ORDER BY p.Name";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@CartId", Session["CartId"]);
                    con.Open();

                    using (var rdr = cmd.ExecuteReader())
                    {
                        CartRepeater.DataSource = rdr;
                        CartRepeater.DataBind();
                    }

                    cmd.CommandText = "SELECT SUM(p.Price * i.Quantity)" +
                                      " FROM dbo.CartItem i " +
                                      " INNER JOIN dbo.Product p ON p.ProductId = i.ProductId" +
                                      " WHERE CartId = @CartId";
                    var total = cmd.ExecuteScalar();
                    CartTotalLabel.Text = total == DBNull.Value ? "" : ((decimal) total).ToString("C");
                }
            }
        }
    }
}
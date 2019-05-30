using System;
using System.Configuration;
using System.Data.SqlClient;

namespace LegacyApp.Web
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCart();
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
                var sql = "SELECT p.ProductId, p.Name, p.Price, i.Quantity " +
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
                    CartTotalLabel.Text = ((decimal)cmd.ExecuteScalar()).ToString("C");
                }
            }
        }

        protected void CheckoutButton_OnClick(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                var sql =
                    "EXEC dbo.Checkout @CartId, @FirstName, @LastNAme, @AddressLine1, @AddressLine2, @City, @State, @Zip, @CreditCardNumber, @CreditCardExpirationDate";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@CartId", Session["CartId"]);
                    cmd.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@AddressLine1", AddressLine1TextBox.Text);
                    cmd.Parameters.AddWithValue("@AddressLine2", AddressLine2TextBox.Text);
                    cmd.Parameters.AddWithValue("@City", CityTextBox.Text);
                    cmd.Parameters.AddWithValue("@State", StateTextBox.Text);
                    cmd.Parameters.AddWithValue("@Zip", ZipTextBox.Text);
                    cmd.Parameters.AddWithValue("@CreditCardNumber", CreditCardNumberTextBox.Text);
                    cmd.Parameters.AddWithValue("@CreditCardExpirationDate", CreditCardExpirationDate.Text);
                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }

            Response.Redirect("OrderComplete.aspx");
        }
    }
}
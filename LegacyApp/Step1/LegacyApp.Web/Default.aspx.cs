using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using LegacyApp.Web.Application.Cart;

namespace LegacyApp.Web
{
    public partial class Default : System.Web.UI.Page
    {
        private ICartService _cartService;

        public Default() : this(new CartService())
        {
        }

        public Default(ICartService cartService)
        {
            _cartService = cartService;
        }

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
                cartId = _cartService.CreateCart();
                Session["CartId"] = cartId;
            }
            else
            {
                cartId = Convert.ToInt32(Session["CartId"]);
            }
            _cartService.AddItemToCart(cartId, productId);
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
    }
}
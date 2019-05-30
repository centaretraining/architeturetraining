using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LegacyApp.Desktop
{
    public partial class OrderForm : Form
    {
        private readonly int _orderId;

        public OrderForm(int orderId)
        {
            _orderId = orderId;
            InitializeComponent();
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyApp.Desktop.Properties.Settings.LegacyAppConnectionString"].ConnectionString))
            {
                var sql = "SELECT OrderId, OrderDate, OrderStatus FROM dbo.[Order] WHERE OrderId = @OrderId";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", _orderId);
                    con.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            OrderIdValueLabel.Text = rdr.GetInt32(rdr.GetOrdinal("OrderId")).ToString();
                            OrderDateValueLabel.Text = rdr.GetDateTime(rdr.GetOrdinal("OrderDate")).ToShortTimeString();
                            OrderStatusComboBox.SelectedText = rdr.GetString(rdr.GetOrdinal("OrderStatus"));
                        }
                    }
                }

                sql = "SELECT i.OrderItemId, p.Name, i.Price, i.Quantity " +
                      " FROM dbo.OrderItem i" +
                      " INNER JOIN dbo.Product p ON p.ProductId = i.ProductId" +
                      " WHERE i.OrderId = @OrderId";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", _orderId);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        dataGridView1.DataSource = rdr;
                    }
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["LegacyApp.Desktop.Properties.Settings.LegacyAppConnectionString"].ConnectionString))
            {
                var sql = "UPDATE dbo.[Order] SET OrderSTatus = @OrderStatus WHERE OrderId = @OrderId";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", _orderId);
                    cmd.Parameters.AddWithValue("@OrderStatus", OrderStatusComboBox.SelectedText);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            Close();
        }
    }
}

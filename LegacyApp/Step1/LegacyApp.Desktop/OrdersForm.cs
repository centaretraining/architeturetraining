using System;
using System.Data;
using System.Windows.Forms;

namespace LegacyApp.Desktop
{
    public partial class OrdersForm : Form
    {
        public OrdersForm()
        {
            InitializeComponent();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var order = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row as LegacyAppDataSet.OrderRow;
            var form = new OrderForm(order.OrderId);
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            this.orderTableAdapter.Fill(this.legacyAppDataSet.Order);
        }
    }
}

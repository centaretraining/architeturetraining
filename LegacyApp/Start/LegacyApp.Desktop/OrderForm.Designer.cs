namespace LegacyApp.Desktop
{
    partial class OrderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderForm));
            this.OrderIdLabel = new System.Windows.Forms.Label();
            this.OrderIdValueLabel = new System.Windows.Forms.Label();
            this.OrderDateLabel = new System.Windows.Forms.Label();
            this.OrderDateValueLabel = new System.Windows.Forms.Label();
            this.OrderStatusLabel = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.OrderStatusComboBox = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // OrderIdLabel
            // 
            this.OrderIdLabel.AutoSize = true;
            this.OrderIdLabel.Location = new System.Drawing.Point(12, 74);
            this.OrderIdLabel.Name = "OrderIdLabel";
            this.OrderIdLabel.Size = new System.Drawing.Size(129, 32);
            this.OrderIdLabel.TabIndex = 0;
            this.OrderIdLabel.Text = "Order ID:";
            // 
            // OrderIdValueLabel
            // 
            this.OrderIdValueLabel.AutoSize = true;
            this.OrderIdValueLabel.Location = new System.Drawing.Point(199, 74);
            this.OrderIdValueLabel.Name = "OrderIdValueLabel";
            this.OrderIdValueLabel.Size = new System.Drawing.Size(128, 32);
            this.OrderIdValueLabel.TabIndex = 1;
            this.OrderIdValueLabel.Text = "{OrderId}";
            // 
            // OrderDateLabel
            // 
            this.OrderDateLabel.AutoSize = true;
            this.OrderDateLabel.Location = new System.Drawing.Point(12, 138);
            this.OrderDateLabel.Name = "OrderDateLabel";
            this.OrderDateLabel.Size = new System.Drawing.Size(162, 32);
            this.OrderDateLabel.TabIndex = 2;
            this.OrderDateLabel.Text = "Order Date:";
            // 
            // OrderDateValueLabel
            // 
            this.OrderDateValueLabel.AutoSize = true;
            this.OrderDateValueLabel.Location = new System.Drawing.Point(199, 138);
            this.OrderDateValueLabel.Name = "OrderDateValueLabel";
            this.OrderDateValueLabel.Size = new System.Drawing.Size(165, 32);
            this.OrderDateValueLabel.TabIndex = 3;
            this.OrderDateValueLabel.Text = "{OrderDate}";
            // 
            // OrderStatusLabel
            // 
            this.OrderStatusLabel.AutoSize = true;
            this.OrderStatusLabel.Location = new System.Drawing.Point(12, 202);
            this.OrderStatusLabel.Name = "OrderStatusLabel";
            this.OrderStatusLabel.Size = new System.Drawing.Size(104, 32);
            this.OrderStatusLabel.TabIndex = 4;
            this.OrderStatusLabel.Text = "Status:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1173, 48);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 45);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // OrderStatusComboBox
            // 
            this.OrderStatusComboBox.FormattingEnabled = true;
            this.OrderStatusComboBox.Items.AddRange(new object[] {
            "Pending",
            "In Progress",
            "Out For Delivery",
            "Delivered"});
            this.OrderStatusComboBox.Location = new System.Drawing.Point(205, 199);
            this.OrderStatusComboBox.Name = "OrderStatusComboBox";
            this.OrderStatusComboBox.Size = new System.Drawing.Size(267, 39);
            this.OrderStatusComboBox.TabIndex = 6;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 281);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 40;
            this.dataGridView1.Size = new System.Drawing.Size(1134, 488);
            this.dataGridView1.TabIndex = 7;
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 844);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.OrderStatusComboBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.OrderStatusLabel);
            this.Controls.Add(this.OrderDateValueLabel);
            this.Controls.Add(this.OrderDateLabel);
            this.Controls.Add(this.OrderIdValueLabel);
            this.Controls.Add(this.OrderIdLabel);
            this.Name = "OrderForm";
            this.Text = "OrderForm";
            this.Load += new System.EventHandler(this.OrderForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OrderIdLabel;
        private System.Windows.Forms.Label OrderIdValueLabel;
        private System.Windows.Forms.Label OrderDateLabel;
        private System.Windows.Forms.Label OrderDateValueLabel;
        private System.Windows.Forms.Label OrderStatusLabel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ComboBox OrderStatusComboBox;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
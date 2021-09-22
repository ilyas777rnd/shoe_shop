namespace ShoesShop
{
    partial class ManagerForm
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
            this.tbMakeSale = new System.Windows.Forms.Button();
            this.ShowOrders = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.IsDone = new System.Windows.Forms.Button();
            this.dgvBasket = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBasket)).BeginInit();
            this.SuspendLayout();
            // 
            // tbMakeSale
            // 
            this.tbMakeSale.BackColor = System.Drawing.Color.CornflowerBlue;
            this.tbMakeSale.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMakeSale.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbMakeSale.Location = new System.Drawing.Point(12, 12);
            this.tbMakeSale.Name = "tbMakeSale";
            this.tbMakeSale.Size = new System.Drawing.Size(275, 60);
            this.tbMakeSale.TabIndex = 0;
            this.tbMakeSale.Text = "Оформить продажу";
            this.tbMakeSale.UseVisualStyleBackColor = false;
            this.tbMakeSale.Click += new System.EventHandler(this.btMakeSale_Click);
            // 
            // ShowOrders
            // 
            this.ShowOrders.BackColor = System.Drawing.Color.LightGreen;
            this.ShowOrders.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ShowOrders.Location = new System.Drawing.Point(293, 13);
            this.ShowOrders.Name = "ShowOrders";
            this.ShowOrders.Size = new System.Drawing.Size(275, 59);
            this.ShowOrders.TabIndex = 2;
            this.ShowOrders.Text = "Показать продажи";
            this.ShowOrders.UseVisualStyleBackColor = false;
            this.ShowOrders.Click += new System.EventHandler(this.ShowOrders_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 107);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(677, 478);
            this.dataGridView1.TabIndex = 3;
            // 
            // IsDone
            // 
            this.IsDone.BackColor = System.Drawing.Color.SkyBlue;
            this.IsDone.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IsDone.Location = new System.Drawing.Point(590, 12);
            this.IsDone.Name = "IsDone";
            this.IsDone.Size = new System.Drawing.Size(428, 59);
            this.IsDone.TabIndex = 5;
            this.IsDone.Text = "Заказ выдан";
            this.IsDone.UseVisualStyleBackColor = false;
            this.IsDone.Click += new System.EventHandler(this.IsDone_Click);
            // 
            // dgvBasket
            // 
            this.dgvBasket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBasket.Location = new System.Drawing.Point(695, 107);
            this.dgvBasket.Name = "dgvBasket";
            this.dgvBasket.RowHeadersWidth = 51;
            this.dgvBasket.RowTemplate.Height = 24;
            this.dgvBasket.Size = new System.Drawing.Size(571, 478);
            this.dgvBasket.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(22, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "Заказы(продажи):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(711, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Состав заказа:";
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(156, 591);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1110, 64);
            this.listBox1.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(22, 608);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Покупатель:";
            // 
            // ManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 667);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvBasket);
            this.Controls.Add(this.IsDone);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ShowOrders);
            this.Controls.Add(this.tbMakeSale);
            this.Name = "ManagerForm";
            this.Text = "ManagerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBasket)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button tbMakeSale;
        private System.Windows.Forms.Button ShowOrders;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button IsDone;
        private System.Windows.Forms.DataGridView dgvBasket;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label3;
    }
}
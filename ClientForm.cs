using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Data.Common;

namespace ShoesShop
{
    public partial class ClientForm : Form
    {
        private void updateBasket(string ord_id)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                dgvBasket.Rows.Clear(); dgvBasket.Columns.Clear(); int fields = 0;
                dgvBasket.ColumnCount = fields = 5;
                dgvBasket.Columns[0].HeaderText = "ID заказа"; dgvBasket.Columns[0].Width = 50;
                dgvBasket.Columns[1].HeaderText = "ID товара"; dgvBasket.Columns[1].Width = 50;
                dgvBasket.Columns[2].HeaderText = "Наименование товара"; dgvBasket.Columns[2].Width = 120;
                dgvBasket.Columns[3].HeaderText = "Цена"; dgvBasket.Columns[3].Width = 50;
                dgvBasket.Columns[4].HeaderText = "Размер"; dgvBasket.Columns[4].Width = 50;
                string query = "Select item_cart, item_warehouse, name, price, size from " +
                " (item JOIN warehouse ON item_warehouse = warehouse.id) " +
                $" JOIN shoes ON warehouse_shoes = shoes.id WHERE item_cart={ord_id}";
                int ind = 0;
                NpgsqlCommand find = new NpgsqlCommand(query, con);
                NpgsqlDataReader npgSqlDataReader = find.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    dgvBasket.RowCount++;
                    for (int i = 0; i < fields; i++)
                        dgvBasket[i, ind].Value = dbDataRecord[i].ToString();
                    ind++;
                }
            }
        }
        private void output(object sender, EventArgs e)
        {
            string id = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
            updateBasket(id);
        }

        private void show_my_orders()
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string query = $"Select id,price from cart WHERE customer ={Login.purch_id} AND price>0 ORDER by id DESC";
                dataGridView1.Rows.Clear(); dataGridView1.Columns.Clear(); int fields = 2;
                dataGridView1.ColumnCount = fields;
                dataGridView1.Columns[0].HeaderText = "ID заказа"; dataGridView1.Columns[0].Width = 50;
                dataGridView1.Columns[1].HeaderText = "Стоимость"; dataGridView1.Columns[1].Width = 60;
                NpgsqlCommand find = new NpgsqlCommand(query, con); int ind = 0;
                NpgsqlDataReader npgSqlDataReader = find.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    dataGridView1.RowCount++;
                    for (int i = 0; i < fields; i++)
                        dataGridView1[i, ind].Value = dbDataRecord[i].ToString();
                    ind++;
                }
            }
        }
        public ClientForm()
        {
            InitializeComponent();
            dataGridView1.CellClick += output;
        }

        private void MakeOrder_Click(object sender, EventArgs e)
        {
            MakeOrderForm order = new MakeOrderForm();
            order.Show();
        }

        private void myOrders_Click(object sender, EventArgs e)
        {
            show_my_orders();
        }
    }
}

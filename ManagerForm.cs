using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using Npgsql;

namespace ShoesShop
{
    public partial class ManagerForm : Form
    {
        public ManagerForm()
        {
            InitializeComponent();
            dataGridView1.CellClick += output;
        }

        private void output(object sender, EventArgs e)
        {
            string id = dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value.ToString();
            updateBasket(id);
        }

        private void btMakeSale_Click(object sender, EventArgs e)
        {
            MakeOrderForm makeOrderForm = new MakeOrderForm();
            makeOrderForm.Show();
        }

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
                Login.connectionReset(con);
                listBox1.Items.Clear();
                string customer_id = dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                if (!customer_id.Equals(""))
                {
                    string customer_query = $"Select name,surname,login from account WHERE role=0 AND id={customer_id}";
                    NpgsqlCommand cmd = new NpgsqlCommand(customer_query, con);
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    string info = ""; 
        
                    foreach (DbDataRecord dbDataRecord in reader)
                    {
                        for (int i = 0; i < 3; i++)
                            info += dbDataRecord[i].ToString() + " ";
                    }
                    listBox1.Items.Add(info);
                }
            }
        }


        private void show_my_orders()
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string query = $"Select sale.id, seller, sale_cart, day, price " +
                "from sale JOIN cart ON sale_cart = cart.id WHERE price > 0 " +
                " ORDER BY id DESC ";
                dataGridView1.Rows.Clear(); dataGridView1.Columns.Clear(); int fields = 5;
                dataGridView1.ColumnCount = fields;
                dataGridView1.Columns[0].HeaderText = "ID продажи"; dataGridView1.Columns[0].Width = 50;
                dataGridView1.Columns[1].HeaderText = "ID продавца"; dataGridView1.Columns[1].Width = 60;
                dataGridView1.Columns[2].HeaderText = "ID заказа"; dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].HeaderText = "Дата"; dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[4].HeaderText = "Цена заказа"; dataGridView1.Columns[4].Width = 100;
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

        private void ShowOrders_Click(object sender, EventArgs e)
        {
            show_my_orders();
        }

        private void IsDone_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("Выберите строку в таблице заказов (таблица слева)!");
                return;
            }
            string ord_id = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string query = $"UPDATE sale SET seller={Login.stuff_id} WHERE id={ord_id}";
                new NpgsqlCommand(query, con).ExecuteNonQuery();
            }
            show_my_orders();
        }
    }
}

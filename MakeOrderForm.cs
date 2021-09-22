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
    public partial class MakeOrderForm : Form
    {
        public int cart_id { get; private set; }

        private bool is_done;
        public MakeOrderForm()
        {
            InitializeComponent();
            using (NpgsqlConnection con = Login.getConnection())
            {
                is_done = false;
                con.Open(); cart_id = 0;
                var cmd = new NpgsqlCommand("SELECT MAX(id) from Cart", con);
                var reader = cmd.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader)
                {
                    var record = dbDataRecord[0].ToString();
                    if (record.Equals(""))
                    {
                        cart_id = 1;
                    }
                    else
                    {
                        cart_id = int.Parse(record) + 1;
                    }
                }
                Login.connectionReset(con);
                NpgsqlCommand cmd_add;
                label1.Text = $"Заказ номер {cart_id}";
                if (Login.purch_id != null)
                {
                    label3.Text = $"ID покупателя:{Login.purch_id}";
                    cmd_add = new NpgsqlCommand($"INSERT into Cart(id, customer, price) VALUES({cart_id}, {Login.purch_id}, 0)", con);
                    cmd_add.ExecuteNonQuery();
                }
                else if (Login.stuff_id != null)
                {
                    label3.Text = $"ID сотрудника:{Login.stuff_id}";
                    cmd_add = new NpgsqlCommand($"INSERT into Cart(id, price) VALUES({cart_id}, 0)", con);
                    cmd_add.ExecuteNonQuery();
                }
            }
            tbTotal.Text = "0";
            this.FormClosed += Done;
        }

        private void btAddItem_Click(object sender, EventArgs e)
        {
            ChooseForm chooseForm = new ChooseForm(this);
            chooseForm.Show();
        }

        public void recalc_cart()
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string query = "Select item_cart, item_warehouse, name, price, size from " +
                " (item JOIN warehouse ON item_warehouse = warehouse.id) " +
                $" JOIN shoes ON warehouse_shoes = shoes.id WHERE item_cart={cart_id}";
                int fields;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.ColumnCount = fields = 5;
                dataGridView1.Columns[0].HeaderText = "ID корзины"; dataGridView1.Columns[0].Width = 30;
                dataGridView1.Columns[1].HeaderText = "ID товара"; dataGridView1.Columns[1].Width = 30;
                dataGridView1.Columns[2].HeaderText = "Наименовение"; dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[3].HeaderText = "Цена"; dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[4].HeaderText = "Размер"; dataGridView1.Columns[4].Width = 120;
                NpgsqlCommand cmd1 = new NpgsqlCommand(query, con);
                int ind = 0;
                NpgsqlDataReader reader1 = cmd1.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader1)
                {
                    dataGridView1.RowCount++;
                    for (int i = 0; i < fields; i++)
                        dataGridView1[i, ind].Value = dbDataRecord[i].ToString();
                    ind++;
                }
                //Далее сумма корзины
                Login.connectionReset(con);
                string query_price = "Select SUM(price) from " +
                " (item JOIN warehouse ON item_warehouse = warehouse.id) " +
                $" JOIN shoes ON warehouse_shoes = shoes.id WHERE item_cart={cart_id}";
                NpgsqlCommand cmd_price = new NpgsqlCommand(query_price, con);
                var reader = cmd_price.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader)
                {
                    if (dbDataRecord[0].ToString().Equals(""))
                    {
                        tbTotal.Text = "0";
                    }
                    else
                    {
                        tbTotal.Text = dbDataRecord[0].ToString();
                    }
                }
                Login.connectionReset(con);
                //Обновляем стоимость
                string upd_query = $"UPDATE cart SET price ={tbTotal.Text} WHERE id={cart_id} ";
                new NpgsqlCommand(upd_query, con).ExecuteNonQuery();
            }
        }

        private void Done(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string query = "";
                if (!is_done)
                {
                    query = $"Delete from cart where id={cart_id}";
                }
                else
                {
                    if (Login.purch_id != null)
                    {
                        query = $"Insert into sale(sale_cart, day) VALUES ({cart_id},now())";
                    }
                    else if (Login.stuff_id != null)
                    {
                        query = $"Insert into sale(seller, sale_cart, day) VALUES ({Login.stuff_id},{cart_id},now())";
                    }
                }
                new NpgsqlCommand(query, con).ExecuteNonQuery();
            }
        }

        private void btDone_Click(object sender, EventArgs e)
        {
            this.is_done = true;
            if (!tbTotal.Text.Equals(""))
            {
                if (Login.purch_id != null)
                {
                    MessageBox.Show($"Спасибо за заказ, номер заказа {cart_id}.");
                }
                else if (Login.stuff_id != null)
                {
                    MessageBox.Show($"Покупка завершена, номер корзины {cart_id}.");
                }
            }
            this.Close();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                if(dataGridView1.CurrentCell==null)
                {
                    MessageBox.Show("Выберите строку с товаром!");
                    return;
                }
                string prod_id = dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                string query = $"Delete from item WHERE item_cart={cart_id} AND item_warehouse={prod_id}";
                new NpgsqlCommand(query, con).ExecuteNonQuery();
                recalc_cart();
            } 
        }
    }
}

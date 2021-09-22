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
    public partial class Backup : Form
    {
        public Backup()
        {
            InitializeComponent();
            rbOrder.Checked = false;
            rbOrdList.Checked = false;
            show();
        }

        void update_cost(int ord_id, int prod_id, string direction) //Обновляет стоимость заказа при изменении корзины
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                try
                {
                    string query = "UPDATE cart SET " +
                    $" price = price {direction} find_price({prod_id}) " +
                    $" WHERE id = {ord_id}";
                    new NpgsqlCommand(query, con).ExecuteNonQuery();
                }
                catch(Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        void show()
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string query1 = "Select * from cart_audit ORDER by datetime DESC";
                string query2 = "Select * from item_audit ORDER by datetime DESC";
                int fields;
                //1-я таблица - Корзина
                dataGridView1.Rows.Clear(); dataGridView2.Rows.Clear();
                dataGridView1.Columns.Clear(); dataGridView2.Columns.Clear();
                dataGridView1.ColumnCount = fields = 5; int ind = 0;
                dataGridView1.Columns[0].HeaderText = "Время"; dataGridView1.Columns[0].Width = 100;
                dataGridView1.Columns[1].HeaderText = "Действие"; dataGridView1.Columns[1].Width = 60;
                dataGridView1.Columns[2].HeaderText = "ID корзины"; dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].HeaderText = "ID покупателя"; dataGridView1.Columns[3].Width = 60;
                dataGridView1.Columns[4].HeaderText = "Стоимость"; dataGridView1.Columns[4].Width = 100;
                NpgsqlCommand cmd1 = new NpgsqlCommand(query1, con);
                NpgsqlDataReader reader1 = cmd1.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader1)
                {
                    dataGridView1.RowCount++;
                    for (int col = 0; col < fields; col++)
                    {
                        dataGridView1[col, ind].Value = dbDataRecord[col].ToString();
                    }
                    ind++;
                }
                con.Close(); con.Open();
                //2-я таблица - Состав заказа
                dataGridView2.ColumnCount = fields = 5;
                dataGridView2.Columns[0].HeaderText = "Время"; dataGridView2.Columns[0].Width = 60;
                dataGridView2.Columns[1].HeaderText = "Действие"; dataGridView2.Columns[1].Width = 60;
                dataGridView2.Columns[2].HeaderText = "ID"; dataGridView2.Columns[2].Width = 60;
                dataGridView2.Columns[3].HeaderText = "ID корзины"; dataGridView2.Columns[3].Width = 60;
                dataGridView2.Columns[4].HeaderText = "ID товара"; dataGridView2.Columns[4].Width = 60;
                NpgsqlCommand cmd2 = new NpgsqlCommand(query2, con); ind = 0;
                NpgsqlDataReader reader2 = cmd2.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader2)
                {
                    dataGridView2.RowCount++;
                    for (int col = 0; col < fields; col++)
                    {
                        dataGridView2[col, ind].Value = dbDataRecord[col].ToString();
                    }
                    ind++;
                }
            }
        }

        void backup(int row_ind)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string queryH = "", main_query = "";
                try
                {
                    if (rbOrder.Checked)
                    {
                        //queryH = $"Delete from OrderH WHERE idorder={dataGridView1[0, row_ind].Value.ToString()}";
                        if (dataGridView1[1, row_ind].Value.ToString().Equals("I"))
                        {
                            main_query = $"Delete from cart WHERE id={dataGridView1[2, row_ind].Value.ToString()}";
                            //new NpgsqlCommand(queryH, con).ExecuteNonQuery();
                            Console.WriteLine(main_query);
                            new NpgsqlCommand(main_query, con).ExecuteNonQuery();
                        }
                        else if (dataGridView1[1, row_ind].Value.ToString().Equals("U"))
                        {
                            string upd_query = "UPDATE cart SET " +
                            $" price=" + (dataGridView1[4, row_ind].Value.ToString().Equals("") ? "NULL" : dataGridView1[3, row_ind].Value.ToString()) + " " +
                            $" customer=" + (dataGridView1[3, row_ind].Value.ToString().Equals("") ? "NULL" : dataGridView1[3, row_ind].Value.ToString()) + " " +
                            $" WHERE id={dataGridView1[2, row_ind].Value.ToString()}";
                            // new NpgsqlCommand(queryH, con).ExecuteNonQuery();
                            Console.WriteLine(upd_query);
                            new NpgsqlCommand(upd_query, con).ExecuteNonQuery();
                        }
                        else if (dataGridView1[1, row_ind].Value.ToString().Equals("D"))
                        {
                            string query_add = "INSERT INTO cart (id, customer, price) " +
                            $" VALUES({dataGridView1[2, row_ind].Value.ToString()} " +
                            $", " + (dataGridView1[3, row_ind].Value.ToString().Equals("") ? "NULL" : dataGridView1[3, row_ind].Value.ToString()) + " " +
                            $", " + (dataGridView1[4, row_ind].Value.ToString().Equals("") ? "NULL" : dataGridView1[4, row_ind].Value.ToString()) + " )";
                            //new NpgsqlCommand(queryH, con).ExecuteNonQuery();
                            Console.WriteLine(query_add);
                            new NpgsqlCommand(query_add, con).ExecuteNonQuery();
                        }
                    }
                    else if (rbOrdList.Checked)
                    {                       
                        if (dataGridView2[1, row_ind].Value.ToString().Equals("I"))
                        {
                            main_query = $"Delete from item WHERE id={dataGridView2[2, row_ind].Value.ToString()}";
                            new NpgsqlCommand(main_query, con).ExecuteNonQuery();
                            update_cost(int.Parse(dataGridView2[3, row_ind].Value.ToString()), int.Parse(dataGridView2[4, row_ind].Value.ToString()), "-");
                        }
                        else if (dataGridView2[1, row_ind].Value.ToString().Equals("D"))
                        {
                            string ins_query = "INSERT INTO item (id, item_cart, item_warehouse) VALUES (" +
                                $" {dataGridView2[2, row_ind].Value.ToString()} " +
                                $" ,{dataGridView2[3, row_ind].Value.ToString()} " +
                                $" ,{dataGridView2[4, row_ind].Value.ToString()} )";
                            //new NpgsqlCommand(queryH, con).ExecuteNonQuery();
                            new NpgsqlCommand(ins_query, con).ExecuteNonQuery();
                            update_cost(int.Parse(dataGridView2[3, row_ind].Value.ToString()), int.Parse(dataGridView2[4, row_ind].Value.ToString()), "+");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Таблица не выбрана");
                        return;
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void rbOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOrdList.Checked == true)
                rbOrdList.Checked = false;   
        }

        private void rbOrdList_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOrder.Checked == true)
                rbOrder.Checked = false;
        }

        private void btBackup_Click(object sender, EventArgs e)
        {
            try
            {
                int row_index = 0;
                if (rbOrder.Checked == true) row_index = dataGridView1.CurrentCell.RowIndex;
                else if (rbOrdList.Checked == true) row_index = dataGridView2.CurrentCell.RowIndex;
                else throw new Exception("Выберите таблицу!");
                backup(row_index);
                show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}

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
    public partial class Supply : Form
    {
        private void showModels()
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string query1 = "Select shoes.id, shoes.name, shoestyle.name, price, description " +
                " from shoestyle JOIN shoes ON shoes.shoes_style = shoestyle.id ";
                int fields;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.ColumnCount = fields = 5;
                dataGridView1.Columns[0].HeaderText = "ID"; dataGridView1.Columns[0].Width = 30;
                dataGridView1.Columns[1].HeaderText = "Наименовение"; dataGridView1.Columns[1].Width = 120;
                dataGridView1.Columns[2].HeaderText = "Категория"; dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[3].HeaderText = "Цена руб."; dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[4].HeaderText = "Описание"; dataGridView1.Columns[4].Width = 120;
                NpgsqlCommand cmd1 = new NpgsqlCommand(query1, con);
                int ind = 0;
                NpgsqlDataReader reader1 = cmd1.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader1)
                {
                    dataGridView1.RowCount++;
                    for (int i = 0; i < fields; i++)
                        dataGridView1[i, ind].Value = dbDataRecord[i].ToString();
                    ind++;
                }
            }
        }

        private void ChangeShoesQty(string qty, char direction)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string id = dataGridView3[0, dataGridView3.CurrentCell.RowIndex].Value.ToString();
                string size = dataGridView3[2, dataGridView3.CurrentCell.RowIndex].Value.ToString();
                string query = $"UPDATE warehouse SET quantity = quantity {direction} {qty} " +
                    $" WHERE warehouse_shoes = {id} AND size = {size}";
                new NpgsqlCommand(query, con).ExecuteNonQuery();
            }
        }

        private void ModelInfo(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open(); int fields;
                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();
                dataGridView3.ColumnCount = 0;
                if (dataGridView1.CurrentCell.Value == null) return;
                string id = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                string query = "Select warehouse_shoes, name, size, quantity " +
                $" from warehouse left join shoes on warehouse_shoes = shoes.id WHERE warehouse_shoes={id}";
                dataGridView3.ColumnCount = fields = 4;
                dataGridView3.Columns[0].HeaderText = "ID"; dataGridView3.Columns[0].Width = 30;
                dataGridView3.Columns[1].HeaderText = "Наименовение"; dataGridView3.Columns[1].Width = 100;
                dataGridView3.Columns[2].HeaderText = "Размер"; dataGridView3.Columns[2].Width = 60;
                dataGridView3.Columns[3].HeaderText = "Кол-во шт."; dataGridView3.Columns[3].Width = 60;
                NpgsqlCommand cmd1 = new NpgsqlCommand(query, con);
                int ind = 0;
                NpgsqlDataReader reader1 = cmd1.ExecuteReader();
                if (!reader1.HasRows)
                {
                    MessageBox.Show("Эта модель не добавлена на склад. Добавьте эту модель с нужными размерами.");
                    return;
                }
                foreach (DbDataRecord dbDataRecord in reader1)
                {
                    dataGridView3.RowCount++;
                    for (int i = 0; i < fields; i++)
                        dataGridView3[i, ind].Value = dbDataRecord[i].ToString();
                    ind++;
                }
            }
        }

        public Supply()
        {
            InitializeComponent();
            showModels();
            dataGridView1.CellClick += ModelInfo;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open(); 
                try
                {
                    string id = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                    string query = $"INSERT into Warehouse (warehouse_shoes, size, quantity)" +
                    $"VALUES({id}, {tbSize.Text}, 0)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    ModelInfo(null, null);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void Inc_Click(object sender, EventArgs e)
        {
            ChangeShoesQty(tbAmounce.Text, '+');
            ModelInfo(null, null);
        }

        private void Dec_Click(object sender, EventArgs e)
        {
            ChangeShoesQty(tbAmounce.Text, '-');
            ModelInfo(null, null);
        }
    }
}

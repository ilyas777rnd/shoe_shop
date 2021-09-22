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
    public partial class ChooseForm : Form
    {
        public int cart_id { get; set; }
        public MakeOrderForm orderForm { get; private set; }
        public ChooseForm(MakeOrderForm makeOrderForm)
        {
            InitializeComponent();
            showModels(getDefaultQuery());
            dataGridView1.CellClick += ModelInfo;
            fillCategories(cbCategory);
            if (Login.purch_id == null && Login.stuff_id == null)
            {
                btAddToCart.Visible = false;
            }
            if (makeOrderForm != null)
            {
                this.cart_id = makeOrderForm.cart_id;
                this.orderForm = makeOrderForm;
            }
        }

        private string getDefaultQuery() => "Select shoes.id, shoes.name, category.name, shoestyle.name, price, description " +
        " from (shoestyle JOIN shoes ON shoes.shoes_style = shoestyle.id " +
        " JOIN category ON style_category=category.id) WHERE price>0 ";

        private void showModels(string query)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                int fields;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                
                dataGridView1.ColumnCount = fields = 6;
                dataGridView1.Columns[0].HeaderText = "ID"; dataGridView1.Columns[0].Width = 30;
                dataGridView1.Columns[1].HeaderText = "Наименовение"; dataGridView1.Columns[1].Width = 120;
                dataGridView1.Columns[2].HeaderText = "Категория"; dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[3].HeaderText = "Подкатегория"; dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[4].HeaderText = "Цена руб."; dataGridView1.Columns[4].Width = 120;
                dataGridView1.Columns[5].HeaderText = "Описание"; dataGridView1.Columns[5].Width = 1;
                NpgsqlCommand cmd1 = new NpgsqlCommand(query, con);
                int ind = 0;
                NpgsqlDataReader reader1 = cmd1.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader1)
                {
                    dataGridView1.RowCount++;
                    for (int i = 0; i < fields; i++)
                    {
                        dataGridView1[i, ind].Value = dbDataRecord[i].ToString();
                    }
                    ind++;
                }
            }
        }

        private void ModelInfo(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open(); int fields;
                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();
                richTextBox1.Text = dataGridView1[5, dataGridView1.CurrentCell.RowIndex].Value.ToString(); ;
                dataGridView3.ColumnCount = 0;
                if (dataGridView1.CurrentCell.Value == null) return;
                string id = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                string query = "Select warehouse.id, size, quantity " +
                $" from warehouse left join shoes on warehouse_shoes = shoes.id WHERE warehouse_shoes={id} ";
                if (!tbSizeFrom.Text.Equals(""))
                {
                    query += $" AND size>={tbSizeFrom.Text} ";
                }
                if (!tbSizeTo.Text.Equals(""))
                {
                    query += $" AND size<={tbSizeTo.Text} ";
                }
                dataGridView3.ColumnCount = fields = 3;
                dataGridView3.Columns[0].HeaderText = "ID"; dataGridView3.Columns[0].Width = 1;
                dataGridView3.Columns[1].HeaderText = "Размер"; dataGridView3.Columns[1].Width = 60;
                dataGridView3.Columns[2].HeaderText = "Кол-во шт."; dataGridView3.Columns[2].Width = 60;
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

        private void fillCategories(ComboBox cb)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open(); cb.Items.Clear();
                string query = "Select name from category ORDER BY name";
                NpgsqlCommand npgSqlCommand = new NpgsqlCommand(query, con);
                NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    cb.Items.Add(dbDataRecord[0].ToString());
            }
        }

        private void fillShoestyle(ComboBox cb,string category)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open(); cb.Items.Clear();
                string query = "Select shoestyle.name " +
                " FROM shoestyle LEFT JOIN category ON shoestyle.id = category.id " +
                $" WHERE category.name='{category}'";
                NpgsqlCommand npgSqlCommand = new NpgsqlCommand(query, con);
                NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    cb.Items.Add(dbDataRecord[0].ToString());
            }
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                try
                {
                    con.Open();
                    string query = getDefaultQuery();
                    if (!cbCategory.Text.Equals(""))
                    {
                        query += $" AND category.name='{cbCategory.Text}' ";
                    }
                    if (!cbStyle.Text.Equals(""))
                    {
                        query += $" AND shoestyle.name='{cbStyle.Text}'";
                    }
                    if (!tbSizeFrom.Text.Equals(""))
                    {
                        query += $" AND has_size_and_bigger({tbSizeFrom.Text}, shoes.id)>=1 ";
                    }
                    if (!tbSizeTo.Text.Equals(""))
                    {
                        query += $" AND has_size_and_less({tbSizeTo.Text}, shoes.id)>=1 ";
                    }
                    showModels(query);
                }
                catch(Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillShoestyle(cbStyle, cbCategory.Text);
        }

        private void btAddToCart_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                if (dataGridView3.CurrentCell.Value == null) return;
                string product_id = dataGridView3[0, dataGridView3.CurrentCell.RowIndex].Value.ToString();
                if (dataGridView3[2, dataGridView3.CurrentCell.RowIndex].Value.ToString().Equals("0"))
                {
                    MessageBox.Show("Этого размера нет в наличии!");
                    return;
                }
                string query = $"Insert into item(item_cart, item_warehouse) VALUES ({cart_id},{product_id})";
                new NpgsqlCommand(query, con).ExecuteNonQuery();
                orderForm.recalc_cart();
                this.Close();
            }
        }
    }
}

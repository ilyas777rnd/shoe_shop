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
    public partial class CategoriesAndModels : Form
    {
        private Control[] cat_elems;
        private Control[] shoestyle_elems;
        private Control[] shoes_elems;
        public CategoriesAndModels()
        {
            InitializeComponent();
            show();
            cbTable.Items.AddRange(new string[] { "Категории", "Подкатегории", "Модели обуви" });
            cat_elems = new Control[] { label5, tbNameCat };
            shoestyle_elems = new Control[] { label5, tbNameCat, label6 };
            shoes_elems = new Control[] { label7, label8, label9, label10,
            tbModelName, tbPrice, tbDescription};
            this.Closeup();
        }

        private void Closeup()
        {
            foreach(var item in cat_elems.Union(shoestyle_elems).Union(shoes_elems))
            {
                item.Visible = false;
            }
        }

        private void show()
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                string query1 = "Select * from category";
                string query2 = "Select shoestyle.id, shoestyle.name, category.name " +
                    " FROM shoestyle LEFT JOIN category ON style_category = category.id ";
                string query3 = "Select shoes.id, shoes.name, shoestyle.name, price, description " +
                " from shoestyle JOIN shoes ON shoes.shoes_style = shoestyle.id ";
                //1-я таблица - Категории
                int fields;
                dataGridView1.Rows.Clear(); dataGridView2.Rows.Clear(); dataGridView3.Rows.Clear();
                dataGridView1.Columns.Clear(); dataGridView2.Columns.Clear(); dataGridView3.Columns.Clear();
                dataGridView1.ColumnCount = fields = 2;
                dataGridView1.Columns[0].HeaderText = "ID"; dataGridView1.Columns[0].Width = 30;
                dataGridView1.Columns[1].HeaderText = "Наименовение"; dataGridView1.Columns[1].Width = 120;
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
                Login.connectionReset(con);
                //Таблица 2 - Подкатегории
                dataGridView2.ColumnCount = fields = 3;
                dataGridView2.Columns[0].HeaderText = "ID подкатегории"; dataGridView2.Columns[0].Width = 30;
                dataGridView2.Columns[1].HeaderText = "Название"; dataGridView2.Columns[1].Width = 120;
                dataGridView2.Columns[2].HeaderText = "Категория"; dataGridView2.Columns[2].Width = 120;
                NpgsqlCommand cmd2 = new NpgsqlCommand(query2, con);
                ind = 0;
                NpgsqlDataReader reader2 = cmd2.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader2)
                {
                    dataGridView2.RowCount++;
                    for (int i = 0; i < fields; i++)
                    {
                        dataGridView2[i, ind].Value = dbDataRecord[i].ToString();
                    }
                    ind++;
                }
                Login.connectionReset(con);
                //Таблица 3 - Модели
                dataGridView3.ColumnCount = fields = 5;
                dataGridView3.Columns[0].HeaderText = "ID Модели"; dataGridView3.Columns[0].Width = 50;
                dataGridView3.Columns[1].HeaderText = "Название"; dataGridView3.Columns[1].Width = 80;
                dataGridView3.Columns[2].HeaderText = "Подкатегория"; dataGridView3.Columns[2].Width = 90;
                dataGridView3.Columns[3].HeaderText = "Цена руб."; dataGridView3.Columns[3].Width = 70;
                dataGridView3.Columns[4].HeaderText = "Описание"; dataGridView3.Columns[4].Width = 80;
                NpgsqlCommand cmd3 = new NpgsqlCommand(query3, con);
                ind = 0;
                NpgsqlDataReader reader3 = cmd3.ExecuteReader();
                foreach (DbDataRecord dbDataRecord in reader3)
                {
                    dataGridView3.RowCount++;
                    for (int i = 0; i < fields; i++)
                    {
                        dataGridView3[i, ind].Value = dbDataRecord[i].ToString();
                    }
                    ind++;
                }
            }
        }

        private void cbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Closeup();
            switch (cbTable.Text)
            {
                case "Категории":
                    foreach (var item in cat_elems) item.Visible = true;
                    break;
                case "Подкатегории":
                    foreach (var item in shoestyle_elems) item.Visible = true;
                    break;
                case "Модели обуви":
                    foreach (var item in shoes_elems) item.Visible = true;
                    break;
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open(); string query = "";
                try
                {
                    switch (cbTable.Text)
                    {
                        case "Категории":
                            query = $"INSERT INTO category(name) VALUES ('{tbNameCat.Text}')";
                            break;

                        case "Подкатегории":
                            if (dataGridView1.CurrentCell == null)
                            {
                                throw new Exception("Выберите категорию!");
                            }
                            string categ_id = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                            query = $"INSERT into shoestyle(style_category,name) VALUES  "+
                            $"({categ_id}, '{tbNameCat.Text}')";
                            break;

                        case "Модели обуви":
                            if (dataGridView2.CurrentCell == null)
                            {
                                throw new Exception("Выберите категорию!");
                            }
                            string style_id = dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString();
                            query = $"INSERT into shoes(shoes_style,name,price,description) VALUES  " +
                            $"({style_id}, '{tbModelName.Text}', {tbPrice.Text}, '{tbDescription.Text}')";
                            //MessageBox.Show(query);
                            break;
                    }
                    new NpgsqlCommand(query, con).ExecuteNonQuery();
                    show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open(); string query = "",  id="";
                try
                {
                    switch (cbTable.Text)
                    {
                        case "Категории":
                            if (dataGridView1.CurrentCell == null) throw new Exception("Выберите строку для удаления!");
                            id = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                            query = $"Delete from category WHERE id={id}";
                            break;

                        case "Подкатегории":
                            if (dataGridView2.CurrentCell == null)  throw new Exception("Выберите строку для удаления!");
                            id = dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString();
                            query = $"Delete from shoestyle WHERE id={id}";
                            break;

                        case "Модели обуви":
                            if (dataGridView3.CurrentCell == null) throw new Exception("Выберите строку для удаления!");
                            id = dataGridView3[0, dataGridView3.CurrentCell.RowIndex].Value.ToString();
                            query = $"Delete from shoes WHERE id={id}";
                            break;
                    }
                    new NpgsqlCommand(query, con).ExecuteNonQuery();
                    show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

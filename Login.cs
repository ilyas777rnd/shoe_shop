using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ShoesShop
{
    public partial class Login : Form
    {
        public static string stuff_id { get; private set; }
        public static string purch_id { get; private set; }
        private static string connectionString =
        "Server=localhost;Port=7777;User ID=postgres;Password=0000;Database=Shoes;";
        
        public Login()
        {
            InitializeComponent();
            passw.UseSystemPasswordChar = true;
        }

        public static NpgsqlConnection getConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        private void enter_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                stuff_id = ""; con.Open(); string password = "";
                if (chbStuff.Checked)
                {

                    if ((log.Text.Equals("Admin") || log.Text.Equals("admin")) && passw.Text.Equals("7777"))
                    {
                        AdminForm admin = new AdminForm();
                        admin.Show();
                        return;
                    }

                    stuff_id = log.Text;
                    string query = $"Select pass from account WHERE id={stuff_id} AND role=1";
                    NpgsqlCommand npgSqlCommand = new NpgsqlCommand(query, con);
                    NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
                    foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    {
                        password = Cod.DecodeString(dbDataRecord[0].ToString());
                    }
                    if (password.Equals(passw.Text))
                    {
                        purch_id = null;
                        ManagerForm managerForm = new ManagerForm();
                        managerForm.Show();
                    }
                    else MessageBox.Show("Неверный логин или пароль!");
                }
                else
                {
                    string login = log.Text;
                    string cl_query = $"Select id,pass from account WHERE login='{login}' AND role=0";
                    NpgsqlCommand cl_Command = new NpgsqlCommand(cl_query, con);
                    NpgsqlDataReader cl_DataReader = cl_Command.ExecuteReader();
                    foreach (DbDataRecord dbDataRecord in cl_DataReader)
                    {
                        purch_id = dbDataRecord[0].ToString();
                        password = Cod.DecodeString(dbDataRecord[1].ToString());
                    }
                    if (password.Equals(passw.Text))
                    {
                        stuff_id = null;
                        ClientForm client = new ClientForm();
                        client.Show();
                    }
                    else MessageBox.Show("Неверный логин или пароль!");
                    return;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                passw.UseSystemPasswordChar = false;
            else passw.UseSystemPasswordChar = true;
        }

        private void btRegistration_Click(object sender, EventArgs e)
        {
            RegistrationForm registration = new RegistrationForm();
            registration.Show();
        }

        private void btEnterLikeGuest_Click(object sender, EventArgs e)
        {
            stuff_id = purch_id = null;
            ChooseForm chooseForm = new ChooseForm(null);
            chooseForm.Show();
        }

        public static void connectionReset(NpgsqlConnection con)
        {
            con.Close();
            con.Open();
        }
    }
}

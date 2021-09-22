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
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void btDone_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = Login.getConnection())
            {
                con.Open();
                try
                {
                    if (tbPassword.Text.Equals("") || tbName.Text.Equals("") || tbLogin.Equals(""))
                        throw new Exception("Введите Email и пароль!)");
                    string user_query = $"INSERT into account(login, pass, name, surname, patronymic, role) " +
                    $" VALUES ('{tbLogin.Text}','{Cod.EncodeString(tbPassword.Text)}','{tbName.Text}','{tbSurname.Text}','{tbPatr.Text}',0) ";

                    new NpgsqlCommand(user_query, con).ExecuteNonQuery();
                    MessageBox.Show($"Регистрация прошла успешно, логин - {tbLogin.Text}!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

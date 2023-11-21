using System;
using System.Collections.Generic;
using System.ComponentModel;
using SD = System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HeshAutorization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SqlConnection sqlConnection = new SqlConnection(@"Data Source=LAPTOP-EQR48ANI; Initial Catalog=avt; Integrated Security=True");
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

        public void openConnection()
        {
            if (sqlConnection.State == SD.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == SD.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        private void button_Avt(object sender, EventArgs e)
        {
            string Login = textBox1.Text.ToString();
            string Password = textBox2.Text.ToString();

            openConnection();

            if(Proverka(Login, Password) == true)
            {
                MessageBox.Show("авторизация выполнена", ":)");
            }
            else
            {
                MessageBox.Show("введен неверный логин или пароль", ":(");
            }
            closeConnection();
        }

        private void button_Regg(object sender, EventArgs e)
        {
            string Login = textBox3.Text.ToString();
            string Password = textBox4.Text.ToString();
            string Password2 = textBox5.Text.ToString();
            string Name = textBox8.Text.ToString();
            string Family = textBox7.Text.ToString();
            string Dolgnost = textBox6.Text.ToString();

            if (Password == Password2)
            {
                    openConnection();

                if (Proverka(Login, Password) == true)
                {
                    MessageBox.Show("такой аккаун уже есть", ":)");
                }
                else if (Proverka(Login, Password) == false)
                {

                    Password = CreateMD5(Password); 

                    string commandString = $"insert into avt_reg(логин, пароль, имя, фамилия, должность) values('{Login}', '{Password}', '{Name}', '{Family}', '{Dolgnost}')";

                    SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);

                    try
                    {
                        if (sqlCommand.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("акк создан", ":)");
                        }
                        else
                        {
                            MessageBox.Show("акк не создан", ":(");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ":)");
                    }
                }
            }
            else
            {
                MessageBox.Show("вы ввели разные пароли", ":(");
            }

            closeConnection();
        }

        private Boolean Proverka(string log, string pass)
        {
            DataTable table = new DataTable();
            string Login_l = log;
            string Password_p = pass;

            Password_p = CreateMD5(Password_p);

            string commandString = $"select логин, пароль from avt_reg where логин='{Login_l}' and пароль='{Password_p}'";

            SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);

            sqlDataAdapter.SelectCommand = sqlCommand;
            sqlDataAdapter.Fill(table);

            if(table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

         public static string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel_management
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string pass = txtPass.Text;

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select UserInfo.UserID, UserInfo.Fullname, RoleType.Role from RoleType inner join UserInfo on UserInfo.RoleID = RoleType.RoleID where UserInfo.Email='{email}' and UserInfo.Password='{pass}'";
                
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                if (dt.Rows.Count != 1)
                {
                    MessageBox.Show("Invaild usename or password");
                    return;
                }

               string fullName = dt.Rows[0]["Fullname"].ToString();
               string userType = dt.Rows[0]["Role"].ToString();
               string userID = dt.Rows[0]["UserID"].ToString();

                ApplicationHelper.FullName = fullName;
                ApplicationHelper.UserType = userType;
                ApplicationHelper.UserID = userID;


                MessageBox.Show("Welcome, " + fullName);

                if (userType == "Admin" || userType == "Receptionist")
                {
                    FormAdministration fa = new FormAdministration();
                    fa.Show(this);
                    this.Hide();

                }
                else 
                {
                    FormCustomer fc = new FormCustomer();
                    fc.Show(this);
                    this.Hide();
                }

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void btnShow_Click(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = false;
            timer1.Start();
            btnShow.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar=true;
            timer1.Stop();
            btnShow.Visible=true;
        }

        private void lblSignUp_Click(object sender, EventArgs e)
        {
            FormSignup fsign = new FormSignup();
            fsign.Show(this);
            this.Hide();
        }

        private void lblForgot1_Click(object sender, EventArgs e)
        {
            FormForgotPass1 fp1 = new FormForgotPass1();
            fp1.Show();

        }


    }
}

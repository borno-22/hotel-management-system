using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace hotel_management
{
    public partial class FormSignup : Form
    {
        public FormSignup()
        {
            InitializeComponent();
        }

        private void FormSignup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Show();
        }




        private void btnSignup_Click(object sender, EventArgs e)
        {
            int roleID=ApplicationHelper.getRoleID();
            if (roleID == 0) 
            {
                MessageBox.Show("SignUp is temporary off.");
                return; 
            }
            string fullname = txtFname.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string gender = "";
            string address = txtAddress.Text;
            string password = txtPassword.Text;

            if (rdbMale.Checked)
            {
                gender += "Male";
            }
            else if (rdbFemale.Checked)
            {
                gender += "Female";
            }

            if (fullname == "" || email == "" || phone == "" || address == "" || password == "" || gender == "")
            {
                MessageBox.Show("Please input every field.");
                return;
            }


            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"insert into UserInfo  (Fullname,Email,Phone,Gender,Address,Password,RoleID)values('{fullname}','{email}','{phone}','{gender}','{address}','{password}','{roleID}')";
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("SignUp successfull");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            if (this.Owner != null)
            {
                this.Owner.Show();
                this.Close();
            }

        }


        private void lblLogin_Click(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Show();
                this.Close();
        }

        private void btnHidden_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
            btnHidden.Visible = false;
            btnEye.Visible = true;

        }

        private void btnEye_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            btnEye.Visible = false;
            btnHidden.Visible = true;
        }

        private void FormSignup_Load(object sender, EventArgs e)
        {

        }
    }
}

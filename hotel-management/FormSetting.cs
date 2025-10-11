using System;
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
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            this.LoadData();
        }

        //
        //get current info
        //
        private void LoadData()
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();


                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select * from UserInfo where UserID='{ApplicationHelper.UserID}'";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);


                txtID.Text = dt.Rows[0]["UserID"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();
                txtFname.Text = dt.Rows[0]["Fullname"].ToString();
                txtPhone.Text = dt.Rows[0]["Phone"].ToString();
                txtAddress.Text = dt.Rows[0]["Address"].ToString();
                dateDOB.Text = dt.Rows[0]["DOB"].ToString();
                string gender = dt.Rows[0]["Gender"].ToString();

                if (gender == "Male")
                    rdbMale.Checked = true;
                else if (gender == "Female")
                    rdbFemale.Checked = true;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //
        //btn save---generalinfo
        //
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.generalInfo();
        }

        //
        // general info update
        //
        private void generalInfo()
        {
            string id = txtID.Text;
            string fullname = txtFname.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string address = txtAddress.Text;
            string dob = dateDOB.Value.ToString("yyyy-MM-dd");
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string gender = "";

            if (rdbMale.Checked)
            {
                gender += "Male";
            }
            else if (rdbFemale.Checked)
            {
                gender += "Female";
            }

            if (fullname == "" || phone == "" || gender == "" || address == "")
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "";

            if (dob == today)
            {
                query = $"update UserInfo set Fullname='{fullname}',Phone='{phone}', Gender='{gender}', Address='{address}' Where UserID='{id}'";
            }
            else
            {
                query = $"update UserInfo set Fullname='{fullname}',Phone='{phone}', Gender='{gender}', Address='{address}', DOB='{dob}'  Where UserID='{id}'";
            }

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Record saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //
        //unable password section
        //
        private void ckbShowpass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.Enabled = ckbShowpass.Checked;
            txtNewPass.Enabled = ckbShowpass.Checked;
            btnConfirm.Enabled = ckbShowpass.Checked;
        }

        //
        //btn confirm-- change pass
        //
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.securityInfo();
        }

        //
        //change password
        //
        private void securityInfo()
        {
            string id = txtID.Text;
            string pass = txtPass.Text;
            string npass = txtNewPass.Text;

            if (pass == "" || npass == "")
            {
                MessageBox.Show("Please enter a valid password.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (pass == npass)
            {
                MessageBox.Show("The new password cannot be the same as the current password.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Password from UserInfo where UserID='{id}'";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                string dbPass = dt.Rows[0]["Password"].ToString();
                if (dbPass != pass)
                {
                    MessageBox.Show("Current password and new password do not match.", "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    con.Close();
                    return;
                }
                else
                {
                    cmd.CommandText = $"update UserInfo set Password= '{npass}' where UserID='{id}'";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

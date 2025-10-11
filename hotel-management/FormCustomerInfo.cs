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
    public partial class FormCustomerInfo : Form
    {
        public FormCustomerInfo()
        {
            InitializeComponent();
        }

        private void FormUserInfo_Load(object sender, EventArgs e)
        {
            this.LoadCustomerInfo();
        }
        private void LoadCustomerInfo()
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();


                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select * from UserInfo where RoleID=3";

                DataTable dt= new DataTable();
                SqlDataAdapter adp= new SqlDataAdapter(cmd);
                adp.Fill(dt);

                dgvCustomer.AutoGenerateColumns = false;
                dgvCustomer.DataSource = dt;
                dgvCustomer.Refresh();
                dgvCustomer.ClearSelection();

                con.Close();
            }

            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void defProperties() 
        {
            txtFname.Enabled = false;
            txtEmail.Enabled = false;
            txtPhone.Enabled = false;
            txtAddress.Enabled = false;
            pnlGender.Enabled = false;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenData();
        }

        private void OpenData()  //when btnNew is clicked
        {
            txtID.Text = "Auto Generate";
            txtFname.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            rdbMale.Checked = false;
            rdbFemale.Checked = false;


            txtFname.Enabled = true;
            txtEmail.Enabled = true;
            txtPhone.Enabled = true;
            pnlGender.Enabled = true;
            txtAddress.Enabled = true;

            dgvCustomer.ClearSelection();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvCustomer.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtFname.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtEmail.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
            string gender = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtAddress.Text = dgvCustomer.Rows[e.RowIndex].Cells[5].Value.ToString();
            
            if (gender == "Male")
            {
                rdbMale.Checked = true;
            }
            else
            {
                rdbFemale.Checked = true;
            }

            this.defProperties();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtFname.Enabled = true;
            txtPhone.Enabled = true;
            txtAddress.Enabled = true;
            pnlGender.Enabled = true;

            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select a Customer first");
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string query = "";
            string id = txtID.Text;
            string fullname = txtFname.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string gender = "";
            string address = txtAddress.Text;
            string password = txtPhone.Text;
            string roleID = "3";
          
            if (rdbMale.Checked)
            {
                gender += "Male";
            }
            else if (rdbFemale.Checked)
            {
                gender += "Female";
            }

            if (fullname == "" || email == "" || phone == "" || gender == "" || address == "" )
            {
                MessageBox.Show("Please input every field.");
                return;
            }



            if (id == "Auto Generate")
            {
                query = $"insert into UserInfo (Fullname,Email,Phone,Gender,Address,Password,RoleID) values ('{fullname}','{email}','{phone}','{gender}','{address}','{password}','{roleID}') ";
            }
            else
            {
                query = $"update  UserInfo set  Fullname='{fullname}',Phone='{phone}', Gender='{gender}', Address='{address}' Where UserID='{id}'";
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

                MessageBox.Show("saved");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.LoadCustomerInfo();
            this.defProperties();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            this.defProperties();

            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select a Employee first");
                return;
            }
            var result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"delete from UserInfo where UserID={id}";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Deleted");

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.LoadCustomerInfo();
        }
    }
}

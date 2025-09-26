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
    public partial class FormEmployeeInfo : Form
    {
        public FormEmployeeInfo()
        {
            InitializeComponent();
        }

        private void FormEmployeeInfo_Load(object sender, EventArgs e)
        {
            LoadEmployeeInfo();
        }

        private void LoadEmployeeInfo()
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select * from RoleType inner join UserInfo on UserInfo.RoleID = RoleType.RoleID where RoleType.Role<>'Customer'; select RoleID,Role from RoleType where Role<>'Customer'";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                dgvEmployee.AutoGenerateColumns = false;
                dgvEmployee.DataSource = ds.Tables[0];
                dgvEmployee.Refresh();
                dgvEmployee.ClearSelection();

                cmbRole.DataSource = ds.Tables[1];
                cmbRole.DisplayMember = "Role";
                cmbRole.ValueMember = "RoleID";
                cmbRole.SelectedIndex = -1;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void defProperties()  //Properties Enable=false
        {
            txtFname.Enabled = false;
            txtEmail.Enabled = false;
            txtPhone.Enabled = false;
            txtAddress.Enabled = false;
            cmbRole.Enabled = false;
            pnlGender.Enabled = false;
        }


        private void dgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvEmployee.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtFname.Text = dgvEmployee.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtEmail.Text = dgvEmployee.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtPhone.Text = dgvEmployee.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtAddress.Text = dgvEmployee.Rows[e.RowIndex].Cells[6].Value.ToString();
            cmbRole.SelectedValue = dgvEmployee.Rows[e.RowIndex].Cells[9].Value.ToString();
            string gender = dgvEmployee.Rows[e.RowIndex].Cells[5].Value.ToString();
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

        private void btnNew_Click(object sender, EventArgs e)
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
            cmbRole.SelectedIndex = -1;
            rdbMale.Checked = false;
            rdbFemale.Checked = false;


            txtFname.Enabled = true;
            txtEmail.Enabled = true;
            txtPhone.Enabled = true;
            txtAddress.Enabled = true;
            cmbRole.Enabled = true;
            pnlGender.Enabled = true;

            dgvEmployee.ClearSelection();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtFname.Enabled = true;
            txtPhone.Enabled = true;
            txtAddress.Enabled = true;
            cmbRole.Enabled = true;
            pnlGender.Enabled = true;

            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select a employee first");
                return;
            }
        }

        private void NewDataSave()  //when btnSave is clicked
        {
            string query = "";
            string id = txtID.Text;
            string fullname = txtFname.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string gender = "";
            string address = txtAddress.Text;
            string password = txtPhone.Text;
            int roleID = -1;
            if (cmbRole.SelectedValue != null)
            {
                roleID = Convert.ToInt32(cmbRole.SelectedValue);
            }

            if (rdbMale.Checked)
            {
                gender += "Male";
            }
            else if (rdbFemale.Checked)
            {
                gender += "Female";
            }

            if (fullname == "" || email == "" || phone == "" || gender == "" || address == "" || roleID == -1)
            {
                MessageBox.Show("Please input every field.");
                return;
            }



            if (id == "Auto Generate")
            {
                query = $"insert into UserInfo (Fullname,Email,Phone,Gender,Address,Password,RoleID) values ('{fullname}','{email}','{phone}','{gender}','{address}','{password}',{roleID}) ";
            }
            else
            {
                query = $"update  UserInfo set  Fullname='{fullname}',Phone='{phone}', Gender='{gender}', RoleID='{roleID}', Address='{address}'  Where UserID={id}";
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

            this.LoadEmployeeInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.NewDataSave();
            this.LoadEmployeeInfo();
            this.defProperties();
        }

    }
}

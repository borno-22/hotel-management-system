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

        //
        // employee info form load
        //
        private void FormEmployeeInfo_Load(object sender, EventArgs e)
        {
            this.LoadEmployeeInfo();
            this.defProperties();
        }

        //
        // loading dgv+cmb
        //
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

        //
        //some default properties
        //
        private void defProperties()
        {
            txtFname.Enabled = false;
            txtEmail.Enabled = false;
            txtPhone.Enabled = false;
            txtAddress.Enabled = false;
            cmbRole.Enabled = false;
            pnlGender.Enabled = false;
            cmbRole.SelectedIndex = -1;
        }

        //
        // grid veiw click
        //
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
        }

        //
        //new btn
        //
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.NewData();
        }

        //
        //new btn click trigger
        //
        private void NewData()
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

        //
        // update btn click trigger
        //
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
                MessageBox.Show("Please select a row first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        //
        // btn delete
        //
        private void btnDel_Click(object sender, EventArgs e)
        {
            this.Delete();
            this.LoadEmployeeInfo();
            this.defProperties();
        }

        //
        // btn delete click trigger
        //
        private void Delete()
        {
            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select a row first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
                return;

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"delete from UserInfo where UserID='{id}'";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Deleted");

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //
        // btn save
        //
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Save();
            this.LoadEmployeeInfo();
            this.defProperties();
        }
        
        //
        //save function for both new+update
        //
        private void Save()
        {
            string query = "";
            string id = txtID.Text;
            string fullname = txtFname.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            string gender = "";
            string address = txtAddress.Text;
            string password = txtPhone.Text;
            string roleID = "";
            if (cmbRole.SelectedValue != null)
            {
                roleID = cmbRole.SelectedValue.ToString();
            }

            if (rdbMale.Checked)
            {
                gender += "Male";
            }
            else if (rdbFemale.Checked)
            {
                gender += "Female";
            }

            if (fullname == "" || email == "" || phone == "" || gender == "" || address == "" || roleID == "")
            {
                MessageBox.Show("Please fill in all the fields.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            if (id == "Auto Generate")
            {
                query = $"insert into UserInfo (Fullname,Email,Phone,Gender,Address,Password,RoleID) values ('{fullname}','{email}','{phone}','{gender}','{address}','{password}','{roleID}') ";
            }
            else
            {
                query = $"update  UserInfo set  Fullname='{fullname}',Phone='{phone}', Gender='{gender}', RoleID='{roleID}', Address='{address}'  Where UserID='{id}'";
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


    }
}

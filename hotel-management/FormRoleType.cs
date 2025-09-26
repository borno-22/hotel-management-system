using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel_management
{
    public partial class FormRoleType : Form
    {
        public FormRoleType()
        {
            InitializeComponent();
        }

        private void FormRoleType_Load(object sender, EventArgs e)
        {
            this.LoadRoleType();
        }
        private void LoadRoleType() 
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select * from RoleType";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                dgvRole.AutoGenerateColumns = false;
                dgvRole.DataSource = dt;
                dgvRole.Refresh();
                dgvRole.ClearSelection();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.NewData();
            txtRole.Enabled = true;
            txtDesc.Enabled = true;
        }

        private void NewData()
        {
            txtID.Text = "Auto Generate";
            txtRole.Text = "";
            txtDesc.Text = "";
            dgvRole.ClearSelection();
            this.LoadRoleType();

        }

        private void dgvRole_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) 
                return;

            txtID.Text = dgvRole.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtRole.Text = dgvRole.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtDesc.Text= dgvRole.Rows[e.RowIndex].Cells[2].Value.ToString();

            txtRole.Enabled = false;
            txtDesc.Enabled = false;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            if(id =="Auto Generate")
            {
                MessageBox.Show("Please select The Row First");
                return;
            }
            var result = MessageBox.Show("Are you sure?","Confirmation",MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"delete from RoleType where RoleID={id}";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Deleted");
                this.LoadRoleType();
                this.NewData();

                con.Close();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string id =txtID.Text;
            string role = txtRole.Text;
            string description = txtDesc.Text;
            int count = 0;
            string query = "";

            if (id == "Auto Generate")

            {
                query = $"insert into RoleType values ('{role}','{description}') ";
            }
            else
            {
                query = $"update  RoleType set  Role='{role}',Description='{description}'  Where RoleID={id}";
            }

            if (role == "" || description == "")
            {
                count++;
                MessageBox.Show("please fill the input");
            }
            if (count == 0)
            {

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
                    this.LoadRoleType();
                    this.NewData();

                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

       



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtRole.Enabled = true;
            txtDesc.Enabled = true;

            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select The Row First");
                return;
            }
        }

        private void btnSave_MouseHover(object sender, EventArgs e)
        {
            btnSave.BackColor = Color.FromArgb(17,61,50);
            btnSave.ForeColor = Color.White;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            btnSave.BackColor= Color.Transparent;
            btnSave.ForeColor= Color.FromArgb(17,61,50);
        }


    }
}

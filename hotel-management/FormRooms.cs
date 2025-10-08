using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel_management
{
    public partial class FormRooms : Form
    {
        public FormRooms()
        {
            InitializeComponent();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.NewData();
            txtID.Enabled = false;
            txtNo.Enabled = true;
            cBType.Enabled = true;
            cBStatus.Enabled = true;


        }
        private void NewData()
        {
            txtID.Text = "Auto Generate";
            txtNo.Text = "";
            cBType.Text = null;
            cBStatus.Text = null;
            dgvRooms.ClearSelection();
            this.LoadRooms();

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string roomno = txtNo.Text;
            string type = cBType.SelectedValue.ToString();
            string status = cBStatus.Text; 
            int count = 0;
            string query = "";

            if (id == "Auto Generate")

            {
                query = $"insert into Rooms values ('{roomno}','{type}','{status}') ";
            }
            else
            {
                query = $"update  Rooms set  RoomNo='{roomno}',RoomTypeID='{type}',Status='{status}'  Where RoomID={id}";
            }

            if (roomno == "")
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
                    this.LoadRooms();
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
            string id= txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select The Row First");
                return;
            }

            txtNo.Enabled = true;
            cBType.Enabled = true;
            cBStatus.Enabled = true;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select The Row First");
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
                cmd.CommandText = $"delete from Rooms where RoomID={id}";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Deleted");
                this.LoadRooms();
                this.NewData();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormRooms_Load(object sender, EventArgs e)
        {
            this.LoadRooms();
            if(ApplicationHelper.UserType != "Admin")
            {
                pnlButtons.Visible = false;
                pnlInfo.Visible = false;
            }
        }
        private void LoadRooms()
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select Rooms.RoomID,Rooms.RoomNo,RoomType.RoomType,RoomType.Price,Rooms.Status,RoomType.RoomTypeID from Rooms inner join RoomType on Rooms.RoomTypeID=RoomType.RoomTypeID; select * from RoomType";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                dgvRooms.AutoGenerateColumns = false;
                dgvRooms.DataSource = ds.Tables[0];
                dgvRooms.Refresh();
                dgvRooms.ClearSelection();

                cBType.DataSource = ds.Tables[1];
                cBType.DisplayMember = "RoomType";
                cBType.ValueMember = "RoomTypeID";


                txtNo.Enabled = false;
                cBType.Enabled = false;
                cBStatus.Enabled = false;
                cBType.SelectedIndex = -1;


                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvRooms.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtNo.Text = dgvRooms.Rows[e.RowIndex].Cells[1].Value.ToString();
            cBType.SelectedValue = dgvRooms.Rows[e.RowIndex].Cells[5].Value.ToString();
            cBStatus.Text = dgvRooms.Rows[e.RowIndex].Cells[4].Value.ToString();

            txtNo.Enabled = false;
            cBType.Enabled = false;
            cBStatus.Enabled = false;
        }
    }
}

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
    public partial class FormRoomType : Form
    {
        public FormRoomType()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormRoomType_Load(object sender, EventArgs e)

        {
            this.LoadRoomType();
        }
        private void LoadRoomType()
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select * from RoomType";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                dgvRoomType.AutoGenerateColumns = false;
                dgvRoomType.DataSource = dt;
                dgvRoomType.Refresh();
                dgvRoomType.ClearSelection();

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
                txtID.Enabled = false;
                txtRoomType.Enabled = true;
                txtPrice.Enabled = true;


        }
        private void NewData()
        {
            txtID.Text = "Auto Generate";
            txtRoomType.Text = "";
            txtPrice.Text = "";
            dgvRoomType.ClearSelection();
            this.LoadRoomType();

        }

        private void dgvRoomType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvRoomType.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtRoomType.Text = dgvRoomType.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPrice.Text = dgvRoomType.Rows[e.RowIndex].Cells[2].Value.ToString();

            txtRoomType.Enabled = false;
            txtPrice.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string roomtype = txtRoomType.Text;
            string price = txtPrice.Text;
            int count = 0;
            string query = "";

            if (id == "Auto Generate")

            {
                query = $"insert into RoomType values ('{roomtype}','{price}') ";
            }
            else
            {
                query = $"update  RoomType set  RoomType='{roomtype}',Price='{price}'  Where RoomTypeID={id}";
            }

            if (roomtype == "" || price == "")
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
                    this.LoadRoomType();
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
            txtRoomType.Enabled = true;
            txtPrice.Enabled = true;

            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select The Row First");
                return;
            }
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
                cmd.CommandText = $"delete from RoomType where RoomTypeID={id}";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Deleted");
                this.LoadRoomType();
                this.NewData();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_MouseHover(object sender, EventArgs e)
        {
            btnSave.BackColor = Color.FromArgb(17, 61, 50);
            btnSave.ForeColor = Color.White;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            btnSave.BackColor = Color.Transparent;
            btnSave.ForeColor = Color.FromArgb(17, 61, 50);
        }

    }
}
            
        
    


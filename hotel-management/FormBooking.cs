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
    public partial class FormBooking : Form
    {
        public FormBooking()
        {
            InitializeComponent();
        }

        private void FormBooking_Load(object sender, EventArgs e)
        {
            this.LoadBooking();
        }

        private void LoadBooking()  //don't touch
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Booking.BookingID 'BookingID', UserInfo.Fullname 'Customer', Rooms.RoomNo 'Room', Booking.CheckIN 'CheckIN',Booking.CheckOut 'CheckOut', Booking.Status 'Status', Booking.CreateAt 'CreateAt' from Booking inner join UserInfo on UserInfo.UserID=Booking.UserID inner join Rooms on Booking.RoomID=Rooms.RoomID;  select distinct RoomType from Rooms where Status='Available'";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                dgvBooking.AutoGenerateColumns = false;
                dgvBooking.DataSource = ds.Tables[0];
                dgvBooking.Refresh();
                dgvBooking.ClearSelection();

                cmbStatus.DataSource = ds.Tables[0];
                cmbStatus.DisplayMember = "Status";
                cmbStatus.ValueMember = "BookingID";
                cmbStatus.SelectedIndex = -1;

                cmbType.DataSource = ds.Tables[1];
                cmbType.DisplayMember = "RoomType";
                //cmbType.ValueMember = "RoomID";   //for using distinct in 2nd query
                cmbType.SelectedIndex = -1;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)  //don't touch
        {
            if (cmbType.SelectedIndex != -1)
            {
                string selectedType = cmbType.Text;
                DateTime checkIn = dateCkIn.Value;
                DateTime checkOut = dateCkOut.Value;

                try
                {
                    var con = new SqlConnection();
                    con.ConnectionString = ApplicationHelper.connectionPath;
                    con.Open();

                    var cmd = new SqlCommand();
                    cmd.Connection = con;

                    cmd.CommandText = cmd.CommandText = $"select RoomID, RoomNo from Rooms where RoomType = '{selectedType}' and Status = 'Available' and RoomID not in (select RoomID from Booking where not (CheckOut <= '{checkIn:yyyy-MM-dd}' OR CheckIN >= '{checkOut:yyyy-MM-dd}'))";


                    DataTable dt = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    cmbRoom.DataSource = dt;
                    cmbRoom.DisplayMember = "RoomNo";
                    cmbRoom.ValueMember = "RoomID";
                    cmbRoom.SelectedIndex = -1;

                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }




        private void dgvBooking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCname.Text = dgvBooking.Rows[e.RowIndex].Cells[1].Value.ToString();
            cmbStatus.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();


            cmbType.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();
            cmbRoom.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtPrice.Text = dgvBooking.Rows[e.RowIndex].Cells[1].Value.ToString();

            this.defProperties();
        }


        private void LoadProperties()  
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select RoomType.RoomType 'Type', Rooms.RoomNo 'Room', RoomType.Price 'Price' from RoomType inner join Rooms on RoomTypeID=RoomID";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                dgvBooking.AutoGenerateColumns = false;
                dgvBooking.DataSource = ds.Tables[0];
                dgvBooking.Refresh();
                dgvBooking.ClearSelection();

                cmbStatus.DataSource = ds.Tables[0];
                cmbStatus.DisplayMember = "Status";
                cmbStatus.ValueMember = "BookingID";
                cmbStatus.SelectedIndex = -1;

                cmbType.DataSource = ds.Tables[1];
                cmbType.DisplayMember = "RoomType";
                //cmbType.ValueMember = "RoomID";   //for using distinct in 2nd query
                cmbType.SelectedIndex = -1;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }






        private void defProperties()  //Properties Enable=false
        {
            txtCname.Enabled = false;
            dateCkIn.Enabled = false;
            dateCkOut.Enabled = false;
            cmbType.Enabled = false;
            cmbRoom.Enabled = false;
            txtPrice.Enabled = false;
            cmbStatus.Enabled = false;
        }

        private void dateCkOut_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.OpenData();
        }

        private void OpenData()  //when btnNew is clicked
        {
            txtID.Text = "Auto Generate";
            txtCname.Text = "";
            cmbStatus.SelectedIndex = -1;

            dateCkIn.Enabled = true;
            dateCkOut.Enabled = true;
            cmbType.Enabled = true;
            cmbRoom.Enabled = true;
            cmbStatus.Enabled = true;

            dgvBooking.ClearSelection();
        }
    }
}

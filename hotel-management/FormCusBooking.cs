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
    public partial class FormCusBooking : Form
    {
        public FormCusBooking()
        {
            InitializeComponent();
        }

        private void FormCusBooking_Load(object sender, EventArgs e)
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
                cmd.CommandText = $"select * from RoomType";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                cmbType.DataSource = dt;
                cmbType.DisplayMember = "RoomType";
                cmbType.ValueMember = "RoomTypeID";

                cmbType.SelectedIndex = -1;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearchRoom_Click(object sender, EventArgs e)
        {
            this.LoadAvailableRoom();
        }

        private void LoadAvailableRoom()
        {
            string checkIn = dateCkIn.Value.ToString("yyyy-MMM-dd");
            string checkOut = dateCkOut.Value.ToString("yyyy-MMM-dd");
            string typeID = "";

            if (dateCkOut.Value <= dateCkIn.Value)
            {
                MessageBox.Show("Check-out date must be after check-in date.");
                return;
            }

            if (cmbType.SelectedValue == null)
            {
                MessageBox.Show("Please select a room type");
                return;
            }
            else
            {
                typeID=cmbType.SelectedValue.ToString();
            }


            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Rooms.RoomID, Rooms.RoomNo from Rooms where Rooms.RoomTypeID = '{typeID}' and Rooms.Status = 'Available' and Rooms.RoomID NOT IN (select Booking.RoomID from Booking where Booking.Status IN ('Confirmed','Checked-In') and (Booking.CheckIN <= '{checkOut}' and Booking.CheckOut >= '{checkIn}'))";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                cmbRoomNo.DataSource = dt;
                cmbRoomNo.DisplayMember = "RoomNo";
                cmbRoomNo.ValueMember = "RoomID";

                cmbRoomNo.SelectedIndex = -1;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
    }
}

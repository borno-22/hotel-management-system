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

        private void FormCusBooking_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Show();
        }



        private void FormCusBooking_Load(object sender, EventArgs e)
        {
            this.LoadRoomType();
            this.defProperties();
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

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void defProperties()
        {
            cmbRoomNo.Enabled = false;
            txtDuration.Enabled = false;
            txtAmount.Enabled = false;

            cmbType.SelectedIndex = -1;
            cmbRoomNo.SelectedIndex = -1;

        }





        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbRoomNo.SelectedIndex = -1;
            cmbRoomNo.Enabled = false;
        }

        private void btnSearchRoom_Click(object sender, EventArgs e)
        {
            this.LoadPrice();
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

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No available room.");
                    return;
                }

                cmbRoomNo.DataSource = dt;
                cmbRoomNo.DisplayMember = "RoomNo";
                cmbRoomNo.ValueMember = "RoomID";
                cmbRoomNo.Enabled = true;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadPrice()
        {
            int duration = (dateCkOut.Value.Date - dateCkIn.Value.Date).Days;

            if(duration ==0 || cmbType.SelectedValue==null)
                return;

            txtDuration.Text = duration.ToString();

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Price from RoomType where RoomTypeID='{cmbType.SelectedValue}'";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                int amount = duration * Convert.ToInt32(dt.Rows[0][0]);
                txtAmount.Text = amount.ToString();
                ApplicationHelper.Amount = txtAmount.Text;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }




        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Submit();
        }

        private void Submit()
        {
            if (cmbRoomNo.SelectedIndex==-1)
            {
                MessageBox.Show("Please input all field");
                return;
            }

            string guestID = ApplicationHelper.UserID;
            string roomID = cmbRoomNo.SelectedValue.ToString();
            string checkIn = dateCkIn.Value.ToString("yyyy-MM-dd");
            string checkOut = dateCkOut.Value.ToString("yyyy-MM-dd");
            string status = "Pending";

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"insert into Booking (UserID,RoomID,CheckIN,CheckOut,Status) values ('{guestID}','{roomID}','{checkIn}','{checkOut}','{status}')";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Request sent");


                cmd.CommandText = $"select Booking.BookingID from Booking where UserID='{guestID}' and RoomID='{roomID}' and CheckIN='{checkIn}' and CheckOut='{checkOut}'";
                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                ApplicationHelper.BookingID = dt.Rows[0][0].ToString();

                FormCusBilling cusBilling = new FormCusBilling();
                cusBilling.ShowDialog();
                this.Close();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}

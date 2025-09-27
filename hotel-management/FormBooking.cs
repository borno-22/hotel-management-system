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
                cmd.CommandText = $"select Booking.BookingID 'BooingID', UserInfo.Fullname 'Customer', Rooms.RoomNo 'Room', Booking.CheckIN 'CkIn', Booking.CheckOut 'CkOut', Booking.Status 'Status', Booking.CreateAt 'CreateAt', RoomType.RoomTypeID, Rooms.RoomID from Booking inner join UserInfo on UserInfo.UserID=Booking.UserID inner join Rooms on Booking.RoomID=Rooms.RoomID inner join RoomType on RoomType.RoomTypeID=Rooms.RoomTypeID; select * from RoomType; select * from Rooms";
                
                
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                dgvBooking.AutoGenerateColumns = false;
                dgvBooking.DataSource = ds.Tables[0];
                dgvBooking.Refresh();
                dgvBooking.ClearSelection();

                cmbType.DataSource = ds.Tables[1];
                cmbType.DisplayMember = "RoomType";
                cmbType.ValueMember = "RoomTypeID";
                cmbType.SelectedIndex = -1;

                cmbRoom.DataSource = ds.Tables[2];
                cmbRoom.DisplayMember = "RoomNo";
                cmbRoom.ValueMember = "RoomID";
                cmbRoom.SelectedIndex = -1;

                //cmbStatus.Items.Clear();
                //cmbStatus.Items.Add("Booked");
                //cmbStatus.Items.Add("CheckedIn");
                //cmbStatus.Items.Add("CheckedOut");
                //cmbStatus.Items.Add("Cancelled");

                //cmbStatus.DataSource = ds.Tables[0];
                //cmbStatus.DisplayMember = "Status";
                //cmbStatus.ValueMember = "BookingID";
                //cmbStatus.SelectedIndex = -1;

                //// Room Types
                //cmbType.DataSource = ds.Tables[1];
                //cmbType.DisplayMember = "RoomType";
                //cmbType.ValueMember = "RoomTypeID";
                //cmbType.SelectedIndex = -1;

                //// Rooms
                //cmbRoom.DataSource = ds.Tables[2];
                //cmbRoom.DisplayMember = "RoomNo";
                //cmbRoom.ValueMember = "RoomID";
                //cmbRoom.SelectedIndex = -1;

                //// Status (manual, not from DB)
                //cmbStatus.Items.Clear();
                //cmbStatus.Items.AddRange(new string[] { "Booked", "Pending", "CheckedIn", "CheckedOut" });
                //cmbStatus.SelectedIndex = -1;


                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void AvailableRoomType()  //don't touch
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select distinct RoomType from Rooms where Status='Available'";


                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                cmbType.DataSource = dt;
                cmbType.DisplayMember = "RoomType";
                //cmbType.ValueMember = "RoomID";
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




        private void dgvBooking_CellClick(object sender, DataGridViewCellEventArgs e)  //don't touch
        {
            //if (e.RowIndex < 0)
            //    return;

            //txtID.Text = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();
            //txtCname.Text = dgvBooking.Rows[e.RowIndex].Cells[1].Value.ToString();
            //// txtPrice.Text = dgvBooking.Rows[e.RowIndex].Cells[7].Value.ToString();

            //cmbType.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[7].Value.ToString();
            //cmbRoom.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[2].Value.ToString();
            //cmbStatus.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[8].Value.ToString();

            //this.defProperties();
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


        private void btnNew_Click(object sender, EventArgs e)
        {
            this.OpenData();
            this.AvailableRoomType();
        }

        private void OpenData()  //when btnNew is clicked
        {
            dgvBooking.ClearSelection();

            txtID.Text = "Auto Generate";
            txtCname.Text = "";
            txtPrice.Text = "";
            cmbStatus.Text=null;

            dateCkIn.Enabled = true;
            dateCkOut.Enabled = true;
            cmbType.Enabled = true;
            cmbRoom.Enabled = true;
            cmbStatus.Enabled = true;

        }
    }
}

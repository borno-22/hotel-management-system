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
    public partial class FormBookingMG : Form
    {
        public FormBookingMG()
        {
            InitializeComponent();
        }

        private void FormBookingMG_Load(object sender, EventArgs e)
        {
            this.defProperties();
            this.LoadBookingData();
        }

        private void LoadBookingData()  //load done
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Booking.BookingID 'BookingID' , UserInfo.Fullname 'Fullname', UserInfo.UserID 'UserID' , UserInfo.Phone 'Phone', RoomType.RoomType 'RoomType', Rooms.RoomNo 'RoomNo', Booking.CheckIN 'CheckIn', Booking.CheckOut 'CheckOut', Booking.Status 'Status', RoomType.RoomTypeID 'RoomTypeID', Rooms.RoomID 'RoomID' from Booking inner join UserInfo on Booking.UserID=UserInfo.UserID inner join Rooms on Booking.RoomID=Rooms.RoomID inner join RoomType on RoomType.RoomTypeID=Rooms.RoomTypeID; select * from RoomType; select * from Rooms;";

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

                cmbRoom.DataSource = ds.Tables[2];
                cmbRoom.DisplayMember = "RoomNo";
                cmbRoom.ValueMember = "RoomID";

                cmbStatus.DataSource = ds.Tables[0];
                cmbStatus.DisplayMember = "Status";
                cmbStatus.ValueMember = "BookingID";

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvBooking_CellClick(object sender, DataGridViewCellEventArgs e)   //click done
        {
            this.defProperties();

            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtGID.Text = dgvBooking.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPhone.Text = dgvBooking.Rows[e.RowIndex].Cells[3].Value.ToString();

            cmbType.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[9].Value.ToString();
            cmbRoom.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[10].Value.ToString();
            cmbStatus.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();

            dateCkIn.Value = Convert.ToDateTime(dgvBooking.Rows[e.RowIndex].Cells[6].Value);
            dateCkOut.Value = Convert.ToDateTime(dgvBooking.Rows[e.RowIndex].Cells[7].Value);

        }

        private void defProperties()  //Properties Enable=false
        {
            txtID.Enabled = false;
            txtGID.Enabled = false;
            txtPhone.Enabled = false;
            dateCkIn.Enabled = false;
            dateCkOut.Enabled = false;
            cmbType.Enabled = false;
            cmbRoom.Enabled = false;
            cmbStatus.Enabled = false;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this.NewData();
        }

        private void NewData()  //new btn click trigger-- done
        {
            dgvBooking.ClearSelection();

            txtID.Text = "Auto Generate";
            txtGID.Text = "";
            txtPhone.Text = "";

            dateCkIn.Value = DateTime.Today;
            dateCkOut.Value = DateTime.Today;

            cmbType.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;

            txtGID.Enabled = true;
            dateCkIn.Enabled = true;
            dateCkOut.Enabled = true;
            cmbType.Enabled = true;
            cmbRoom.Enabled = true;
            cmbStatus.Enabled = true;
            btnSave.Enabled = true;

        }



        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)  //selecting roomtype
        {
            if (cmbType.SelectedIndex < 0) return;

            int typeID;
            if (!int.TryParse(cmbType.SelectedValue?.ToString(), out typeID)) return;

            cmbRoom.DataSource = null;
            this.LoadAvailableRooms(typeID);
        }

        private void LoadAvailableRooms(int typeID)  //available rooms based on room type
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select RoomID, RoomNo from Rooms where RoomTypeID={typeID} and Status='Available'";

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

        private void btnUpdate_Click(object sender, EventArgs e)  //update btn click trigger-- done
        {
            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select The Row First");
                return;
            }

            dateCkIn.Enabled = true;
            dateCkOut.Enabled = true;
            cmbType.Enabled = true;
            cmbRoom.Enabled = true;
            cmbStatus.Enabled = true;
            btnSave.Enabled = true;

        }

        private void btnSave_Click(object sender, EventArgs e)  //save done
        {

            if (string.IsNullOrWhiteSpace(txtGID.Text) || cmbRoom.SelectedValue == null || string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Please fill all the inputs");
                return;
            }

            string id = txtID.Text;
            int guestID = Convert.ToInt32(txtGID.Text);
            int roomID = Convert.ToInt32(cmbRoom.SelectedValue);
            DateTime checkIn = dateCkIn.Value;
            DateTime checkOut = dateCkOut.Value;
            string status = cmbStatus.Text;

            string query = "";

            if (id == "Auto Generate")

            {
                query = $"insert into Booking (UserID,RoomID,CheckIN,CheckOut,Status) values ('{guestID}','{roomID}','{checkIn}','{checkOut}','{status}') ";
            }
            else
            {
                query = $"update  Booking set  UserID='{guestID}', RoomID='{roomID}', CheckIN='{checkIn}', CheckOut='{checkOut}', Status='{status}'  Where BookingID={id}";
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
                this.LoadBookingData();
                this.NewData();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnDel_Click(object sender, EventArgs e)  //delete done
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
                cmd.CommandText = $"delete from Booking where BookingID={id}";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Deleted");
                this.LoadBookingData();
                this.NewData();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}

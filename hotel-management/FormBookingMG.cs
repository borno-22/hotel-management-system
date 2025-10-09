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

        //
        // Booking form load
        //
        private void FormBookingMG_Load(object sender, EventArgs e)
        {
            this.LoadBookingData();
            this.defProperties();

            if (ApplicationHelper.UserType != "Admin")
                btnDel.Visible = false;
        }


        //
        // set some default properties
        //
        private void defProperties()
        {
            txtID.Enabled = false;
            txtGID.Enabled = false;
            txtGname.Enabled = false;
            txtPhone.Enabled = false;
            btnSearchPhone.Enabled = false;
            dateCkIn.Enabled = false;
            dateCkOut.Enabled = false;
            cmbType.Enabled = false;
            btnSearchRoom.Enabled = false;
            cmbRoom.Enabled = false;
            cmbStatus.Enabled = false;

            txtGID.Text = "";
            txtGname.Text = "";
            txtPhone.Text = "";
            txtDuration.Text = "";
            txtAmount.Text = "";

            dateCkIn.Value = DateTime.Today;
            dateCkOut.Value = DateTime.Today;

            cmbType.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }


        //
        // Refresh btn load
        //
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.LoadBookingData();
            this.defProperties();
        }


        //
        // Load dgv + cmb
        //
        private void LoadBookingData()
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Booking.BookingID , UserInfo.Fullname, UserInfo.UserID , UserInfo.Phone, RoomType.RoomType, Rooms.RoomNo, Booking.CheckIN, Booking.CheckOut, Booking.Status, RoomType.RoomTypeID, Rooms.RoomID, Bills.TotalAmount from Booking inner join UserInfo on Booking.UserID=UserInfo.UserID inner join Rooms on Booking.RoomID=Rooms.RoomID inner join RoomType on RoomType.RoomTypeID=Rooms.RoomTypeID  inner join Bills on Bills.BookingID=Booking.BookingID;\r\n" +
                        $" select * from RoomType;\r\n" +
                        $" select * from Rooms";

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


                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //
        // dgv click
        //
        private void dgvBooking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.defProperties();

            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtGname.Text = dgvBooking.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtGID.Text = dgvBooking.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPhone.Text = dgvBooking.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtAmount.Text = dgvBooking.Rows[e.RowIndex].Cells[11].Value.ToString();

            cmbStatus.Text = dgvBooking.Rows[e.RowIndex].Cells[8].Value.ToString();
            cmbType.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[9].Value.ToString();
            cmbRoom.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[10].Value.ToString();

            dateCkIn.Value = Convert.ToDateTime(dgvBooking.Rows[e.RowIndex].Cells[6].Value);
            dateCkOut.Value = Convert.ToDateTime(dgvBooking.Rows[e.RowIndex].Cells[7].Value);
        }


        //
        // new btn
        //
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.NewData();
        }


        //
        // new btn click trigger
        //
        private void NewData()
        {
            dgvBooking.ClearSelection();

            txtID.Text = "Auto Generate";
            txtGID.Text = "";
            txtGname.Text = "";
            txtPhone.Text = "";
            txtAmount.Text = "";

            dateCkIn.Value = DateTime.Today;
            dateCkOut.Value = DateTime.Today;

            cmbType.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;

            txtPhone.Enabled = true;
            btnSearchPhone.Enabled = true;
            dateCkIn.Enabled = true;
            dateCkOut.Enabled = true;
            cmbType.Enabled = true;
            btnSearchRoom.Enabled = true;
            btnSave.Enabled = true;           
        }

        
        //
        // Search guest for new booking
        //
        private void btnSearchPhone_Click(object sender, EventArgs e)
        {
            try
            {
                string phone = txtPhone.Text;

                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select UserInfo.UserID, UserInfo.Fullname from UserInfo where UserInfo.Phone='{phone}'";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                if (dt.Rows.Count != 1)
                {
                    MessageBox.Show("Guest not found");
                    return;
                }
                txtGID.Text = dt.Rows[0]["UserID"].ToString();
                txtGname.Text = dt.Rows[0]["Fullname"].ToString();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        //
        // change roomno cmb--- when roomtype change 
        //
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbRoom.SelectedIndex = -1;
            cmbRoom.Enabled = false;
        }
      

        //
        //room search
        //
        private void btnSearchRoom_Click(object sender, EventArgs e)
        {
            this.LoadAvailableRoom();
            this.LoadPrice();
        }
        

        //
        //find available room
        //
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
                typeID = cmbType.SelectedValue.ToString();
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

                cmbRoom.DataSource = dt;
                cmbRoom.DisplayMember = "RoomNo";
                cmbRoom.ValueMember = "RoomID";
                cmbRoom.Enabled = true;
                cmbStatus.Enabled = true;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        //
        // find price based on roomtype+duration
        //
        private void LoadPrice()
        {
            int duration = (dateCkOut.Value.Date - dateCkIn.Value.Date).Days;

            if (duration == 0 || cmbType.SelectedValue == null)
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


        //
        // update btn click trigger
        //
        private void btnUpdate_Click(object sender, EventArgs e)
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
            btnSearchRoom.Enabled = true;
            cmbStatus.Enabled = true; 
            btnSave.Enabled = true;
        }


        //
        // delete btn
        //
        private void btnDel_Click(object sender, EventArgs e)
        {
            this.Delete();
            this.LoadBookingData();
            this.defProperties();
        }

        //
        // delete btn click trigger
        //
        private void Delete()
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

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //
        // save btn
        //
        private void btnSave_Click(object sender, EventArgs e)  //save done
        {
            this.Save();
            this.LoadBookingData();
            this.defProperties();
        }


        //
        // save btn click trigger
        //
        private void Save()
        {
            string id = txtID.Text;
            string guestID = txtGID.Text;
            string roomID = cmbRoom.SelectedValue.ToString();
            string checkIn = dateCkIn.Value.ToString("yyyy-MM-dd");
            string checkOut = dateCkOut.Value.ToString("yyyy-MM-dd");
            string status = cmbStatus.Text;

            if (guestID == "" || roomID == "" || status == "")
            {
                MessageBox.Show("Please fill all the inputs");
                return;
            }

            string query = "";

            if (id == "Auto Generate")

            {
                query = $"insert into Booking (UserID,RoomID,CheckIN,CheckOut,Status) values ('{guestID}','{roomID}','{checkIn}','{checkOut}','{status}') ";
            }
            else
            {
                query = $"update Booking set RoomID='{roomID}', CheckIN='{checkIn}', CheckOut='{checkOut}', Status='{status}'  Where BookingID='{id}'";
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

                MessageBox.Show("Saved");


                if (id == "Auto Generate")
                {
                    cmd.CommandText = $"select BookingID from Booking where UserID='{guestID}' and RoomID='{roomID}' and CheckIN='{checkIn}' and CheckOut='{checkOut}'";
                    DataTable dt = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    ApplicationHelper.BookingID = dt.Rows[0][0].ToString();

                    FormCusBilling cusBilling = new FormCusBilling();
                    cusBilling.ShowDialog();
                }
                else if (status == "Checked-Out")
                {
                    cmd.CommandText = $"select Status from Bills where BookingID='{id}'";
                    DataTable dt = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    if (dt.Rows[0][0].ToString() == "Pending")
                    {
                        ApplicationHelper.BookingID = id;
                        ApplicationHelper.Amount = txtAmount.Text;
                        FormCusBilling cusBilling = new FormCusBilling();
                        cusBilling.ShowDialog();
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}

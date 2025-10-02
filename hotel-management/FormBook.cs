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
    public partial class FormBook : Form
    {
        public FormBook()
        {
            InitializeComponent();
        }

        private void FormBook_Load(object sender, EventArgs e)
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
                cmd.CommandText = $"select Booking.BookingID 'BookingID', UserInfo.Fullname 'Customer', Rooms.RoomNo 'Room', Booking.CheckIN 'CkIn', Booking.CheckOut 'CkOut', Booking.Status 'Status', Booking.CreateAt 'CreateAt',  RoomType.RoomTypeID 'RoomTypeID', Rooms.RoomID 'RoomID', RoomType.Price 'Price' from Booking inner join UserInfo on UserInfo.UserID=Booking.UserID inner join Rooms on Booking.RoomID=Rooms.RoomID inner join RoomType on RoomType.RoomTypeID=Rooms.RoomTypeID; select * from RoomType; select * from Rooms";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                //dgvBooking.AutoGenerateColumns = false;
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

                cmbStatus.DataSource = ds.Tables[0];
                cmbStatus.DisplayMember = "Status";
                cmbStatus.ValueMember = "BookingID";
                cmbStatus.SelectedIndex = -1;


                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dgvBooking_CellClick(object sender, DataGridViewCellEventArgs e)  //don't touch
        {
            this.defProperties();

            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCname.Text = dgvBooking.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPrice.Text = dgvBooking.Rows[e.RowIndex].Cells[9].Value.ToString();


            cmbType.SelectedValue = Convert.ToInt32(dgvBooking.Rows[e.RowIndex].Cells[7].Value);
            cmbRoom.SelectedValue = Convert.ToInt32(dgvBooking.Rows[e.RowIndex].Cells[8].Value);
            cmbStatus.SelectedValue = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();

            dateCkIn.Value = Convert.ToDateTime(dgvBooking.Rows[e.RowIndex].Cells[3].Value);
            dateCkOut.Value = Convert.ToDateTime(dgvBooking.Rows[e.RowIndex].Cells[4].Value);
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

        //private void AvailableRoomType()  //don't touch
        //{
        //    try
        //    {
        //        var con = new SqlConnection();
        //        con.ConnectionString = ApplicationHelper.connectionPath;
        //        con.Open();

        //        var cmd = new SqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandText = $"select distinct RoomType.RoomType,RoomType.RoomTypeID from Rooms inner join RoomType on Rooms.RoomTypeID = RoomType.RoomTypeID where Rooms.Status = 'Available'";


        //        DataTable dt = new DataTable();
        //        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //        adp.Fill(dt);

        //        cmbType.DataSource = dt;
        //        cmbType.DisplayMember = "RoomType";
        //        cmbType.ValueMember = "RoomTypeID";
        //        cmbType.SelectedIndex = -1;

        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void AvailableRoomType()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ApplicationHelper.connectionPath))
                {
                    con.Open();

                    string query = @"
                SELECT DISTINCT rt.RoomTypeID, rt.RoomType
                FROM Rooms r
                INNER JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
                WHERE r.RoomID NOT IN (
                    SELECT b.RoomID
                    FROM Booking b
                    WHERE (b.CheckIN < @CheckOut AND b.CheckOut > @CheckIn)
                )";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CheckIn", dateCkIn.Value);
                        cmd.Parameters.AddWithValue("@CheckOut", dateCkOut.Value);

                        DataTable dt = new DataTable();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);

                        cmbType.DataSource = dt;
                        cmbType.DisplayMember = "RoomType";
                        cmbType.ValueMember = "RoomTypeID";
                        cmbType.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtCname.Text) || cmbType.SelectedIndex == -1 || cmbRoom.SelectedIndex == -1 || cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            string id = txtID.Text;
            string customerName = txtCname.Text;
            string roomTypeID = cmbType.SelectedValue.ToString(); // Make sure you set ValueMember=RoomTypeID
            string roomNo = cmbRoom.SelectedValue.ToString();     // Make sure you set ValueMember=RoomNo
            string status = cmbStatus.Text; // Or SelectedValue if you bind status

            try
            {
                using (var con = new SqlConnection(ApplicationHelper.connectionPath))
                {
                    con.Open();

                    string query = "";

                    if (id == "Auto Generate")
                    {
                        query = @"INSERT INTO Booking (UserID, RoomNo, RoomTypeID, Status, CheckIN, CheckOut, CreateAt) 
                          VALUES (
                              (SELECT UserID FROM UserInfo WHERE Fullname=@CustomerName), 
                              @RoomNo, @RoomTypeID, @Status, @CheckIn, @CheckOut, GETDATE()
                          )";
                    }
                    else
                    {
                        query = @"UPDATE Booking 
                          SET RoomNo=@RoomNo, RoomTypeID=@RoomTypeID, Status=@Status, CheckIN=@CheckIn, CheckOut=@CheckOut 
                          WHERE BookingID=@BookingID";
                    }

                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerName", customerName);
                        cmd.Parameters.AddWithValue("@RoomNo", roomNo);
                        cmd.Parameters.AddWithValue("@RoomTypeID", roomTypeID);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@CheckIn", dateCkIn.Value);
                        cmd.Parameters.AddWithValue("@CheckOut", dateCkOut.Value);

                        if (id != "Auto Generate")
                            cmd.Parameters.AddWithValue("@BookingID", id);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Saved successfully!");
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            this.AvailableRoomType();
            this.OpenData();
            dgvBooking.ClearSelection();
        }

        private void OpenData()  //when btnNew is clicked
        {
            txtID.Text = "Auto Generate";
            txtCname.Text = "";
            txtPrice.Text = "";
            dateCkIn.Value = DateTime.Today;
            dateCkOut.Value = DateTime.Today;
            cmbType.SelectedIndex = -1;
            cmbRoom.SelectedIndex = -1;

            cmbStatus.DataSource=null;
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Check-In");
            cmbStatus.Items.Add("Check-Out");
            cmbStatus.Items.Add("Booked");
            cmbStatus.Items.Add("Pending");
            cmbStatus.Items.Add("Cancelled");
            cmbStatus.SelectedIndex = -1;

            dateCkIn.Enabled = true;
            dateCkOut.Enabled = true;
            cmbType.Enabled = true;
            cmbRoom.Enabled = true;
            cmbStatus.Enabled = true;

        }

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
            cmbRoom.Enabled = true;
            cmbStatus.Enabled = true;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel_management
{
    public partial class FormCusHistory : Form
    {
        public FormCusHistory()
        {
            InitializeComponent();
        }

        private void FormCusHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Show();
        }

        private void FormCusHistory_Load(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            string id = ApplicationHelper.UserID;

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Booking.BookingID, RoomType.RoomType, Rooms.RoomNo, Booking.CheckIN, Booking.CheckOut, Booking.Status from Booking inner join Rooms on Booking.RoomID=Rooms.RoomID inner join RoomType on RoomType.RoomTypeID=Rooms.RoomTypeID inner join UserInfo on UserInfo.UserID=Booking.UserID  where Booking.UserID='{id}'; \r\n" +
                    $"select Bills.BillsID,Bills.BookingID,Bills.TotalAmount,Bills.PaymentMethod,Bills.Status,Bills.GeneratedAt from Bills inner join Booking on Booking.BookingID=Bills.BookingID  where Booking.UserID='{id}';";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                dgvBooking.AutoGenerateColumns = false;
                dgvBooking.DataSource = ds.Tables[0];
                dgvBooking.Refresh();
                dgvBooking.ClearSelection();

                dgvPayment.AutoGenerateColumns = false;
                dgvPayment.DataSource = ds.Tables[1];
                dgvPayment.Refresh();
                dgvPayment.ClearSelection();

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}

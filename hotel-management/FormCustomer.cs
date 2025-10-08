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
    public partial class FormCustomer : Form
    {
        public FormCustomer()
        {
            InitializeComponent();
        }

        private void FormCustomer_Load(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void LoadData()
        {
            string fullname = ApplicationHelper.FullName;
            lblGreeting.Text = $"It's pleasure to see you again, {fullname}";

            string id = ApplicationHelper.UserID;

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select count(*) 'Tbook' from Booking where Booking.UserID='{id}' and Status not in ('Pending', 'Cancelled'); \r\n" +
                    $"select count(*) 'Pbook' from Booking where Booking.UserID = {id} and Status = 'Pending'; \r\n" +
                    $"select sum(TotalAmount) 'Spending' from Bills inner join Booking on Booking.BookingID = Bills.BookingID where Booking.UserID = {id} and Bills.Status = 'Paid'; \r\n" +
                    $"select sum(TotalAmount) 'Unpaid' from Bills inner join Booking on Booking.BookingID = Bills.BookingID where Booking.UserID = {id} and Bills.Status = 'Pending';";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                lblTbook.Text = ds.Tables[0].Rows[0][0].ToString();
                lblPbook.Text = ds.Tables[1].Rows[0][0].ToString();
                lblSpending.Text = ds.Tables[2].Rows[0][0].ToString();
                lblUnpaid.Text = ds.Tables[3].Rows[0][0].ToString();

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void FormCustomer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Show();
        }



        private void btnBooking_Click(object sender, EventArgs e)
        {
            FormCusBooking cusBooking = new FormCusBooking();
            cusBooking.Show(this);
            this.Hide();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            FormCusHistory cusHistory = new FormCusHistory();
            cusHistory.Show(this);
            this.Hide();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Show();
                this.Close();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            FormSetting setting = new FormSetting();
            setting.ShowDialog();
        }


    }
}

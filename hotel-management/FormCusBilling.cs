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
    public partial class FormCusBilling : Form
    {
        public FormCusBilling()
        {
            InitializeComponent();
        }

        private void FormCusBilling_Load(object sender, EventArgs e)
        {
            txtBookingID.Text = ApplicationHelper.BookingID;
            txtAmount.Text = ApplicationHelper.Amount;
        }


        private void btnPayNow_Click(object sender, EventArgs e)
        {
            this.PayNow();
        }

        private void PayNow()
        {
            string bookingID = ApplicationHelper.BookingID;
            string amount = ApplicationHelper.Amount;
            string method = cmbMethod.Text;
            string status = "Paid";

            if (method == "")
            {
                MessageBox.Show("Please select Payment Method.");
                return;
            }

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"insert into Bills (BookingID,TotalAmount,PaymentMethod,Status) values('{bookingID}','{amount}','{method}','{status}')";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Payment Successful");

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            this.Close();
        }



        private void btnPayLater_Click(object sender, EventArgs e)
        {
            this.PayLater();
        }

        private void PayLater()
        {
            string bookingID = ApplicationHelper.BookingID;
            string amount = ApplicationHelper.Amount;
            string method = "Cash";
            string status = "Pending";

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"insert into Bills (BookingID,TotalAmount,PaymentMethod,Status) values('{bookingID}','{amount}','{method}','{status}')";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pending payment");
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Close();
        }
    }
}

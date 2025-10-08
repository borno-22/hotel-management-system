﻿using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            this.defProperties();
        }

        private void defProperties()
        {
            pnlCancelBooking.Enabled = false;
            pnlBillPay.Enabled = false;
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
                cmd.CommandText = $"select Booking.BookingID, RoomType.RoomType, Rooms.RoomNo, Booking.CheckIN, Booking.CheckOut, Booking.Status from Booking inner join Rooms on Booking.RoomID=Rooms.RoomID inner join RoomType on RoomType.RoomTypeID=Rooms.RoomTypeID inner join UserInfo on UserInfo.UserID=Booking.UserID  where Booking.UserID='{id}' order by Booking.CreateAt desc; \r\n" +
                    $"select Bills.BillsID,Bills.BookingID,Bills.TotalAmount,Bills.PaymentMethod,Bills.Status,Bills.GeneratedAt from Bills inner join Booking on Booking.BookingID=Bills.BookingID  where Booking.UserID='{id}' order by Bills.GeneratedAt desc;";

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

                cmbMethod.SelectedIndex = -1;

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void dgvBooking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtBookingID.Text = dgvBooking.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtBookingStatus.Text = dgvBooking.Rows[e.RowIndex].Cells[5].Value.ToString();

            if (txtBookingStatus.Text == "Pending" || txtBookingStatus.Text == "Confirmed")
            {
                pnlCancelBooking.Enabled = true;
            }
            else
            {
                pnlCancelBooking.Enabled = false;
            }
        }

        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            string id = txtBookingID.Text;
            string status = "Cancelled";

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
                cmd.CommandText = $"update  Booking set Status='{status}' where BookingID='{id}'";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Booking Cancelled");

                this.LoadData();
                this.defProperties();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void dgvPayment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtBillingID.Text = dgvPayment.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtAmount.Text = dgvPayment.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtBillingStatus.Text = dgvPayment.Rows[e.RowIndex].Cells[4].Value.ToString();
            cmbMethod.Text = dgvPayment.Rows[e.RowIndex].Cells[3].Value.ToString();

            if (txtBillingStatus.Text == "Pending")
            {
                pnlBillPay.Enabled = true;
            }
            else
            {
                pnlBillPay.Enabled = false;
            }
        }

        private void btnPayNow_Click(object sender, EventArgs e)
        {
            if(cmbMethod.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a payment method.");
            }

            string id = txtBillingID.Text; 
            string method = cmbMethod.Text;
            string status = "Paid";

            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"update Bills set PaymentMethod='{method}', Status='{status}' where BillsID='{id}'";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Payment Completed");

                this.LoadData();
                this.defProperties();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}

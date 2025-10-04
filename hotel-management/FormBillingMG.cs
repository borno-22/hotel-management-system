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
    public partial class FormBillingMG : Form
    {
        public FormBillingMG()
        {
            InitializeComponent();
        }

        private void FormBillingMG_Load(object sender, EventArgs e)
        {
            this.defProperties();
            this.LoadBillingData();
        }

        private void defProperties()  //Properties Enable=false
        {
            txtID.Enabled = false;
            txtBookingID.Enabled = false;
            txtAmount.Enabled = false;
            cmbMethod.Enabled = false;
            cmbStatus.Enabled = false;
            btnSearchBkID.Enabled = false;
            btnSave.Enabled = false;
        }

        private void LoadBillingData()  // load bills
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Bills.BillsID, Bills.BookingID, UserInfo.Fullname, UserInfo.Phone, Bills.TotalAmount, Bills.PaymentMethod, Bills.Status, Bills.GeneratedAt from Bills inner join Booking on Bills.BookingID=Booking.BookingID inner join UserInfo on Booking.UserID=UserInfo.UserID;";

                DataTable dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                dgvBilling.AutoGenerateColumns = false;
                dgvBilling.DataSource = dt;
                dgvBilling.Refresh();
                dgvBilling.ClearSelection();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvBilling_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.defProperties();

            if (e.RowIndex < 0)
                return;

            txtID.Text = dgvBilling.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtBookingID.Text = dgvBilling.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtAmount.Text = dgvBilling.Rows[e.RowIndex].Cells[4].Value.ToString();
            cmbMethod.Text = dgvBilling.Rows[e.RowIndex].Cells[5].Value.ToString();
            cmbStatus.Text = dgvBilling.Rows[e.RowIndex].Cells[6].Value.ToString();

        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            this.NewData();
        }

        private void NewData()  //when new is clicked
        {
            dgvBilling.ClearSelection();

            txtID.Text = "Auto Generate";
            txtBookingID.Text = "";
            txtAmount.Text = "";

            cmbMethod.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;

            txtBookingID.Enabled = true;
            cmbMethod.Enabled = true;
            cmbStatus.Enabled = true;
            btnSearchBkID.Enabled = true;
            btnSave.Enabled = true;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            if (id == "Auto Generate")
            {
                MessageBox.Show("Please select The Row First");
                return;
            }

            cmbMethod.Enabled = true;
            cmbStatus.Enabled = true;
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string id = txtID.Text;
            string bookingID = txtBookingID.Text;
            string amount = txtAmount.Text;
            string method = cmbMethod.Text;
            string status = cmbStatus.Text;

            if (bookingID == "" || method == "" || status == "")
            {
                MessageBox.Show("Please fill all the inputs");
                return;
            }

            string query = "";

            if (id == "Auto Generate")

            {
                query = $"insert into Bills (BookingID,TotalAmount,PaymentMethod,Status) values ('{bookingID}','{amount}','{method}','{status}') ";
            }
            else
            {
                query = $"update Bills set BookingID='{bookingID}', TotalAmount='{amount}', PaymentMethod='{method}', Status='{status}'  Where BillsID={id}";
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
                this.LoadBillingData();
                this.NewData();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSearchBkID_Click(object sender, EventArgs e)  //search total amount
        {
            try
            {
                int bookingID = Int32.Parse(txtBookingID.Text);

                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select Booking.Duration*RoomType.Price from Booking inner join Rooms on Booking.RoomID = Rooms.RoomID inner join RoomType on Rooms.RoomTypeID = RoomType.RoomTypeID where Booking.BookingID={bookingID}";

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    txtAmount.Text = result.ToString();
                }
                else
                {
                    MessageBox.Show("Invalid Booking ID");
                    return;
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

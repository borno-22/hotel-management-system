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
    public partial class FormAdministration : Form
    {
        public FormAdministration()
        {
            InitializeComponent();
        }


        private void FormAdministration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Show();
        }

        private void FormAdministrator_Load(object sender, EventArgs e)
        {
            lblFullname.Text = $"{ApplicationHelper.FullName}";
            lblUserType.Text = $"{ApplicationHelper.UserType}";
            this.LoadData();
        }

        private void btnGuestMG_Click(object sender, EventArgs e)
        {
            FormCustomerInfo cs = new FormCustomerInfo();
            cs.ShowDialog();
        }

        private void btnBookingMG_Click(object sender, EventArgs e)
        {
            FormBookingMG bk = new FormBookingMG();
            bk.ShowDialog();
        }

        private void btnBillingMG_Click(object sender, EventArgs e)
        {
            FormBillingMG bl = new FormBillingMG();
            bl.ShowDialog();
        }

        private void btnRoomMG_Click(object sender, EventArgs e)
        {
            FormRooms rm = new FormRooms();
            rm.ShowDialog();
        }

        private void btnRoomTypeMG_Click(object sender, EventArgs e)
        {
            FormRoomType rmt = new FormRoomType();
            rmt.ShowDialog();
        }

        private void btnEmployeeMG_Click(object sender, EventArgs e)
        {
            FormEmployeeInfo em = new FormEmployeeInfo();
            em.ShowDialog();
        }

        private void btnRoleMG_Click(object sender, EventArgs e)
        {
            FormRoleType role = new FormRoleType();
            role.ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            FormSetting setting = new FormSetting();
            setting.ShowDialog();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (this.Owner != null) 
            {
                this.Owner.Show();
                this.Close();
            }
        }


        private void LoadData()
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select count(*) 'Employee' from UserInfo inner join RoleType on UserInfo.RoleID=RoleType.RoleID  where Role <> 'Customer';\r\n" +
                    $"select count(*) 'Customer' from UserInfo inner join RoleType on UserInfo.RoleID=RoleType.RoleID  where Role = 'Customer';\r\n" +
                    $"select count(*) 'Aroom' from Rooms where Status = 'Available';\r\n" +
                    $"select count(*) 'Oroom' from Rooms where Status = 'Booked';\r\n" +
                    $"select sum(TotalAmount) 'Revenue' from Bills;\r\n" +
                    $"select count(*) 'Pending' from Bills where Status = 'Pending';\r\n" +
                    $"select count(*) 'Book' from Booking where cast(CreateAt as DATE) = cast(GETDATE() as DATE);\r\n" +
                    $"select count(*) 'Tbook' from Booking where Status <> 'Cancelled';\r\n" +
                    $"select count(*) 'Cbook' from Booking where Status = 'Cancelled';";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                lblEmp.Text = ds.Tables[0].Rows[0][0].ToString();
                lblGuest.Text = ds.Tables[1].Rows[0][0].ToString();
                lblAroom.Text = ds.Tables[2].Rows[0][0].ToString();
                lblOroom.Text = ds.Tables[3].Rows[0][0].ToString();
                lblRevenue.Text = ds.Tables[4].Rows[0][0].ToString();
                lblPending.Text = ds.Tables[5].Rows[0][0].ToString();
                lblBook.Text = ds.Tables[6].Rows[0][0].ToString();
                lblTbook.Text = ds.Tables[7].Rows[0][0].ToString();
                lblCbook.Text = ds.Tables[8].Rows[0][0].ToString();

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }


    }
}

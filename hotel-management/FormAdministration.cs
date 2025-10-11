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

        //
        //go to previous page
        //
        private void FormAdministration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Show();
        }

        //
        //role based dashboard load
        //
        private void FormAdministrator_Load(object sender, EventArgs e)
        {
            lblFullname.Text = $"{ApplicationHelper.FullName}";
            lblUserType.Text = $"{ApplicationHelper.UserType}";
            this.LoadData();
            this.empRestriction();
        }

        //
        //restriction for employee
        //
        private void empRestriction()
        {
            if (ApplicationHelper.UserType != "Admin")
            {
                btnRoomTypeMG.Visible=false;
                btnRoleMG.Visible=false;
                btnEmployeeMG.Visible=false;
                pnlEmp.Visible=false;
                pnlRevenue.Visible=false;
                pnlPending.Visible=false;
            }
        }

        //
        //all the labels info --counting
        //
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
                    $"select count(*) 'Troom' from Rooms where Status = 'Available';\r\n" +
                    $"select count(*) 'Oroom' from Booking where Status = 'Checked-In';\r\n" +
                    $"select sum(TotalAmount) 'Revenue' from Bills where Status ='Paid';\r\n" +
                    $"select sum(TotalAmount) 'Pending' from Bills where Status = 'Pending';\r\n" +
                    $"select count(*) 'Book' from Booking where cast(CreateAt as DATE) = cast(GETDATE() as DATE);\r\n" +
                    $"select count(*) 'Pbook' from Booking where Status = 'Pending';\r\n" +
                    $"select count(*) 'Cbook' from Booking where Status = 'Cancelled'; \r\n" +
                    $"select count(*) 'Paid' from Bills where Status = 'Paid';\r\n" +
                    $"select count(*) 'Unpaid' from Bills where Status = 'Pending';";

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);

                lblEmp.Text = ds.Tables[0].Rows[0][0].ToString();
                lblGuest.Text = ds.Tables[1].Rows[0][0].ToString();
                lblTroom.Text = ds.Tables[2].Rows[0][0].ToString();
                lblOroom.Text = ds.Tables[3].Rows[0][0].ToString();
                lblRevenue.Text = ds.Tables[4].Rows[0][0].ToString();
                lblPending.Text = ds.Tables[5].Rows[0][0].ToString();
                lblBook.Text = ds.Tables[6].Rows[0][0].ToString();
                lblPbook.Text = ds.Tables[7].Rows[0][0].ToString();
                lblCbook.Text = ds.Tables[8].Rows[0][0].ToString();
                lblPaidCount.Text = $"Total Revenue ({ds.Tables[9].Rows[0][0].ToString()})";
                lblPendingCount.Text = $"Pending Payment ({ds.Tables[10].Rows[0][0].ToString()})";

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //
        //btn refresh
        //
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        //
        //all the management --below
        //
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

        //
        //go to settings
        //
        private void btnSettings_Click(object sender, EventArgs e)
        {
            FormSetting setting = new FormSetting();
            setting.ShowDialog();
        }

        //
        //logout
        //
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (this.Owner != null) 
            {
                this.Owner.Show();
                this.Close();
            }
        }

    }
}

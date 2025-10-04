using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}

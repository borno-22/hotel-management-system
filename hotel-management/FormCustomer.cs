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
    public partial class FormCustomer : Form
    {
        public FormCustomer()
        {
            InitializeComponent();
        }

        private void FormCustomer_Load(object sender, EventArgs e)
        {

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
    }
}

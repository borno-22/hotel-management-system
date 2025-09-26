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

        }
    }
}

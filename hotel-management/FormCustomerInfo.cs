using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace hotel_management
{
    public partial class FormCustomerInfo : Form
    {
        public FormCustomerInfo()
        {
            InitializeComponent();
        }

        private void FormUserInfo_Load(object sender, EventArgs e)
        {
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();


                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select * from UserInfo where RoleID=3";

                DataTable dt= new DataTable();
                SqlDataAdapter adp= new SqlDataAdapter(cmd);
                adp.Fill(dt);

                dgvCustomer.AutoGenerateColumns = false;
                dgvCustomer.DataSource = dt;
                dgvCustomer.Refresh();
                dgvCustomer.ClearSelection();

                con.Close();
            }

            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}

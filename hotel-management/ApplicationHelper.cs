using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace hotel_management
{
    internal static class ApplicationHelper
    {
        public static string connectionPath = @"Data Source=DESKTOP-0E77KI6;Initial Catalog=HotelMSdb;Integrated Security=True;Encrypt=False";
        public static string FullName = "";
        public static string UserType = "";

        public static int getRoleID()     //for customerSignUp
        {
            int roleID = 0;
            try
            {
                var con = new SqlConnection();
                con.ConnectionString = ApplicationHelper.connectionPath;
                con.Open();

                var cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select RoleID from RoleType where Role = 'Customer'";

                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    roleID = Convert.ToInt32(result);
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return roleID;
        }
    }
}

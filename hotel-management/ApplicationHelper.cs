using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel_management
{
    internal static class ApplicationHelper
    {
        public static string connectionPath = @"Data Source=DESKTOP-0E77KI6;Initial Catalog=HotelMSdb;Integrated Security=True;Encrypt=False";
        public static string FullName = "";
        public static string UserType = "";
        public static string UserID = "";
        public static string BookingID = "";
        public static string Amount = "";

    }
}

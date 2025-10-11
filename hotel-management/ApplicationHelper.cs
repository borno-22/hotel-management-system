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
        public static string UserID = ""; //to get current user's id-- used in settings,CusBooking
        public static string BookingID = ""; //to get the bookingid in CusBilling
        public static string Amount = ""; //to get amount in CusBilling
        public static string BookingStaus = ""; //for CusBilling

    }
}

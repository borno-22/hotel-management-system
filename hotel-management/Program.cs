using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel_management
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormLogin());
            //Application.Run(new FormForgotPass1());
            //Application.Run(new FormForgotPass2());
            //Application.Run(new FormSignup());
            //Application.Run(new FormAdministration());
            //Application.Run(new FormCustomer());
            Application.Run(new FormCusBooking());
            //Application.Run(new FormRoleType());
            //Application.Run(new FormCustomerInfo());
            //Application.Run(new FormEmployeeInfo());
            //Application.Run(new FormRoomType());
            //Application.Run(new FormRooms());
            //Application.Run(new FormBookingMG());
            //Application.Run(new FormBillingMG());




        }
    }
}

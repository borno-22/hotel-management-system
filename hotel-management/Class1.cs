using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel_management
{
    internal class Class1
    {

        //private void btnSave_Click(object sender, EventArgs e)  //save btn from booking form
        //{

        //    if (string.IsNullOrWhiteSpace(txtGID.Text) || cmbRoom.SelectedValue == null || string.IsNullOrWhiteSpace(cmbStatus.Text))
        //    {
        //        MessageBox.Show("Please fill all the inputs");
        //        return;
        //    }

        //    string id = txtID.Text;
        //    int guestID = Int32.Parse(txtGID.Text);
        //    int roomID = Convert.ToInt32(cmbRoom.SelectedValue);
        //    DateTime checkIn = dateCkIn.Value;
        //    DateTime checkOut = dateCkOut.Value;
        //    string status = cmbStatus.Text;

        //    string query = "";

        //    if (id == "Auto Generate")

        //    {
        //        query = $"insert into Booking (UserID,RoomID,CheckIN,CheckOut,Status) values ('{guestID}','{roomID}','{checkIn}','{checkOut}','{status}') ";
        //    }
        //    else
        //    {
        //        query = $"update  Booking set  UserID='{guestID}', RoomID='{roomID}', CheckIN='{checkIn}', CheckOut='{checkOut}', Status='{status}'  Where BookingID={id}";
        //    }

        //    try
        //    {
        //        var con = new SqlConnection();
        //        con.ConnectionString = ApplicationHelper.connectionPath;
        //        con.Open();

        //        var cmd = new SqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandText = query;
        //        cmd.ExecuteNonQuery();

        //        MessageBox.Show("saved");
        //        this.LoadBookingData();
        //        this.NewData();

        //        con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}
    }
}

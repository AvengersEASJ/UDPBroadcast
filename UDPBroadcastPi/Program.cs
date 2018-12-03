using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDPNumberSenderBroad
{
    class Program
    {
        static void Main(string[] args)
        {

            const string conn = "Server=tcp:frienddb.database.windows.net,1433;Initial Catalog=FriendDatabase;Persist Security Info=False;User ID=avengers;Password=Mads12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            UdpClient udpClient = new UdpClient(0);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Broadcast, 9999);




            while (true)
            {

                string sql = "SELECT * FROM FRIENDS WHERE friendsID = 1";

                using (var databaseConnection = new SqlConnection(conn))
                {
                    databaseConnection.Open();

                    using (var selectCommand = new SqlCommand(sql, databaseConnection))
                    {
                       // selectCommand.Parameters.AddWithValue("@friendsID", id);
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    int friendsID = reader.GetInt32(0);
                                    string friendsName = reader.GetString(1);
                                    bool gender = reader.GetBoolean(2);
                                    int thirst = reader.GetInt32(3);
                                    int hunger = reader.GetInt32(4);
                                    int task = reader.GetInt32(5);
                                    int fun = reader.GetInt32(6);
                                    int dress = reader.GetInt32(7);

                                    Byte[] sendBytes = Encoding.ASCII.GetBytes(friendsID + " " + friendsName + " " + gender + " " + thirst + " " + hunger +
                                                                               " " + task + " " + fun + " " + dress);
                                    udpClient.Send(sendBytes, sendBytes.Length, RemoteIpEndPoint);
                                    Console.Write("Message is: " + friendsID + " " + friendsName + " " + gender + " " + thirst + " " + hunger +
                                                                    " " + task + " " + fun + " " + dress + "\n");
                                    Thread.Sleep(1000);

                                }



                            }
                        }

                    }


                }


            }

        }
    }
}

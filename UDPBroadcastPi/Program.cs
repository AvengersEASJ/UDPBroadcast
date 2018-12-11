using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Threading;

namespace UDPNumberSenderBroad
{
    class Program
    {
        static void Main(string[] args)
        {

            const string conn = "Server=tcp:frienddb.database.windows.net,1433;Initial Catalog=FriendDatabase;Persist Security Info=False;User ID=avengers;Password=Mads12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            UdpClient udpClient = new UdpClient(0);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Broadcast, 4000);

           


            while (true)
            {

                string sql = "SELECT * FROM FRIENDS WHERE friendsID = 4";

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
                                    WebClient webClient = new WebClient();
                                    string reply = webClient.DownloadString($"http://api.openweathermap.org/data/2.5/weather?q=Roskilde&APPID=2f16a2d58ddb4a816fe93d7520731870");
                                    var kelvinTemp = JObject.Parse(reply)["main"]["temp"];
                                    int grader = (int)kelvinTemp - 273;

                                    string friendsName = reader.GetString(1);
                                    int friendsID = reader.GetInt32(0);                                    
                                    bool gender = reader.GetBoolean(2);
                                    int thirst = reader.GetInt32(3);
                                    int hunger = reader.GetInt32(4);
                                    int task = reader.GetInt32(5);
                                    int fun = reader.GetInt32(6);
                                    int degrees = grader;
                                    int dress = reader.GetInt32(7);
                                    

                                    Byte[] sendBytes = Encoding.ASCII.GetBytes(friendsID + " " + friendsName + " " + gender + " " + thirst + " " + hunger +
                                                                               " " + task + " " + fun + " " + " " + degrees + " " + dress);
                                    udpClient.Send(sendBytes, sendBytes.Length, RemoteIpEndPoint);
                                    Console.Write("Message is: " + friendsID + " " + friendsName + " " + gender + " " + thirst + " " + hunger +
                                                                    " " + task + " " + fun + " " + " " + degrees + " " + dress + "\n");

                                    
                                
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

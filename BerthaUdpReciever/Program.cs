using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BerthaUdpReciever
{
    class Program
    {

        static readonly temperature temperature = new temperature();
        static readonly Health health=new Health();
        static readonly HttpClient Client = new HttpClient();
        private static string TempUri = "https://birthawebservice20181031094923.azurewebsites.net/api/environment";
        private static string HealthUri = "https://birthawebservice20181031094923.azurewebsites.net/api/health";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter -T- for temp Data, -H- for health data  -R- for raspberry or stop \r\n");
            string option = Console.ReadLine();

            try
            {
                while (!string.Equals(option, "stop", StringComparison.Ordinal))
                {
                    if (option != null)
                    {
                        switch (option.ToUpper())
                        {
                            case "T":
                                Temp().GetAwaiter().GetResult();
                                break;
                            case "H":
                                Health().GetAwaiter().GetResult();

                                break;
                            case "R":
                                recieveFromRas().GetAwaiter().GetResult();
                                break;
                        }
                    }

                }
            }
           catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        static async Task AddToPost(temperature temperature)
        {
            var jsonString = JsonConvert.SerializeObject(temperature);
            //Console.WriteLine("JSON:: " + jsonString);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpResponseMessage response = await Client.PostAsync(TempUri, content);
                //Console.WriteLine(response);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        static async Task AddToPostHealth(Health health)
        {
            var jsonString = JsonConvert.SerializeObject(health);
            //Console.WriteLine("JSON:: " + jsonString);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpResponseMessage response = await Client.PostAsync(HealthUri, content);
                //Console.WriteLine(response);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }


       

        static async Task Health()
        {
            int UserId = 0;
            int BloodPressure = 0;
            int HeartBeat = 0;
            int Age = 0;
            int Weight = 0;

            DateTime date = DateTime.Now;

            UdpClient udpReceiver = new UdpClient(7000);

            // This IPEndPoint will allow you to read datagrams sent from any ip-source on port 9000


            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 9000);

            // receivingUdpClient.Connect(RemoteIpEndPoint); what is this used for ??

            // Blocks until a message returns on this socket from a remote host.
            Console.WriteLine("Health Receiver is blocked");

            try
            {
                while (true)
                {
                    Byte[] receiveBytes = udpReceiver.Receive(ref RemoteIpEndPoint);
                    string receivedData = Encoding.ASCII.GetString(receiveBytes);
                    if (receivedData.Equals("STOP.Secret")) throw new Exception("Receiver stopped");
                    Console.WriteLine("Sender: " + receivedData.ToString());

                    string[] textLines = receivedData.Split('\n');

                    for (int index = 0; index < textLines.Length; index++)
                        Console.Write(textLines[index]);
                    //0 Dat
                    //1 user id
                    //2 blood
                    //3 heart
                    //4 age  
                    //5we
                    //6
                    string[] list0 = textLines[0].Split();
                    string text0 = list0[1];
                    string[] list1 = textLines[1].Split();
                    string text1 = list1[1];
                    string[] list2 = textLines[2].Split();
                    string text2 = list2[1];
                    string[] list3 = textLines[3].Split();
                    string text3 = list3[1];
                    string[] list4 = textLines[4].Split();
                    string text4 = list4[1];
                    string[] list5 = textLines[5].Split();
                    string text5 = list5[1];

                    UserId = Int32.Parse(text1);
                    BloodPressure = Int32.Parse(text2);
                    HeartBeat = Int32.Parse(text3);
                    Age = Int32.Parse(text4);
                    Weight = Int32.Parse(text5);
                    


                    health.UserId = UserId;
                    health.BloodPressure = BloodPressure;
                    health.HeartBeat = HeartBeat;
                    health.Age = Age;
                    health.Weight = Weight;
                    health.DateTime = DateTime.Now;



                   AddToPostHealth(health).GetAwaiter().GetResult();



                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine();
        }



        static async Task Temp()
        {
            int UserId = 0;
            decimal Humidity = 0;
            decimal Temperature = 0;
            string Location = " ";


            DateTime date = DateTime.Now;

            UdpClient udpReceiver = new UdpClient(9000);

            // This IPEndPoint will allow you to read datagrams sent from any ip-source on port 9000


            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 7000);

            // receivingUdpClient.Connect(RemoteIpEndPoint); what is this used for ??

            // Blocks until a message returns on this socket from a remote host.
            Console.WriteLine("Receiver is blocked");

            try
            {
                while (true)
                {
                    Byte[] receiveBytes = udpReceiver.Receive(ref RemoteIpEndPoint);
                    string receivedData = Encoding.ASCII.GetString(receiveBytes);
                    if (receivedData.Equals("STOP.Secret")) throw new Exception("Receiver stopped");
                    Console.WriteLine("Sender: " + receivedData.ToString());

                    string[] textLines = receivedData.Split('\n');

                    for (int index = 0; index < textLines.Length; index++)
                        Console.Write(textLines[index]);

                    string[] list0 = textLines[0].Split();
                    string text0 = list0[1];
                    string[] list1 = textLines[1].Split();
                    string text1 = list1[1];
                    string[] list2 = textLines[2].Split();
                    string text2 = list2[1];
                    string[] list3 = textLines[3].Split();
                    string text3 = list3[1];
                    string[] list4 = textLines[4].Split();
                    string text4 = list4[1];

                    UserId = Int32.Parse(text1);
                    Humidity = decimal.Parse(text2);
                    Temperature = decimal.Parse(text3);
                    Location = text4;
                    temperature.UserId = UserId;
                    temperature.Humidity = Humidity;
                    temperature.Temperatur = Temperature;
                    temperature.Location = Location;
                    temperature.DateTime = date;


                    AddToPost(temperature).GetAwaiter().GetResult();



                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine();
        }

        static async Task recieveFromRas()
        {

            using (UdpClient socket = new UdpClient(new IPEndPoint(IPAddress.Any, 7000)))
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(0, 0);
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast {0}", socket.Client.LocalEndPoint);
                    byte[] datagramReceived = socket.Receive(ref remoteEndPoint);
                    string message = Encoding.ASCII.GetString(datagramReceived, 0, datagramReceived.Length);
                    Console.WriteLine("Receives {0} bytes from {1} port {2} message {3}", datagramReceived.Length,
                        remoteEndPoint.Address, remoteEndPoint.Port, message);
                    string[] textLines = message.Split('\n');
                    for (int index = 0; index < textLines.Length; index++)
                        Console.Write(textLines[index]);

                    string[] list0 = textLines[0].Split();
                    string text0 = list0[1];
                    string[] list1 = textLines[1].Split();
                    string text1 = list1[1];
                    int UserId = 90;
                    decimal Humidity = 0;
                    decimal Temperature = 0;
                    string Location = "Roskilde";
                    DateTime date = DateTime.Now;


                    Humidity = decimal.Parse(text0);
                    Temperature = decimal.Parse(text1);
                    temperature.UserId = UserId;
                    temperature.Humidity = Humidity;
                    temperature.Temperatur = Temperature;
                    temperature.Location = Location;
                    temperature.DateTime = date;

                    AddToPost(temperature).GetAwaiter().GetResult();

                }
            }
        }

    }
}

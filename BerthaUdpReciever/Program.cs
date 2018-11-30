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
        static readonly Temp te = new Temp();
        static readonly HttpClient Client = new HttpClient();
        private static string TempUri = "https://birthawebservice20181031094923.azurewebsites.net/api/environment";

        static void Main(string[] args)
        {

      
   
            int UserId = 0;
            decimal Humidity = 0;
            decimal Temperature = 0;
            string Location = " ";


            DateTime date = DateTime.Now;

            UdpClient udpReceiver = new UdpClient(9000);

            // This IPEndPoint will allow you to read datagrams sent from any ip-source on port 9000


            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 9000);

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

                    //1 id
                    //2 hum
                    //3 temp
                    //4 loc
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
                    Humidity = Int32.Parse(text2);
                    Temperature = Int32.Parse(text3);
                    Location = text4;
                    te.UserId = UserId;
                    te.Humidity = Humidity;
                    te.Temperatur = Temperature;
                    te.Location = Location;
                    te.DateTime = date;

                    AddToPost(te).GetAwaiter().GetResult();



                    Thread.Sleep(1000);

                    //number++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine();
        }

        static async Task AddToPost(Temp Temp)
        {
            var jsonString = JsonConvert.SerializeObject(Temp);
            Console.WriteLine("JSON " + jsonString);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpResponseMessage response = await Client.PostAsync(TempUri, content);
                Console.WriteLine(response);

                response.EnsureSuccessStatusCode();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }





        


        }
    }
}

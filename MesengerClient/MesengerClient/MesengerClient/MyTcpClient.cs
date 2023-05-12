using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KlientTcp
{
    public class MyTcpClient
    {
        private TcpClient myClient;
        private bool isRunning;
        private StreamReader reader;
        StreamReader Reader{ get; set; }

        public StreamWriter writer { get; set; }
        //StreamWriter Writer{ get; set; }

        public MyTcpClient(string ipAddress, int port)
        {
            myClient = new TcpClient(ipAddress, port);
            Thread thread = new Thread(new ParameterizedThreadStart(ClientLoop));
            thread.Start(myClient);
            isRunning = true;


        }

        private async void ClientLoop(object tcpClient)
        {
            Console.WriteLine("Klient byl spusten"); 
            try
            {
                TcpClient myClient = (TcpClient)tcpClient;
                reader = new StreamReader(myClient.GetStream(), Encoding.UTF8);
                writer = new StreamWriter(myClient.GetStream(), Encoding.UTF8);
                string? data = null;
                string? dataRecive = null;
                bool loginValid = false;

                while (isRunning)
                {
                    data = reader.ReadLine();
  //                  Console.WriteLine(data);
                    string[] word = data.Split(' ', 2);
                    switch (word[0])
                    {
                        case "print":
                            Console.WriteLine(word[1]);
                            break;
                        case "input":
                            dataRecive = "";
                            do
                            {
                                try
                                {

                                    Console.Write("\r>> ");
                                    dataRecive = MyReader.ReadLine(10000);
                                    writer.WriteLine(dataRecive);
                                    writer.Flush();
                                }
                                catch (TimeoutException)
                                {
                                    bool avail = myClient.GetStream().DataAvailable;
                                    if (avail)
                                    {
                                        Console.WriteLine("");
                                        break;
                                    }
                                        
                                }
                            } while (dataRecive =="");
                            break;
                        case "end":
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("unknown command " + word[0]);
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}

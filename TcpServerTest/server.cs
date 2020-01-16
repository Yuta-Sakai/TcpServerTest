using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace TcpServerTest
{
    class server
    {
        static void Main(string[] args)
        {
            RunServer();

            Console.ReadKey();
        }



        static async void RunServer()
        {
            var ip = IPAddress.Parse("192.168.102.123");
            var port = 12345;

            var tcpListener = new TcpListener(ip, port);

            tcpListener.Start();

            while (true)
            {
                using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
                using (var stream = tcpClient.GetStream())
                using (var reader = new StreamReader(stream))
                using (var writer = new StreamWriter(stream))
                {
                    // 接続元
                    Console.WriteLine(tcpClient.Client.RemoteEndPoint);

                    string line;
                    do
                    {
                        line = await reader.ReadLineAsync();
                        // 受信メッセージ
                        if (line != "")
                        {
                            Console.WriteLine($"recieve message:{line}");
                        };

                    } while (!String.IsNullOrWhiteSpace(line));


                    // 返信
                    await writer.WriteLineAsync($"Server has recieved your message"); // 終わり
                    await writer.WriteLineAsync(); // 終わり
                }

                Console.WriteLine("end connection");
            }
        }
    }
}

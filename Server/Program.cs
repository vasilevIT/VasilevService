using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Program pg = new Program();
            pg.run();
        }
          private TcpListener server;
            private List<TcpClient> clients = new List<TcpClient>();

            public void run()
            {
                server = new TcpListener(1970);
                server.Start();
                while (true)
                {
                    Log("Wait connect to server.");
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                        Log("Client " + clients.Count + " connected.");
                        clients.Add(client);
                        NetworkStream tcpStream = client.GetStream();
                        Byte[] bytes = new Byte[1024];
                        String temp = "";
                        int readByte = 0;
                        while ((readByte = tcpStream.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            temp = Encoding.Unicode.GetString(bytes).Trim();
                            int x = int.Parse(temp); //получили число от клиента
                            String responce = getFib(x).ToString();
                            Array.Clear(bytes, 0, bytes.Length);
                            bytes = Encoding.Unicode.GetBytes(x + " число Фиббоначи = " + responce);
                            tcpStream.Write(bytes, 0, bytes.Length);
                        }
                        Log("Client " + clients.Count + " task completed.");
                    }
                    catch(Exception e) {
                        Log("Exception: " + e.Message);
                    }
                }
            }

private void Log(string p)
{
 	Console.WriteLine(p);
}

            private void acceptClient(TcpClient client)
            {
                NetworkStream tcpStream = client.GetStream();
                Byte[] bytes = new Byte[1024];
                String temp = "";
                int readByte = 0;
                while((readByte = tcpStream.Read(bytes,0,bytes.Length)) > 0){
                    temp = Encoding.Unicode.GetString(bytes).Trim();
                    int x = int.Parse(temp); //получили число от клиента
                    String responce = getFib(x).ToString();
                    Array.Clear(bytes, 0, bytes.Length);
                    bytes = Encoding.Unicode.GetBytes(x+" число Фиббоначи = " + responce);
                    tcpStream.Write(bytes, 0, bytes.Length);
                }
            }
            private int getFib(int x)
            {
                if (x <= 2)
                {
                    return 1;
                }
                return getFib(x - 1) * getFib(x - 2);
            }

        }
    
}

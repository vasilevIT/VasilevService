using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace VasilevService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log("Service Start");
            Thread thread = new Thread(new ThreadStart((new myServer().run)));
            thread.Start();

        }

        protected override void OnStop()
        {
            Log("Service End");
        }


        private static void Log(String logMessage)
        {
            try
            {
                if(!EventLog.SourceExists("VasilevService")){
                    EventLog.CreateEventSource("VasilevService", "VasilevService");
                }
                EventLog eventLog = new EventLog();
                eventLog.Source = "VasilevService";
                eventLog.WriteEntry(logMessage);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private class myServer
        {
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
                        clients.Add(client);
                        Thread th = new Thread(() => acceptClient(client));
                        th.Start();
                    }
                    catch (Exception e)
                    {
                        Log("Exception: " + e.Message);
                    }
                }
            }

            private void acceptClient(TcpClient client)
            {
                        NetworkStream tcpStream = client.GetStream();
                        Byte[] bytes = new Byte[1024];
                        String temp = "";
                        int readByte = 0;
                        if ((readByte = tcpStream.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            temp = Encoding.Unicode.GetString(bytes).Trim();
                            int x =  Int32.Parse(temp); //получили число от клиента
                            String responce = getFib(x).ToString();
                            Array.Clear(bytes, 0, bytes.Length);
                            bytes = Encoding.Unicode.GetBytes(x+ " число Фиббоначи = " + responce);
                            tcpStream.Write(bytes, 0, bytes.Length);
                        }
            }
            private int getFib(int x)
            {
                if (x <= 2)
                {
                    return 1;
                }
                return getFib(x - 1) + getFib(x - 2);
            }

        }
    }
}

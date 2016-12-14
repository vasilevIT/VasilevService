using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true){
            TcpClient client = new TcpClient();
            client.Connect("localhost",1970);
            Console.WriteLine("Подключились к серверу. Введите число");
            String str = Console.ReadLine();
            NetworkStream tcpStream = client.GetStream();
            Byte[] bytes = Encoding.Unicode.GetBytes(str);

            tcpStream.Write(bytes, 0, bytes.Length);
            Console.WriteLine("Отправили число");

            bytes = new Byte[1024];
            if (tcpStream.Read(bytes, 0, bytes.Length) > 0)
            {
                String responce = Encoding.Unicode.GetString(bytes).Trim();
                Console.WriteLine("Ответ: " + responce);
            }
            else
            {
                Console.WriteLine("Ответ не получен.");
            }
            Console.ReadLine();
            }

        }
    }
}

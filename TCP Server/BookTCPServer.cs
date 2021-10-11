using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using BookClassLibrary;

namespace TCP_Server
{
    class BookTcpServer
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 4646);
            listener.Start();
            Console.WriteLine($"Server has started {IPAddress.Loopback}:4646");

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Task.Run(() => { HandleClient(socket); });
            }
        }
        public static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            BookRepo repo = new BookRepo();
            Console.WriteLine("Client connected");

            while (true)
            {
                string command = reader.ReadLine();
                string parameter = reader.ReadLine();

                if (command == "GetAll" && string.IsNullOrEmpty(parameter))
                {
                    var Books = repo.GetAll();

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(Books, options);


                    writer.WriteLine(jsonString);
                    writer.Flush();
                }
                else if (command == "Get" && BookRepo.BookList.Any(b => b.Isbn == parameter))
                {
                    Book foundBook = repo.Get(parameter);
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(foundBook, options);
                }



                writer.Flush();
            }
        }
    }
}

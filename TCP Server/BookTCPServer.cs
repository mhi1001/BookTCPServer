using System;
using System.Collections.Generic;
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
                    List<Book> books = repo.GetAll();

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(books, options);

                    writer.WriteLine(jsonString);
                    writer.Flush();
                }
                else if (command == "Get" && BookRepo.BookList.Any(b => b.Isbn == parameter))
                {
                    Book foundBook = repo.Get(parameter);

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(foundBook, options);

                    writer.WriteLine(jsonString);
                    writer.Flush();
                }
                else if (command == "Save" && !string.IsNullOrEmpty(parameter))
                {
                    try
                    {
                        Book newBook = JsonSerializer.Deserialize<Book>(parameter);
                        repo.Add(newBook);
                        writer.WriteLine("Book was added successfully");
                        writer.Flush();
                    }
                    catch (Exception e)
                    {
                        writer.WriteLine(e.Message);
                        writer.WriteLine("Invalid JSON, please check the formatting");
                        writer.Flush();
                    }
                }
                else
                {
                    writer.WriteLine("Invalid Command. Syntax is GetAll, Get or Save");
                    writer.WriteLine("GetAll is followed by an empty line. \n" +
                                     "Get is followed by the book isbn \n" +
                                     "Save is followed by the JSON of the book you want to add");
                    writer.Flush();
                }
            }
        }
    }
}

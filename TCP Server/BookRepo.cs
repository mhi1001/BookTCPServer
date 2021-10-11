using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookClassLibrary;

namespace TCP_Server
{
    public class BookRepo
    {
        public static readonly List<Book> BookList = new List<Book>()
        {
            new Book(){Author = "Pedro", Isbn = "01010101", PageNumber = 800, Title = "Nice"},
            new Book(){Author = "John", Isbn = "91919191", PageNumber = 800, Title = "IT Essentials"},
            new Book(){Author = "Jane", Isbn = "30303003", PageNumber = 800, Title = "I have no imagination"}
        };

        public List<Book> GetAll()
        {
            List<Book> result = new List<Book>(BookList);
            return result;
        }
        public Book Get(string isbn)
        {
            return BookList.Find(book => book.Isbn == isbn);
        }
        public Book Add(Book newBook)
        {
            BookList.Add(newBook);
            return newBook;
        }
    }
}

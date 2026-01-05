using LMS.Model.Models.Catalog;
using LMS.Model.Models.Catalog.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Interfaces
{
    public interface IBookRepository
    {
        int Add(Book book);
        Book GetById(int bookId);
        bool ISBNExists(string isbn);
        bool CallNumberExists(string callNumber);
        List<Book> GetAll();
    }
}
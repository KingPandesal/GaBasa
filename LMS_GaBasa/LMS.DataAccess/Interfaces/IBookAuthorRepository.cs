using LMS.Model.Models.Catalog.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Interfaces
{
    public interface IBookAuthorRepository
    {
        void Add(BookAuthor bookAuthor);
        List<BookAuthor> GetByBookId(int bookId);
    }
}

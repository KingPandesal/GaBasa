using LMS.Model.Models.Catalog.Books;
using System.Collections.Generic;

namespace LMS.DataAccess.Interfaces
{
    public interface IBookAuthorRepository
    {
        void Add(BookAuthor bookAuthor);
        List<BookAuthor> GetByBookId(int bookId);

        // Returns distinct AuthorIDs that appear in BookAuthor with the given role (e.g. "Editor")
        List<int> GetDistinctAuthorIdsByRole(string role);
    }
}

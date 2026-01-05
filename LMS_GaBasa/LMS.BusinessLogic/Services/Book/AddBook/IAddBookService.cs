using LMS.Model.DTOs.Book;

namespace LMS.BusinessLogic.Services.Book.AddBook
{
    public interface IAddBookService
    {
        BookCreationResultService CreateBook(DTOCreateBook dto);
    }
}

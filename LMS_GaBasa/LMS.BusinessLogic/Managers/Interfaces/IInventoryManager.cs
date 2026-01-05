using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Model.DTOs.Book;
using LMS.BusinessLogic.Services.Book.AddBook;

namespace LMS.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// Manages inventory operations: adding books, managing copies
    /// </summary>
    public interface IInventoryManager
    {
        BookCreationResultService AddBook(DTOCreateBook dto);
        bool ValidateBookData(DTOCreateBook dto, out string errorMessage);
    }
}

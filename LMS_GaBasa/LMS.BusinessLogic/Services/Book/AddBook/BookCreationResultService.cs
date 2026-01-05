using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BusinessLogic.Services.Book.AddBook
{
    public class BookCreationResultService
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public int BookID { get; private set; }

        // New: list of accession numbers created for initial copies
        public List<string> AccessionNumbers { get; private set; }

        private BookCreationResultService() { }

        public static BookCreationResultService Ok(int bookId, List<string> accessionNumbers = null)
        {
            return new BookCreationResultService
            {
                Success = true,
                BookID = bookId,
                AccessionNumbers = accessionNumbers ?? new List<string>()
            };
        }

        public static BookCreationResultService Fail(string message)
        {
            return new BookCreationResultService { Success = false, ErrorMessage = message };
        }

    }
}

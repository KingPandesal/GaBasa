using LMS.Model.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess.Interfaces
{
    public interface IAuthorRepository
    {
        int Add(Author author);
        Author GetById(int authorId);
        Author GetByName(string fullName);
        List<Author> GetAll();
    }
}

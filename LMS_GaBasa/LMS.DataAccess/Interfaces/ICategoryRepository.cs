using LMS.Model.Models.Catalog;
using System.Collections.Generic;

namespace LMS.DataAccess.Interfaces
{
    public interface ICategoryRepository
    {
        int Add(Category category);
        List<Category> GetAll();
        Category GetById(int categoryId);
        Category GetByName(string name);
    }
}

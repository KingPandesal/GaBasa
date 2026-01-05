using LMS.Model.DTOs.Book;
using LMS.Model.Models.Catalog;
using System.Collections.Generic;

namespace LMS.BusinessLogic.Managers.Interfaces
{
    /// <summary>
    /// Manages catalog operations: categories, languages, publishers
    /// </summary>
    public interface ICatalogManager
    {
        List<Category> GetAllCategories();
        Category GetOrCreateCategory(string categoryName);
        List<string> GetAllLanguages();
        void AddLanguageIfNotExists(string language);
    }
}